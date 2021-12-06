using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public float walkSpeed = 2;
    public float jumpPower = 20;
    private Animator anim;
    float x = 0;
    float y = 0;
    [SerializeField] float vx, vy;
    public Vector2 moveInput;
    int dir;
    int lastDir;
    int airJump;
    public bool Jumping;
    public bool wallJumping;
    public bool onObstacle;
    public bool propelling;
    public bool standing;
    public bool wall;

    //Settings settings;
    static int w = 24;
    static int h = 48;

    float propellingTime;

    bool shortjump;

    BoxCollider2D boxCollider2d;
    bool corner_correction = false;

    [SerializeField] private ContactFilter2D filter2d = default;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        airJump = Settings.Instance.AirJump;
    }

    // Update is called once per frame
    void Update()
    {
        control();
        move();

        //rigidbody2D.velocity = new Vector2(vx, rigidbody2D.velocity.y);

        anim.SetFloat("x", rigidbody2D.velocity.x);
        anim.SetFloat("y", rigidbody2D.velocity.y);


        bool ground = rigidbody2D.IsTouching(filter2d);
        x = Input.GetAxisRaw("Horizontal");
        if (ground)
        {
            anim.SetBool("Ground", true);
            onObstacle = true;
        }
        else
        {
            if (y <= 0)
            {
                anim.SetBool("Ground", false);
            }
            onObstacle = false;
        }

        if (rigidbody2D.velocity.y < 0)
        {
            anim.SetBool("isFalling", true);
            rigidbody2D.gravityScale = Settings.Instance.gravityFalling;
        }
        else
        {
            if (propelling)
            {
                rigidbody2D.gravityScale = 0;
            }
            else
            {
                rigidbody2D.gravityScale = Settings.Instance.gravityRising;
            }
            anim.SetBool("isFalling", false);
        }

        moveInput = new Vector2(vx, vy);
        anim.SetBool("allowAerialJump", Settings.Instance.allowAerialJump);
        if (wall)
        {
            anim.SetBool("wallSlide", true);
        }
    }

    private void FixedUpdate()
    {
        collisionTorelance();
        wall = false;
        anim.SetBool("wallSlide", false);

        Settings.Instance.jumperX = transform.position.x;
        Settings.Instance.jumperY = transform.position.y;
        Settings.Instance.jumperVX = rigidbody2D.velocity.x;
        Settings.Instance.jumperVY = rigidbody2D.velocity.y;
    }
    void velocityXUpdate()
    {
        float ax;
        float lastFramesVx = vx;
        vx = rigidbody2D.velocity.x;
        if (Jumping || (!onObstacle && !Settings.Instance.allowAerialWalk))
        {
            if (wallJumping && rigidbody2D.velocity.y > Settings.Instance.FallingLimitForwallJumpRigid && Settings.Instance.wallJumpRigid)
            {
                ax = 0;
            }
            else
            {
                ax = Settings.Instance.axJumping;
            }
        }
        else if (Mathf.Sign(vx) != dir)
        {
            ax = Settings.Instance.axBrake;
        }
        else
        {
            ax = Settings.Instance.axNormal;
        }
        if (dir != 0)
        {
            if (Jumping && !onObstacle && !Settings.Instance.allowAerialTurn)
            {
                //allowAerialTurn = true
            }
            else if (!onObstacle && Settings.Instance.allowAerialTurn)
            {
                if (!Settings.Instance.allowAerialFallingTurn)
                {
                    transform.localScale = new Vector3(dir, transform.localScale.y);
                }
                else
                {
                    float AerialTurnSpeedLimit = Settings.Instance.FallingSpeedLimitForAerialTurn;
                    if (rigidbody2D.velocity.y < AerialTurnSpeedLimit)
                    {
                        transform.localScale = new Vector3(dir, transform.localScale.y);
                    }
                }
            }
            else if (onObstacle)
            {
                transform.localScale = new Vector3(dir, transform.localScale.y);
            }
            anim.SetBool("running", true);
            vx += ax * dir;
            if (Settings.Instance.vxAdjustmentAtTakeoff > 0 && Jumping)
            {
                vx = Mathf.Clamp(vx, -Settings.Instance.maxVx * (1 + Settings.Instance.vxAdjustmentAtTakeoff), Settings.Instance.maxVx * (1 + Settings.Instance.vxAdjustmentAtTakeoff));
            }
            else
            {
                vx = Mathf.Clamp(vx, -Settings.Instance.maxVx, Settings.Instance.maxVx);
            }

        }
        else
        {
            anim.SetBool("running", false);
            vx -= ax * Mathf.Sign(vx);
            if (Mathf.Abs(vx) <= ax)
            {
                vx = 0;
            }
        }

        if (Settings.Instance.stopAndFall && !onObstacle && !Jumping)
        {
            vx = 0;
            anim.SetBool("running", false);
        }

        //vx = Mathf.Clamp(vx, -Settings.Instance.maxVx, Settings.Instance.maxVx);
        if (Mathf.Abs(vx) < Settings.Instance.maxVx)
        {
        }
        ax = (vx - lastFramesVx) / Time.deltaTime;
        Settings.Instance.jumperAX = ax;
        float RunningAnimSpeed;
        RunningAnimSpeed = 0.6f * (Mathf.Abs(rigidbody2D.velocity.x) / Settings.Instance.maxVx) + 0.4f;
        anim.SetFloat("RunningSpeed", RunningAnimSpeed);
    }

    void velocityYUpdate()
    {
        float ay;
        float lastFramesVy = vy;
        vy = rigidbody2D.velocity.y;

        if (Jumping == true && propelling == true && propellingTime <= Settings.Instance.maxPropellingFrames)
        {
            propellingTime += Time.deltaTime;
        }
        else if (Jumping && propelling && propellingTime > Settings.Instance.maxPropellingFrames)
        {
            propelling = false;
            rigidbody2D.gravityScale = Settings.Instance.gravityFalling;
        }

        if (onObstacle && !Jumping)
        {
            vy = 0;
        }

        if (rigidbody2D.velocity.y < -Settings.Instance.maxVy)
        {
            vy = -Settings.Instance.maxVy;
        }
        if (wall)
        {
            if (rigidbody2D.velocity.y > Settings.Instance.wallSlideMinSpeed)
            {
                vy = rigidbody2D.velocity.y * Settings.Instance.wallFriction;
            }
            else
            {
                vy = Settings.Instance.wallSlideMinSpeed;
            }
        }

        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, vy);
        ay = (vy - lastFramesVy) / Time.deltaTime;
        Settings.Instance.jumperAY = ay;
    }


    void move()
    {
        dir = (int)x;//キーの方向
        if (dir != lastDir)
        {
            Settings.Instance.dir = dir;
        }
        if (dir != 0)
        {
            lastDir = dir;
        }

        velocityXUpdate();
        velocityYUpdate();
    }
    void jumpStart()
    {
        float jumpAnticipationFrames = Settings.Instance.jumpAnticipationFrames;
        if (jumpAnticipationFrames == 0)
        {
            anim.SetFloat("jumpAnticipationFrames", 100);
        }
        else
        {
            anim.SetFloat("jumpAnticipationFrames", 1 / jumpAnticipationFrames);
        }
        if (!Jumping)
        {
            if (onObstacle && !wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (!onObstacle && wall)
            {
                anim.SetTrigger("Jump");
            }
            else if (!onObstacle && !wall)
            {

                anim.SetTrigger("Jump");
                anim.SetInteger("airJump", airJump);
            }
        }
        else if (airJump > 0)
        {
            anim.SetTrigger("Jump");
            anim.SetInteger("airJump", airJump);
        }
    }
    void jump()
    {
        if (!onObstacle && !wall)
        {
            airJump -= 1;
            if (Mathf.Sign(vx) != dir)
            {
                rigidbody2D.velocity = new Vector2(0, 0);
            }
        }
        else if (onObstacle && !wall)
        {
            airJump = Settings.Instance.AirJump;
        }
        anim.SetInteger("airJump", airJump);
        wallJumping = false;

        Jumping = true;
        Settings.Instance.jumping = Jumping;
        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
        collisionTorelance();
        if (Settings.Instance.maxPropellingFrames > 0)
        {
            propelling = true;
        }

        if (shortjump)
        {
            rigidbody2D.AddForce(new Vector2(0, 100 + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
            shortjump = false;
        }
        else
        {
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
        }
        vx = Settings.Instance.vxAdjustmentAtTakeoff * vx;
    }
    void wallJump()
    {
        if (Settings.Instance.allowWallJump)
        {
            transform.localScale = new Vector2(-transform.localScale.x, 1);
            rigidbody2D.velocity = new Vector2(Settings.Instance.maxVx * Settings.Instance.wallJumpSpeedRatio * transform.localScale.x, 0);
            rigidbody2D.AddForce(new Vector2(0, Settings.Instance.jumpVelocity + Mathf.Abs(vx) * Settings.Instance.jumpVelocityBonus));
            Jumping = true;
            wallJumping = true;
            Settings.Instance.jumping = Jumping;
        }
    }
    void jumpCanceled()
    {
        if (Jumping)
        {
            if (vy > 0) rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y * Settings.Instance.verticalSpeedSustainLevel);
        }
    }

    void control()
    {
        if (Input.GetKeyDown(KeyCode.Space) || ControllerManager.Instance.jumpButtonDown)
        {
            jumpStart();
            propellingTime = 0;
        }
        else if (Input.GetKeyUp(KeyCode.Space) || ControllerManager.Instance.jumpButtonUp)
        {
            jumpCanceled();
            propelling = false;
        }
        else if (Jumping && !Input.GetKey(KeyCode.Space))
        {
            jumpCanceled();
        }
    }

    void ground()
    {
        Jumping = false;
        wallJumping = false;
        Settings.Instance.jumping = Jumping;
        propelling = false;
        airJump = Settings.Instance.AirJump;
        anim.SetInteger("airJump", airJump);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "ground" && !onObstacle && transform.localScale.x == dir && Settings.Instance.allowWallSlide)
        {
            wall = true;
        }
    }

    void wallSlide()
    {
        Jumping = false;
        wallJumping = false;
        Settings.Instance.jumping = Jumping;
        propelling = false;
    }

    void collisionTorelance()
    {
        float offSet = Settings.Instance.Offset;
        float toleranceLength = Settings.Instance.collisionTolerance;
        float penalty = 0;
        Ray2D rayRight = new Ray2D((Vector2)transform.position + Vector2.up * Settings.Instance.collisionToleranceHeight + offSet * Vector2.right, Vector2.left);//右のRay
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + Vector2.up * Settings.Instance.collisionToleranceHeight + offSet * Vector2.left, Vector2.right);//左のRay

        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offSet * 2);
        //RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offSet * 2);
        Debug.DrawRay(rayRight.origin + Vector2.up * 0.01f, (toleranceLength) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (toleranceLength) * rayLeft.direction, Color.blue);

        corner_correction = false;

        if (hitCastRight.distance < Settings.Instance.collisionTolerance && hitCastLeft.distance > 0)//右方向のRay(左サイド)だけ始点が壁にめり込んでない
        {
            corner_correction = true;
            penalty = -(offSet * 2 - hitCastLeft.distance) - 0.05f;
        }
        else if (hitCastLeft.distance < Settings.Instance.collisionTolerance && hitCastRight.distance > 0)
        {
            corner_correction = true;
            penalty = offSet * 2 - hitCastRight.distance + 0.05f;
        }

        if (corner_correction == true && Jumping)
        {
            if (Mathf.Abs(penalty) < toleranceLength)
            {
                transform.position = new Vector2(transform.position.x + penalty, transform.position.y);
            }
        }
    }
}
