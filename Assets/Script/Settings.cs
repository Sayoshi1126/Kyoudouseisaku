using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Settings : SingletonMonoBehaviour<Settings>
{
    [HideInInspector] public float dir;

    [HideInInspector] public float jumperX;
    [HideInInspector] public float jumperY;
    [HideInInspector] public float jumperVX;
    [HideInInspector] public float jumperVY;
    [HideInInspector] public float jumperAX;
    [HideInInspector] public float jumperAY;

    [HideInInspector] public float cameraX;
    [HideInInspector] public float cameraY;
    [HideInInspector] public float cameraVX;
    [HideInInspector] public float cameraVY;

    [HideInInspector] public Rigidbody2D Masaorb2D;
    [HideInInspector] public Jumper jumper;
    [HideInInspector] public GameObject Masao;
    bool preShowTrail;

    public bool jumping;


    [PersistentAmongPlayMode] public bool allowAerialJump = true;// Allow aerial jump or not.
    [PersistentAmongPlayMode] public bool allowAerialWalk = true;// Allow aerial walk or not.
    [PersistentAmongPlayMode] public bool allowAerialTurn = false; //Allow aerial turn or not.
    [PersistentAmongPlayMode] public bool allowAerialFallingTurn = false; //Allow aerial turn or not.
    [PersistentAmongPlayMode] public float FallingSpeedLimitForAerialTurn = 10;//一定の落下速度に達するまでキャラクターの向きを変えられなくする
    [PersistentAmongPlayMode] public bool stopAndFall = true; // The hoizontal motion is halted when the jumper goes off the foothold.
    [PersistentAmongPlayMode] public bool allowWallJump; // Allow wall jump or not.
    [PersistentAmongPlayMode] public bool allowWallSlide; // Allow wall slide or not.
    [PersistentAmongPlayMode] public float maxVx = 8; // Maximum horizontal velocity of the jumper.
    [PersistentAmongPlayMode] public float maxVy = 30; // Maximum vertical velocity of the jumper. Limits only the falling motion.
    [PersistentAmongPlayMode] public float jumpVelocity = 13;// Initial vertical velocity of a jump motion.
    [PersistentAmongPlayMode] public float jumpVelocityBonus = 0;// The fasterrun gives an initial jump velocity bonus that allows a higher jump.
    [PersistentAmongPlayMode] public int jumpAnticipationFrames = 1; //Duration of the anticipation of jump motion in frames.
    [PersistentAmongPlayMode] public float vxAdjustmentAtTakeoff = 0.0f; //Horizontal velocity adjustment at the takeoff.
    [PersistentAmongPlayMode] public float maxPropellingFrames = 0;
    [PersistentAmongPlayMode] public float gravityRising = 0.5f;
    [PersistentAmongPlayMode] public float gravityFalling = 1.2f; // gravity when falling.
    [PersistentAmongPlayMode] public float verticalSpeedSustainLevel = 1.0f; // Sustain level of the vertical speed when the button released.
    [PersistentAmongPlayMode] public float axNormal = 0.2f; // Horizontal acceleration in normal state.
    [PersistentAmongPlayMode] public float axBrake = 1.0f; // Horizontal acceleration when braking.
    [PersistentAmongPlayMode] public float axJumping = 0.1f; // Horizontal acceleration when jumping.
    [PersistentAmongPlayMode] public float collisionTolerance = 0.1f; // Tolerance to automatically avoid blocks when jumping (in pixels).
    [PersistentAmongPlayMode] public float collisionToleranceHeight = 0.1f;//collision torelance縦の位置
    [PersistentAmongPlayMode] public float Offset = 0.1f;//collision torelanceの横の位置
    [PersistentAmongPlayMode] public float wallJumpSpeedRatio = 1.0f; // The velocity raito of the jumping speed of the wall jump to the maxVx.
    [PersistentAmongPlayMode] public float wallFriction = 0.9f;
    [PersistentAmongPlayMode] public float wallSlideMinSpeed = -3.0f;
    [PersistentAmongPlayMode] public bool wallJumpRigid = true;//壁ジャンしたとき一定の落下速度に達するまで操作できなくする
    [PersistentAmongPlayMode] public float FallingLimitForwallJumpRigid = 0f;//上の一定の速度を決める
    [PersistentAmongPlayMode] public int AirJump = 1;//空中ジャンプの回数

    // Camera parameters
    //public bool showCameraMarker = false; // Show the center marker or not.
    //public bool cameraEasing_x = false; // Ease the horizon camera motion or not.
    //public bool cameraEasing_y = true; // Ease the vertical camera motion or not.
    //public bool forwardFocus = false; //  Shift the focus to the front of the jumper to enable wide forward view.
    //public bool projectedFocus = false; // Shift the focus to the projeced position of the jumber, which means running faster moves the focal point farther.
    //public bool platformSnapping = false; // Halt the vertical camera motion when the jumper is jumping.
    //public float cameraEasingNormal_x = 0.1; // Smoothness of the horizontal camera motion in normal state.
    //public float cameraEasingNormal_y = 0.1; // Smoothness of the vertical camera motion in normal state.
    //public float cameraEasingGrounding_y = 0.2; // Smoothness of the camera motion when the jumper grounded.
    //public float cameraWindow_h = 0; // Height of the camera window.
    //public float cameraWindow_w = 0; // Width of the camera window.
    //public float focusDistance = 100; // Distance to the focal point.
    //public float focusingSpeed = 5; // Velocity of the focal point movement.
    [HideInInspector] public float bgScrollRatio = 0.5f; // Determine the scroll speed of the BG layer.

    // Misc. parameters
    [PersistentAmongPlayMode] public bool showVelocityChart = false; // Show velocity chart in the chart canvas.
    [PersistentAmongPlayMode] public bool showTrail = false;// Show the trail or not.
    [PersistentAmongPlayMode] public bool showAfterimage = false; // Show afterimage instead of red dots when 'showTrail' is true.
    [PersistentAmongPlayMode] public bool showInputStatus = false; // Show the input status.

    public enum CharaType
    {
        oneHead,
        threeHead,
        eightHead
    }
    [SerializeField] CharaType charaType = CharaType.oneHead;
    private CharaType changeChara;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

    }
    private void Start()
    {
        //Application.targetFrameRate = 60; //FPSを60に設定 

        Masao = GameManager.Instance.Masao;
        Masaorb2D = Masao.GetComponent<Rigidbody2D>();
        jumper = Masao.GetComponent<Jumper>();
        Masao.GetComponent<echoEffect>().enabled = showTrail;
        changeChara = charaType;
        //if (charaType == CharaType.oneHead)
        //{
        //    Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.OneHeadAnimator);
        //}
        //else if (charaType == CharaType.threeHead)
        //{
        //    Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.MasaoAnimator);
        //}
        //else if (charaType == CharaType.eightHead)
        //{
        //    Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.RealAnimator);
        //}
    }

    private void Update()
    {
        if(showTrail != preShowTrail)
        {
            Masao.GetComponent<echoEffect>().enabled = showTrail;
            preShowTrail = showTrail;
        }

        if (charaType != changeChara)
        {
            if (charaType == CharaType.oneHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.OneHeadAnimator);
            }
            else if (charaType == CharaType.threeHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.MasaoAnimator);
            }
            else if (charaType == CharaType.eightHead)
            {
                Masao.GetComponent<Animator>().runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(GameManager.Instance.RealAnimator);
            }
            changeChara = charaType;
        }
    }
}