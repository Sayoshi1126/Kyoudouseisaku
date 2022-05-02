using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���n�����蔻��Ɋւ��鏈��������
/// </summary>
public class HitBoxScript : MonoBehaviour
{
    public GameObject TargetObj;
    public string ColliderName;
    public string TargetTag;

    [SerializeField] private BoxCollider2D[]HitBox;

    [SerializeField] public float Vx;
    [SerializeField] public float FroationgPower;
    [SerializeField] public float Ax;
    [SerializeField] private float _attackForceX;
    [SerializeField] private float _attackForceY;
    [SerializeField] public int ATKValue;
    [SerializeField] private int _hitStopFrame;
    [SerializeField] private GameObject _player;


    //�����蔻��̌`�����ɐݒ肷��
    //��@ColliderSize[0], ColliderSize[1]�͂��ꂼ��I�u�W�F�N�g�ɂ���1�ڂ�2�ڂ�boxcollider2d�̏�������
    //��@ColliderSize[n].Offset[0], ColliderSize[].Size[1]�͂��ꂼ��ColliderSize[n]��1��ڂɓǂ܂��offset��2��ڂ̓����蔻��̑傫���Ɋւ����������
    [System.Serializable]
    struct ColliderSize
    {
        public Vector2[] Offset;
        public Vector2[] Size;
    }
    [SerializeField] ColliderSize[] colliderSize;

    /// <summary>
    /// ���̃X�N���v�g���A�^�b�`����Ă�gameobject�ɂ���BoxCollider2d�����ׂĎ擾
    /// </summary>
    public void GetBoxColliderComponent(){
        HitBox = GetComponents<BoxCollider2D>();
        ColliderName = gameObject.name;
    }

    /// <summary>
    /// �����蔻���on�ɂ���
    /// </summary>
    public void ActiveCollider()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// �����蔻���off�ɂ���
    /// </summary>
    public void DisActiveCollider()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// �����蔻��̌`���Ԃɕς���
    /// </summary>
    public void ChangeColliderSize(int n)//n�ɂ͍U���͈͕ύX������ڂ��̏�񂪓���
    {
        for (int i = 0; i < HitBox.Length; i++)
        {

            SetHitBoxSize(colliderSize[i].Size[n], colliderSize[i].Offset[n], i);
        }
    }

    /// <summary>
    ///�@�R���C�_�[�̌`�����ۂɕύX����
    /// </summary>
    /// <param name="Size"></param>
    /// <param name="Offset"></param>
    /// <param name="colliderNum"></param>
    void SetHitBoxSize(Vector2 Size,Vector2 Offset,int colliderNum)
    {

        Vector2 colliderSize = Size;
        Vector2 offset = Offset;

        HitBox[colliderNum].size = colliderSize;
        HitBox[colliderNum].offset = offset;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        TargetObj = collision.gameObject;
        if (collision.tag == "enemy"&&TargetTag == "enemy")
        {
            EnemyScriptMaster enemy = TargetObj.GetComponent<EnemyScriptMaster>();
            enemy.Damaged(gameObject.name,new Vector2(_attackForceX*_player.transform.localScale.x,_attackForceY),ATKValue);
            TimeScaleManager.Instance.HitStop(_hitStopFrame);
        }else if(collision.tag == "Player"&&TargetTag == "Player")
        {
            Jumper player = TargetObj.GetComponent<Jumper>();
            player.Damaged(gameObject.name, new Vector2(_attackForceX * _player.transform.localScale.x, _attackForceY), ATKValue );
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        TargetObj = null;
    }
}
