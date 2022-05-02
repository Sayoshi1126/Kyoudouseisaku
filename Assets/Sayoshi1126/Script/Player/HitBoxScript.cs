using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃系当たり判定に関する処理を扱う
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


    //当たり判定の形を順に設定する
    //例　ColliderSize[0], ColliderSize[1]はそれぞれオブジェクトにある1つ目と2つ目のboxcollider2dの情報をもつ
    //例　ColliderSize[n].Offset[0], ColliderSize[].Size[1]はそれぞれColliderSize[n]の1回目に読まれるoffsetと2回目の当たり判定の大きさに関する情報をもつ
    [System.Serializable]
    struct ColliderSize
    {
        public Vector2[] Offset;
        public Vector2[] Size;
    }
    [SerializeField] ColliderSize[] colliderSize;

    /// <summary>
    /// このスクリプトがアタッチされてるgameobjectにあるBoxCollider2dをすべて取得
    /// </summary>
    public void GetBoxColliderComponent(){
        HitBox = GetComponents<BoxCollider2D>();
        ColliderName = gameObject.name;
    }

    /// <summary>
    /// 当たり判定をonにする
    /// </summary>
    public void ActiveCollider()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 当たり判定をoffにする
    /// </summary>
    public void DisActiveCollider()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 当たり判定の形順番に変える
    /// </summary>
    public void ChangeColliderSize(int n)//nには攻撃範囲変更が何回目かの情報が入る
    {
        for (int i = 0; i < HitBox.Length; i++)
        {

            SetHitBoxSize(colliderSize[i].Size[n], colliderSize[i].Offset[n], i);
        }
    }

    /// <summary>
    ///　コライダーの形を実際に変更する
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
