using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyScriptMaster : MonoBehaviour
{
    [SerializeField] bool FlyingType;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _target;
    [SerializeField] float span;
    [SerializeField] GameObject _explosionObject;
    float wait;
    [SerializeField] int _hp;

    string player_attack_name;
    private Rigidbody2D _rigidbody2d;
    private Animator _anim;

    [SerializeField] float _axNormal;
    [SerializeField] float _axDamaged;
    bool _damaged;
    [SerializeField] private float _maxWalkSpeed;
    [SerializeField] private float _maxVerticalSpeed;


    void Start()
    {
        if (span <= 0) span = 1;
        wait = Random.Range(0, span);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        Observable.Interval(System.TimeSpan.FromMilliseconds(_turnTime)).Subscribe(_ =>
        {
            _directionY *= -1;
        }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        VelocityXUpdate();
        if (FlyingType)
        {
            VelocityYUpdate();
        }
    }

    public void shotBullet()
    {
        wait -= Time.deltaTime;//�e�̏o��Ԋu���ҋ@
        if (wait < 0)
        {
            wait = span;
            GameObject b = Instantiate(_bulletPrefab);//�e�𐶐�
            b.transform.position = this.transform.position;
            Vector3 dir = (_target.transform.position - this.transform.position);
            // �����Ō������������ɉ�]�����Ă܂�
            b.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            TestBullet _testBullet = b.GetComponent<TestBullet>();
            _testBullet.SetParentObject(this.gameObject);
        }
    }

    /// <summary>
    /// �U���𓖂Ă�ꂽ�u�ԂɃv���C���[������Ăяo���Ahp��m�b�N�o�b�N�̒l���v�Z����
    /// attack_name_�ɂ͍U���p�̃R���C�_�[���A�^�b�`���ꂽ�I�u�W�F�N�g�̖��O������
    /// </summary>
    /// <param name="attack_name_"></param>
    /// <param name="nock_back_power_"></param>
    /// <param name="ATKValue"></param>
    public void Damaged(string attack_name_,Vector2 nock_back_power_,int ATKValue)
    {
        if (attack_name_ != player_attack_name)
        {
            _rigidbody2d.velocity = new Vector2(0,_rigidbody2d.velocity.y);
            player_attack_name = attack_name_;
            NockBack(nock_back_power_);
            _hp -= ATKValue;
            _anim.SetTrigger("damaged");
            _damaged = true;
        }
    }

    private void NockBack(Vector2 nock_back_power_)
    {
        _rigidbody2d.velocity = Vector2.zero;
        _rigidbody2d.AddForce(nock_back_power_);
    }

    /// <summary>
    /// �_���[�W���󂯂��u�Ԃ�animator����Ăяo���֐�
    /// ����p�r�i�V
    /// </summary>
    void damageAnimStart()
    {

    }

    /// <summary>
    /// �_���[�W���󂯂��u�Ԃ�animator����Ăяo���֐�
    /// </summary>
    void damageAnimEnd()
    {
        _damaged = false;
        player_attack_name = "null";
    }

    /// <summary>
    /// HP�̒l�𒲂ׂāA0�ȉ��Ȃ炱�̓G�����ł�����
    /// </summary>
    public void CheckHP()
    {
        if (_hp <= 0)
        {
            DestroyEnemy();
        }
    }
    private void DestroyEnemy()
    {
        Instantiate(_explosionObject,transform.position,new Quaternion(0,0,0,0));
        Destroy(gameObject);
    }

    /// <summary>
    /// �����̑��x���v�Z
    /// </summary>
    void VelocityXUpdate()
    {
        float vx_ = _rigidbody2d.velocity.x;
        float ax_;
        if (_damaged)
        {
            ax_ = _axDamaged;
            vx_ -= ax_ * Mathf.Sign(vx_);
            if (Mathf.Abs(vx_) <= ax_&&_damaged)//���x���\���ɏ�������Β�~
            {
                vx_ = 0;
            }
        }
        else
        {
            vx_ = _maxWalkSpeed * transform.localScale.x;
            Turn();
        }
        _rigidbody2d.velocity = new Vector2(vx_, _rigidbody2d.velocity.y);
    }

    private int _directionY=1;
    [SerializeField] private int _turnTime;
    void VelocityYUpdate()
    {
        float vy_ = _rigidbody2d.velocity.y;
        float ay_;
        vy_ = _maxVerticalSpeed*_directionY;
        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x,vy_);
    }

    [SerializeField] bool _edgeTurn;
    [SerializeField] bool _wallTurn;

    [SerializeField] float _wallRayOffsetX;
    [SerializeField] float _wallRayOffsetY;
    [SerializeField] float _wallRayLength;
    [SerializeField] float _edgeRayOffsetX;
    [SerializeField] float _edgeRayOffsetY;
    [SerializeField] float _edgeRayLength;
    /// <summary>
    /// �ǁA�������͊R���ڂ̑O�ɂ���Ƃ��ɐi�s������ς��鏈�����s��
    /// </summary>
    private void Turn()
    {
        Ray2D rayFront = new Ray2D((Vector2)transform.position + Vector2.up * _wallRayOffsetY + Vector2.right * _wallRayOffsetX * transform.localScale.x, Vector2.right * transform.localScale.x);
        Ray2D rayRight = new Ray2D((Vector2)transform.position + _edgeRayOffsetX * Vector2.right + _edgeRayOffsetY * Vector2.up, Vector2.up * -1);
        Ray2D rayLeft = new Ray2D((Vector2)transform.position + _edgeRayOffsetX * Vector2.left + _edgeRayOffsetY * Vector2.up, Vector2.up * -1);

        int layerMask_ = 1 << 8;
        RaycastHit2D hitCastFront = Physics2D.Raycast(rayFront.origin, rayFront.direction, _wallRayLength, layerMask_);
        RaycastHit2D hitCastRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, _edgeRayLength, layerMask_);
        RaycastHit2D hitCastLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, _edgeRayLength, layerMask_);

        Debug.DrawRay(rayFront.origin, _wallRayLength * rayFront.direction, Color.yellow);
        Debug.DrawRay(rayRight.origin, _edgeRayLength * rayRight.direction, Color.red);
        Debug.DrawRay(rayLeft.origin, _edgeRayLength * rayLeft.direction, Color.blue);

        if (_edgeTurn)
        {
            if (hitCastLeft.distance == 0 && hitCastRight.distance != 0 && hitCastRight.collider.tag == "ground")
            {
                transform.localScale = new Vector2(1, transform.localScale.y);
            }
            else if (hitCastLeft.distance != 0 && hitCastRight.distance == 0 && hitCastLeft.collider.tag == "ground")
            {
                transform.localScale = new Vector2(-1, transform.localScale.y);
            }
        }

        if (_wallTurn)
        {
            if (hitCastFront.distance != 0 && hitCastFront.collider.tag == "ground")
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }
    }
}
