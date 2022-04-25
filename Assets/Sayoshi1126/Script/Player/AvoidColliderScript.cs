using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 回避アクションで攻撃をかすめた時の処理
/// </summary>
public class AvoidColliderScript : MonoBehaviour
{
    public GameObject lockon_target;
    [SerializeField] Jumper jumper;
    [SerializeField] GameObject lockonPointer;
    void OnTriggerEnter2D(Collider2D col)
    {
        //かすめた時の処理の例
        //回避の無敵フレームの間でかすめた条件を追加
        if (col.gameObject.tag == "Bullet"&&jumper.Invicible)
        {
            TestBullet _t;
            _t = col.gameObject.GetComponentInParent<TestBullet>();
            if (_t != null)
            {
                lockon_target = _t.GetParentObject();
                Debug.Log("LockOn!:" + lockon_target);
            }
        }
    }

    public void lockOnPointer()
    {
        if(jumper.gameObject.transform.localScale.x != 1)
        {
            this.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        if (lockon_target != null)
        {
            lockonPointer.SetActive(true);
            Vector3 dir = (lockon_target.transform.position - this.transform.position);
            lockonPointer.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        }
        else
        {
            lockonPointer.SetActive(false);
        }
    }
}
