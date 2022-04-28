using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーのアニメーションの遷移と座標計算を行う
/// </summary>
public class Jumper : PlayerMaster
{
    private Rigidbody2D _rigidbody2D;
    private Animator _anim;
    private float x, y;
    private float vx, vy;

    //プレイヤーの状態に関する変数
    private bool _jumping;
    private bool _wallJumping;
    private bool _onGround;
    private bool _propelling;
    private bool _onWall;
    private bool _avoidance;
    private bool _justAvoidance;
    public bool Invicible { get; set; }
    private bool _canControl = true;
    private bool _chasing;


    /// <summary>
    /// 以下のパラメタはインスペクタから値を設定する
    /// </summary>
    [SerializeField] private bool _allowAerialJump = true;
    [SerializeField] private bool _allowAerialWalk = true;
    [SerializeField] private bool _allowAerialTurn = false;
    [SerializeField] private bool _allowAerialFallingTurn = false;
    [SerializeField] private float _fallingSpeedLimitAerialTurn = 10;
    [SerializeField] private bool _stopAndFall = true;
    [SerializeField] private bool _allowWallJump;
    [SerializeField] private bool _allowWallSlide;
    [SerializeField] private float _maxVx = 8;
    [SerializeField] private float max_vy = 30;
    [SerializeField] private float _jumpPower = 13;
    [SerializeField] private float _jumpVelocityBonus = 0;
    [SerializeField] private int _jumpAnticipationFrames = 1;
    [SerializeField] private float _vxAdjustmentAtTakeoff = 0.0f;
    [SerializeField] private float _maxPropellingFrames = 0;
    [SerializeField] private float _gravityrising = 0.5f;
    [SerializeField] private float _gravityFalling = 1.2f;
    [SerializeField] private float _verticalSpeedSustainLevel = 1.0f;
    [SerializeField] private float _axNormal = 0.2f;
    [SerializeField] private float _axBrake = 1.0f;
    [SerializeField] private float _axJumping = 0.1f;
    [SerializeField] private float _collisionTolerance = 0.1f;
    [SerializeField] private float _collisionToleranceHeight = 0.1f;
    [SerializeField] private float _collisionToleranceOffset = 0.1f;
    [SerializeField] private float _wallJumpSpeedRatio = 1.0f; //この値だけ壁ジャンプが通常ジャンプより強く飛ぶ
    [SerializeField] private float _wallFriction = 0.9f;//壁の摩擦係数
    [SerializeField] private float _wallSlideMinSpeed = -3.0f;//壁すべりの最低速度
    [SerializeField] private bool _wallJumpTurnRestriction = true;//壁ジャンプの際に一定時間振り向けないようにする
    [SerializeField] private float _wallJumpTurnRestrictionByFallingSpeed = 0f;//_wallJumpRigit一定の速度に達するまで方向転換できないする
    [SerializeField] private int _airJumpNum = 1;//空中ジャンプの回数
    [SerializeField] private int _airAttackNum = 1;//ジャンプから着地までに空中攻撃を出せる回数

    [SerializeField] CheckColliderScript _stageCheck;//目の前に壁があるか判定するコライダーのオブジェクトをインスペクタから設定
    [SerializeField] private AvoidColliderScript _avoidColliderScript;
    [SerializeField] private HitBoxScript _chaseTackleHitbox;
    /// <summary>
    /// 未完成のパラメタ
    /// </summary>
    [SerializeField] private float _avoidSpeed;
    [SerializeField] private float _axAvoid;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _chasingTime;
    [SerializeField] private Vector2 _chaseTackleHitStep;
    [SerializeField] private float _chaseTackleLengthMax;

    [SerializeField] private Vector2 _wallSlideGunPos;
    [SerializeField] private Vector2 _normalGunPos;
    [SerializeField] private Vector2 _runningGunPos;
    [SerializeField] private Vector2 _jumpingGunPos;

    private int _playerDir;
    private int _airJump;
    private int _airAttack;
    public bool WitchTime;//ジャスト回避成功時にゲームをスローにするか判断するのに使用
    private float _distanceToEnemy;

    private float _propellingTime;

    [SerializeField] private ContactFilter2D _filter2D = default;


    private SpriteRenderer _spriteColor;
    public GameObject TargetEnemyObject { get; set; }

    [SerializeField] TimeScaleManager _timeScaleManager;

    public HitBoxScript[] AttackHitBox; 
    public int AttackAnimNum;//現在アニメーションしている攻撃を認識するの番号を保存
    private float _attackVelocityVx;
    private float _attackFroatingPower;
    private bool _attacking;
    private float _attackVelocityAx;

    private bool _attackBufferTime;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();        
        _spriteColor = GetComponent<SpriteRenderer>();

        //攻撃アクションで使用するオブジェクトのコンポーネントの取得
        AttackHitBox = new HitBoxScript[this.transform.GetChild(3).gameObject.transform.childCount];
        for (int i = 0; i < this.transform.GetChild(3).gameObject.transform.childCount; i++)
        {
            AttackHitBox[i] = this.transform.GetChild(3).gameObject.transform.GetChild(i).GetComponent<HitBoxScript>();
            AttackHitBox[i].GetBoxColliderComponent();
            AttackHitBox[i].DisActiveCollider();
        }

        //空中ジャンプの回数を初期化
        _airJump = _airJumpNum;

    }

    // Update is called once per frame
    void Update()
    {
        Control();
        _avoidColliderScript.lockOnPointer();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        //ジャンプ上昇中に自動で天井を避ける処理
        if (_rigidbody2D.velocity.y > 0)
        {
            CollisionTorelance();
        }

        //Settingsのプレイヤーのパラメタを更新
        JumperPos.x = transform.position.x;
        JumperPos.y= transform.position.y;
        JumperVel.x= _rigidbody2D.velocity.x;
        JumperVel.y= _rigidbody2D.velocity.y;
    }

    /// <summary>
    /// x軸でマイフレーム計算が必要な処理を行う
    /// </summary>
    void VelocityXUpdate()
    {
        float ax_;
        float pre_frame_vx_ = vx;
        vx = _rigidbody2D.velocity.x;

        //プレイヤーの状態に応じてx軸の加速度の切り替える処理===============================================================================================--
        if (_jumping || (!_onGround && !_allowAerialWalk))//空中にいるとき
        {
            if (_wallJumping && _rigidbody2D.velocity.y > _wallJumpTurnRestrictionByFallingSpeed && _wallJumpTurnRestriction)//壁ジャンプの場合はすぐには振り向けないようにする
            {
                ax_ = 0;
            }
            else {
                ax_ = _axJumping;
            }
        } else if (Mathf.Sign(vx) != _playerDir)//進行方向を向いていない時
        {
            ax_ = _axBrake;
        }
        else if (_avoidance && Mathf.Abs(vx) > 0.1f)//回避アクション時
        {
            ax_ = -_axAvoid;
        }
        else if(_attacking && Mathf.Abs(vx) > 0.1f)//攻撃アクション時
        {
            ax_ = -_attackVelocityAx;
        }
        else
        {
            ax_ = _axNormal;
        }

        //プレイヤーが横軸に入力していたときのキャラクターの向きの切り替え処理=============================================================================
        if(_playerDir!=0)//横軸に入力してた場合
        {

            if (_jumping && !_onGround&& !_allowAerialTurn)
            {
                //allowAerialTurn = true
            }
            else if(!_onGround && _allowAerialTurn)//ジャンプ時にキャラクターの向きを変えられる設定の場合
            {
                if (_allowAerialFallingTurn)//キャラクターの向きを変える条件に落下速度を追加する場合
                {
                    float AerialTurnSpeedLimit = _fallingSpeedLimitAerialTurn;
                    if (_rigidbody2D.velocity.y < AerialTurnSpeedLimit)
                    {
                        transform.localScale = new Vector3(_playerDir, transform.localScale.y);
                    }
                }
                else
                {
                    transform.localScale = new Vector3(_playerDir, transform.localScale.y);
                }
            }
            else if(_onGround)//足が地面についてるとき
            {
                transform.localScale = new Vector3(_playerDir, transform.localScale.y);
            }
            _anim.SetBool("running", true);

            //加速度とキャラクターの向きから速度を計算
            vx += ax_ * _playerDir;
            
            //速度が一定の値を超えないようにする処理
            if (_vxAdjustmentAtTakeoff > 0 && _jumping)//走りながらジャンプしたとき速度を上げる設定のときの速度制限
            {
                vx = Mathf.Clamp(vx, -_maxVx * (1 + _vxAdjustmentAtTakeoff), _maxVx * (1 + _vxAdjustmentAtTakeoff));
            }else if(_canControl== false)//操作不可の時の速度制限
            {
                vx = Mathf.Clamp(vx,-100,100);
            }
            else//通常の速度制限
            {
                vx = Mathf.Clamp(vx, -_maxVx, _maxVx);
            }

        }
        else//横入力してないとき
        {
            _anim.SetBool("running", false);

            //減速させる処理
            vx -= ax_ * Mathf.Sign(vx);
            if (Mathf.Abs(vx) <= ax_)//速度が十分に小さければ停止
            {
                vx = 0;
            }
        }

        //現在の速度に関わらず速度を上書きする必要がある場合の処理

        if(_stopAndFall&&!_onGround&&!_jumping)//崖から飛びだしたら速度を0にする設定の処理
        {
            vx = 0;
            _anim.SetBool("running",false);
        }

        if (_chasing)//体当たり時対象の敵に直進するためのx軸速度を計算
        {
            ax_ = 0;
            Vector2 _playerDir = TargetEnemyObject.transform.position - transform.position;
            vx = _playerDir.normalized.x * _distanceToEnemy * _chaseSpeed;//YvelocityUpdateにも同様の式があるので同時に編集すること
        }

        //Settingに加速度を記録
        ax_ = (vx - pre_frame_vx_) / Time.deltaTime;
        JumperAxcel.x = ax_;

        //現在速度に応じて走るアニメの速さを調整
        float running__anim_speed_ = 0.6f*(Mathf.Abs(_rigidbody2D.velocity.x) / _maxVx)+0.4f;
        _anim.SetFloat("RunningSpeed",running__anim_speed_);
    }

    /// <summary>
    /// y軸の速度において毎フレーム計算が必要な処理
    /// </summary>
    void VelocityYUpdate()
    {
        float ay_;
        float last_frame_vy_ = vy;
        vy = _rigidbody2D.velocity.y;

        //無重力ジャンプの設定時
        if(_jumping == true && _propelling == true&&_propellingTime<=_maxPropellingFrames)//上昇できなくなる制限時間に達してない場合制限時間を進める
        {
            _propellingTime += Time.deltaTime;
        }else if (_jumping&&_propelling&&_propellingTime > _maxPropellingFrames)//上昇できなくなる時間に達したとき重力を戻す
        {
            _propelling = false;
            _rigidbody2D.gravityScale = _gravityFalling;
        }

        if (_onGround && !_jumping && _canControl)//地面に足がついてるとき
        {
            vy = 0;
        }

        if (_rigidbody2D.velocity.y < -max_vy&&!_onGround)//y軸の速度をmaxVy以下にするw
        {
            vy = -max_vy;
        }

        //壁張り付き時
        if (_onWall)
        {
            if (_rigidbody2D.velocity.y > _wallSlideMinSpeed)//壁滑り時の速度が減速しきってないとき速度を減速する
            {
                vy = _rigidbody2D.velocity.y * _wallFriction;//壁滑りが最小速度まで減速しきったとき速度を維持
            }
            else
            {
                vy = _wallSlideMinSpeed;
            }
        }

        if (_chasing)//体当たり時対象の敵に直進するためのy軸速度を計算
        {
            Vector2 _playerDir = TargetEnemyObject.transform.position - transform.position;
            vy = _playerDir.normalized.y * _distanceToEnemy * _chaseSpeed;
        }
        
        //y軸の加速度をSettngsに保存
        ay_ = (vy - last_frame_vy_) / Time.deltaTime;
        JumperAxcel.y = ay_;
    }

    /// <summary>
    /// プレイヤーのアニメーションや速度など挙動に関する処理を行う
    /// </summary>
    void Move()
    {
        //速度の更新
        VelocityXUpdate();
        VelocityYUpdate();
        _rigidbody2D.velocity = new Vector2(vx, vy);
        _anim.SetFloat("x", _rigidbody2D.velocity.x);
        _anim.SetFloat("y", _rigidbody2D.velocity.y);

        
        bool groundRay_ = _rigidbody2D.IsTouching(_filter2D);
        //プレイヤーの状態から_animatorを制御
        if (groundRay_)//フィルターに地面が触れれば地面についてることにする
        {
            _anim.SetBool("Ground", true);
            _onGround = true;
        }
        else
        {
            if (y <= 0)
            {
                _anim.SetBool("Ground", false);
            }
            _onGround = false;
        }

        if(_onGround&&Dir!=0)
        {
            Running = true;
        }
        else
        {
            Running = false;
        }


        if (_rigidbody2D.velocity.y < 0)//y軸の速度が0以下なら落ちてることにする
        {
            _anim.SetBool("isFalling", true);
            _rigidbody2D.gravityScale = _gravityFalling;
        }
        else if (_chasing)//体当たり時の重力を0にする
        {
            _rigidbody2D.gravityScale = 0;
        }
        else//上昇中の場合
        {
            if (_propelling)//無重力ジャンプの設定なら、上昇できる制限時間まで重力を0にする
            {
                _rigidbody2D.gravityScale = 0;
            }
            else//通常のジャンプ上昇用の重力に切り替え
            {
                _rigidbody2D.gravityScale = _gravityrising;
            }
            _anim.SetBool("isFalling", false);
        }

        _anim.SetBool("allowAerialJump", _allowAerialJump);

        if (_stageCheck.OrTriggerStay == true && !_onGround && transform.localScale.x == _playerDir && _allowWallSlide)//壁に向かって横軸に入力していれば壁滑り状態にする
        {
            _onWall = true;
        }
        else
        {
            _onWall = false;
            _anim.SetBool("wallSlide", false);
        }

        if (_stageCheck.OrTriggerStay == false)
        {
            _onWall = false;
            _anim.SetBool("wallSlide", false);
        }

        //壁に接してるとき壁滑り状態にする
        if (_onWall)
        {
            _anim.SetBool("wallSlide", true);
        }

        ColliderCheck();

    }

    /// <summary>
    /// 敵の攻撃やこちらの体当たりが当たった時の処理
    /// //現状敵の攻撃が未実装
    /// </summary>
    void ColliderCheck()
    {
        if (Invicible && _avoidColliderScript.lockon_target != null && _justAvoidance)//ジャスト回避成功時WitchTime状態にする
        {
            //体当たりのターゲットと自身の位置から進行方向を計算
            Vector2 playerPos = transform.position;
            TargetEnemyObject = _avoidColliderScript.lockon_target;
            Vector2 targetPos = TargetEnemyObject.transform.position;
            Vector2 _playerDir = targetPos - playerPos;
            _distanceToEnemy = Vector2.Distance(targetPos, playerPos);

            if (_distanceToEnemy < _chaseTackleLengthMax) _timeScaleManager._witchTime = true;
        }

        if (_chaseTackleHitbox.TargetObj == TargetEnemyObject && _chasing)//体当たりした相手がターゲットだった時、体当たり成功にする
        {
            _anim.SetBool("chaseTackleHit", true);
        }
    }

    /// <summary>
    /// ジャンプ中にジャンプボタンを離したとき上昇を止めてy軸の速度を減速させる処理
    /// </summary>
    void JumpCanceled()
    {
        if (_jumping)
        {
            if (vy > 0) _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y * _verticalSpeedSustainLevel);
        }
    }
    /// <summary>
    /// プレイヤーの入力の読み取り、入力に応じたアニメーションや動きをさせる
    /// </summary>
    void Control()//プレイヤーの入力を管理する関数で入力したフラグに応じてMoveで数値を処理する
    {
        x = Input.GetAxisRaw("Horizontal");
        JumperLocalScale = transform.localScale.x;
        if (_canControl)//プレイヤーの入力に応じてキャラが動く状態の場合
        {
            _playerDir = (int)x;//キーの方向
            Dir = _playerDir;


            //ジャンプボタンを押せばジャンプのアクションをスタートさせる
            if (Input.GetKeyDown(ControllerManager.Instance.JumpButton))
            {
                JumpStart();
                _propellingTime = 0;
            }
            else if (Input.GetKeyUp(ControllerManager.Instance.JumpButton))//ジャンプボタンを離したときジャンプキャンセルで上昇を止める
            {
                JumpCanceled();
                _propelling = false;
            }
            else if (_jumping && !Input.GetKey(ControllerManager.Instance.JumpButton))
            {
                JumpCanceled();
            }

            ////回避ボタンで回避アクションのアニメーションをスタートさせる
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    _anim.SetTrigger("_avoidance");
            //}

            //攻撃ボタンで攻撃アクションをスタートさせる
            if (Input.GetKeyDown(ControllerManager.Instance.AttackButton))
            {
                if (_onGround)
                {
                    _anim.SetTrigger("attack");
                }
                else if (!_onGround && _airAttack > 0)
                {
                    _anim.SetTrigger("attack");
                    _airAttack -= 1;
                }
            }

            //銃撃ボタンで銃を打つアクションをスタートさせはなしたとき銃撃を止める
            if (Input.GetKeyDown(ControllerManager.Instance.ShotButton))
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("run")&&_playerDir!=0)
                {
                    //通常の走行中に銃撃を始めた時、走行しながらの銃撃アニメーションの開始位置を調節
                    _anim.Play("run_gun",0,_anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                }
                _anim.SetBool("gun", true);

                InstanteateBullet();
            }
            else if (Input.GetKeyUp(ControllerManager.Instance.ShotButton))
            {
                if (_anim.GetCurrentAnimatorStateInfo(0).IsName("run_gun") && _playerDir != 0)
                {
                    //走行しながらの銃撃アニメーションを走行中に終えた時、通常の走行アニメーションの開始位置を調節
                    //_anim.Play("run", 0, _anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
                }
                _anim.SetBool("gun",false);
            }
        }
        else//特殊な状況のみプレイヤーの入力を受け付ける場合(例:攻撃の後隙(!_canControl)の間に入力を受け付けてコンボ攻撃に繋げるとき) 
        {
            _playerDir = 0;
            if (Input.GetKeyDown(ControllerManager.Instance.AttackButton)&& WitchTime)//ジャスト回避成功後にたいあたりボタンで体当たりアクションを開始
            {
                _anim.SetTrigger("chase");
            }

            if (Input.GetKeyDown(ControllerManager.Instance.AttackButton) &&_attackBufferTime)//攻撃アクションのコンボ時の先行入力受付時
            {
                if (_onGround)
                {
                    _anim.SetTrigger("attack");
                }
                else if(!_onGround&&_airAttack>0){
                    _anim.SetTrigger("attack");
                    _airAttack -= 1;
                }
            }
        }

    }
    void CollisionTorelance()//頭上にブロックに頭をぶつけた時ある程度は自動でよける補助
    {
        float offset_ = _collisionToleranceOffset;
        float ray_length_ = _collisionTolerance;
        float _collisionTolerance_move_distance_ = 0;
        float height_ = _collisionToleranceHeight;
        Vector2 pos_ = transform.position;
        Ray2D rayRight = new Ray2D(pos_ + Vector2.up * height_ + offset_ * Vector2.right, Vector2.left);//右のRay
        Ray2D rayLeft = new Ray2D(pos_ + Vector2.up * height_ + offset_ * Vector2.left, Vector2.right);//左のRay
        Ray2D rayRightY = new Ray2D(pos_ + Vector2.up * (height_ - 0.5f) + offset_ * Vector2.right, Vector2.up);
        Ray2D rayLeftY = new Ray2D(pos_ + Vector2.up * (height_ - 0.5f) + offset_ * Vector2.left, Vector2.up);

        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offset_ * 2);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, offset_ * 2);
        RaycastHit2D hitCastLeftY = Physics2D.Raycast(rayLeftY.origin, rayLeftY.direction, 1);
        RaycastHit2D hitCastRightY = Physics2D.Raycast(rayRightY.origin, rayRightY.direction, 1);
        //RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, offset_ * 2);
        Debug.DrawRay(rayRight.origin + Vector2.up * 0.01f, (ray_length_) * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, (ray_length_) * rayLeft.direction, Color.blue);
        Debug.DrawRay(rayRightY.origin, rayRightY.direction * 1, Color.red);
        Debug.DrawRay(rayLeftY.origin, rayLeftY.direction * 1, Color.blue);

        bool corner_correction_ = false;

        if (hitCastRight.distance < _collisionTolerance && hitCastLeft.distance > 0)//右方向のRay(左サイド)だけ始点が壁にめり込んでない
        {
            if (hitCastLeftY.distance < hitCastRightY.distance)
            {
                corner_correction_ = true;
                _collisionTolerance_move_distance_ = -(offset_ * 2 - hitCastLeft.distance) - 0.05f;
            }
        }
        else if (hitCastLeft.distance < _collisionTolerance && hitCastRight.distance > 0)
        {
            if (hitCastRightY.distance < hitCastLeftY.distance)
            {
                corner_correction_ = true;
                _collisionTolerance_move_distance_ = offset_ * 2 - hitCastRight.distance + 0.05f;
            }
        }
        if (corner_correction_ == true && _jumping)// && !_onGround)
        {
            if (Mathf.Abs(_collisionTolerance_move_distance_) < ray_length_)
            {
                transform.position = new Vector2(transform.position.x + _collisionTolerance_move_distance_, transform.position.y);
            }
        }
    }
    /// <summary>
    /// 現在がジャンプ可能な状況か調べ状況に応じたジャンプの動きをさせる
    /// </summary>
    void JumpStart()
    {
        float _jumpAnticipationFrames_ = _jumpAnticipationFrames;
        if (_jumpAnticipationFrames_ == 0)
        {
            _anim.SetFloat("jumpAnticipationFrames", 100);
        }
        else
        {
            _anim.SetFloat("jumpAnticipationFrames", 1 / _jumpAnticipationFrames_);
        }
        if (!_jumping)
        {
            if (_onGround && !_onWall)
            {
                _anim.SetTrigger("Jump");
            }
            else if (!_onGround && _onWall)
            {
                _anim.SetTrigger("Jump");
            }
            else if (!_onGround && !_onWall)
            {

                _anim.SetTrigger("Jump");
                _anim.SetInteger("airJump", _airJump);
            }
        }
        else if (_airJump > 0)
        {
            _anim.SetTrigger("Jump");
            _anim.SetInteger("airJump", _airJump);
        }
    }
    /// <summary>
    /// プレイヤーの状態を地面に足がついている状態に更新させる
    /// 地面に足を付けてるアニメーションの時に_animatorから読まれる
    /// </summary>
    void GroundCheck()
    {
        _jumping = false;
        _wallJumping = false;
        Jumping = _jumping;
        _propelling = false;
        _airJump = _airJumpNum;
        _anim.SetInteger("airJump",_airJump);
        _airAttack = _airAttackNum; 
    }

    /// <summary>
    ///  ジャンプのアニメーションが始まった瞬間のアニメーションの遷移と上昇速度の計算をする(_animator経由で動かす) 
    /// </summary>
    void Jump()
    {
        if (!_onGround && !_onWall)
        {
            _airJump -= 1;
            if (Mathf.Sign(vx) != _playerDir)//空中ジャンプの瞬間に進行方向と逆方向へ飛ぼうとしたとき進行方向への慣性を消す
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            }
        }
        //else if (_onGround && !_onWall)
        //{
        //    _airJump = _airJumpNum;
        //}

        //プレイヤーの状態をジャンプしていることにする
        _anim.SetInteger("airJump", _airJump);
        _wallJumping = false;
        _jumping = true;
        Jumping = _jumping;

        //ジャンプ始めに天井がないか調べる
        CollisionTorelance();

        //無重力ジャンプをする設定なら無重力ジャンプをしていることにする
        if (_maxPropellingFrames > 0)
        {
            _propelling = true;
        }

        //ジャンプの初速度の力を与える
        _rigidbody2D.AddForce(new Vector2(0, _jumpPower + Mathf.Abs(vx) * _jumpVelocityBonus));
        
        //ジャンプの瞬間、横軸の速度を加減速させる設定の時の計算
        vx = _vxAdjustmentAtTakeoff * vx;
    }

    /// <summary>
    /// 壁ジャンプできる状態か調べ、可能なら壁ジャンプのアニメーションと力を与える
    /// </summary>
    void WallJump()
    {
        if (_allowWallJump)//壁ジャンプできる状態なら壁から設定した値だけ斜め上にジャンプさせる
        {
            _rigidbody2D.velocity = new Vector2(_maxVx * _wallJumpSpeedRatio * -transform.localScale.x, 0);
            _rigidbody2D.AddForce(new Vector2(0, _jumpPower + Mathf.Abs(vx) * _jumpVelocityBonus));
            transform.localScale = new Vector2(-transform.localScale.x, 1);

            //壁ジャンプしていることにする
            _jumping = true;
            _wallJumping = true;
            Jumping = _jumping;
        }
    }
    /// <summary>
    /// プレイヤーを壁滑り状態にする
    /// _animator経由で動かす
    /// </summary>
    void WallSlide()
    {
        _jumping = false;
        _wallJumping = false;
        Jumping = _jumping;
        _propelling = false;
    }

    /// <summary>
    /// 回避アニメーションが始まった瞬間に読まれ、プレイヤーを回避していることにする
    /// _animator経由で動かす
    /// </summary>
    void Avoidance()
    {
        //状態を回避していることにする
        //現状、ジャスト回避時間も回避開始と同時に受け付け始める
        _canControl = false;
        Invicible = true;
        _justAvoidance = true;
        _spriteColor.color = new Color(255,0,0);
        _avoidance = true;

        //回避アクションの初速度を与える
        if (_playerDir != 0)
        {
            _rigidbody2D.velocity = new Vector2(_avoidSpeed * _playerDir, 0);
        }
    }

    /// <summary>
    /// ジャスト回避の受付時間を終わらせる
    /// _animator経由で動かす
    /// </summary>
    void justAvoidEnd()
    {
        _spriteColor.color = new Color(0, 255, 0);
        _justAvoidance = false;
    }

    /// <summary>
    /// 回避アクションを終了したときの状態にする
    /// _animator経由で動かす
    /// </summary>
    void AvoidanceEnd()
    {
        _avoidance = false;
        _rigidbody2D.velocity = Vector2.zero;
        _canControl = true;
        Invicible = false;
        _timeScaleManager._witchTime = false;
        _rigidbody2D.velocity = Vector2.zero;
        _spriteColor.color = new Color(255, 255, 255);
        _anim.SetFloat("_chasingTime",_chasingTime);
    }

    /// <summary>
    /// 回避の後隙時に読まれ、ジャスト回避が成功していたなら体当たりのターゲットを外す
    /// _animator経由で動かす
    /// </summary>
    void AvoidanceGap()
    {
        _avoidColliderScript.lockon_target = null;
    }

    /// <summary>
    /// 相手に体当たりするアニメーションを始める時に呼ばれる
    /// _animator経由で動かす
    /// </summary>
    void chase()
    {
        //体当たりアクションの状態にする
        _chasing = true;
        _canControl = false;
        //進行方向へ初速度を与える
        Vector2 playerPos = transform.position;
        TargetEnemyObject = _avoidColliderScript.lockon_target;
        Vector2 targetPos = TargetEnemyObject.transform.position;
        Vector2 _playerDir = targetPos - playerPos;
        _rigidbody2D.velocity = _playerDir.normalized *_chaseSpeed;
        
        _spriteColor.color = new Color(0,0,255);
    }

    /// <summary>
    /// 体当たりアクションが終わった時体当たり状態でないことにする
    /// _animator経由で動かす
    /// </summary>
    void chaseEnd()
    {
        _chasing = false;
        _canControl = true;
        _rigidbody2D.velocity = Vector2.zero;
        _spriteColor.color = new Color(255, 255, 255);

        //体当たりを終え対象をターゲットから外す
        _avoidColliderScript.lockon_target = null;
    }

    /// <summary>
    /// 体当たりで敵に衝突した瞬間のアニメーション始まった時に読む
    /// _animator経由で動かす
    /// </summary>
    void ChaseTackleHit()
    {
        //少し上に跳ねさせる
        _rigidbody2D.velocity = new Vector2(0,0);
        _rigidbody2D.AddForce(_chaseTackleHitStep);
        //衝突時にヒットストップさせる
        TimeScaleManager.Instance.HitStop(10);
    }
    /// <summary>
    /// 体当たり開始時に読む
    /// _animator経由で動かす
    /// </summary>
    void ChaseTackleStart()
    {
        //体当たりの当たり判定をoffにする
        _chaseTackleHitbox.ActiveCollider();
    }

    /// <summary>
    /// 体当たり終了時に読む
    /// _animator経由で動かす
    /// </summary>
    void ChaseTackleEnd()
    {
       　//体当たりの当たり判定をoffにする
        _chaseTackleHitbox.DisActiveCollider();
    }


    //攻撃系アクションのアニメーションに関連する処理===============================================================-
    private int _colliderShapeNum;//

    /// <summary>
    /// 攻撃アニメーションが始まった時に読む
    /// _animator経由で読む
    /// </summary>
    void AtackStart()
    {
        _attackVelocityVx = AttackHitBox[AttackAnimNum].Vx;
        _attackVelocityAx = AttackHitBox[AttackAnimNum].Ax;
        
        //攻撃アクションを始めた瞬間の状態にする
        _attackBufferTime = false;
        _canControl = false;
        _attacking = true;
        _colliderShapeNum = 0;

        //攻撃アクション時に少し前進させる
        if (_onGround)
        {
            _rigidbody2D.velocity = new Vector2(_attackVelocityVx * transform.localScale.x, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_attackVelocityVx * transform.localScale.x, _rigidbody2D.velocity.y);
        }
    }
    /// <summary>
    /// 読み込まれた瞬間に現在アニメーションしている攻撃に紐づけた当たり判定を出す
    /// _animator経由で読む
    /// </summary>
    void ActiveAttackCollider()
    {
        AttackHitBox[AttackAnimNum].ActiveCollider();
    }

    /// <summary>
    /// 読み込まれた瞬間に現在アニメーションしている攻撃に紐づけた当たり判定を消す
    /// _animator経由で読む
    /// </summary>
    void DisActiveAttackCollider()
    {
        AttackHitBox[AttackAnimNum].DisActiveCollider();
    }

    /// <summary>
    /// 読み込まれた瞬間に攻撃の先行入力を受け付け始める
    /// </summary>
    void ActiveBufferingTime()
    {
        _attackBufferTime = true;
    }

    /// <summary>
    /// 読み込まれた瞬間に攻撃の先行入力の受け付けを終える
    /// </summary>
    void DisActiveBufferTime()
    {
        _attackBufferTime = false;
    }


    /// <summary>
    /// 読み込まれた瞬間にHitBoxScriptで設定した当たり判定の形へ順に切り替える
    /// </summary>
    void ChangeAttackCollider()
    {
        AttackHitBox[AttackAnimNum].ChangeColliderSize(_colliderShapeNum);
        _colliderShapeNum += 1;
    }

    /// <summary>
    /// 攻撃アニメーションを終える瞬間にプレイヤーを攻撃をしていない状態にする
    /// </summary>
    void AttackEnd()
    {
        _attacking = false;
        _canControl = true;
    }

    [SerializeField] BulletControllerScript[]bulletObject = new BulletControllerScript[15];
    [SerializeField] GameObject GunPosition;
    private int bulletNum = 0;
    void InstanteateBullet()
    {
        bulletObject[bulletNum].gameObject.SetActive(true);
        var gunDirection = _onWall == true ? -1 : 1;
        Vector2 gunPosition;
        if(Jumping)
        {
            gunPosition = _jumpingGunPos;
        }else if (_onWall)
        {
            gunPosition = _wallSlideGunPos;
        }else if (Running)
        {
            gunPosition = _runningGunPos;
        }
        else
        {
            gunPosition = _normalGunPos;
        }

        bulletObject[bulletNum].SetBulletDirection(new Vector2(transform.localScale.x*gunDirection,transform.localScale.y));
        GunPosition.transform.localPosition = gunPosition;
        bulletObject[bulletNum].gameObject.transform.position = GunPosition.transform.position;
        bulletNum++;
        if (bulletNum >= bulletObject.Length)
        {
            bulletNum = 0;
        }
    }
    /// <summary>
    /// アニメーションに関わるbool変数を全部リセットする(走らせた後にIdleもしくはisFallingの状態になる)
    /// </summary>
    void StateReset()
    {
        _jumping = false;
        _wallJumping = false;
        _propelling = false;
        Invicible = false;
        _timeScaleManager._witchTime = false;
        _justAvoidance = false;
        _attacking = false;
        _attackBufferTime = false;
        _anim.SetBool("wallSlide",false);
        _anim.SetBool("running", false);
        _anim.SetBool("gun", false);
    }
    [SerializeField] private int _hp;
    private bool _damaged;
    public void Damaged(string attack_name_, Vector2 nock_back_power_, int ATKValue)
    {
        if (!Invicible)
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
            NockBack(nock_back_power_);
            _hp -= ATKValue;
            _anim.SetTrigger("damaged");
            _damaged = true;
        }
    }

    private void NockBack(Vector2 nock_back_power_)
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.AddForce(nock_back_power_);
    }
}
