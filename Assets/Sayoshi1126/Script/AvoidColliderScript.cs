using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidColliderScript : MonoBehaviour
{
    public GameObject lockonTarget;
    [SerializeField] Jumper jumper;
    [SerializeField] GameObject lockonPointer;
    void OnTriggerEnter2D(Collider2D col)
    {
        //‚©‚·‚ß‚½‚Ìˆ—‚Ì—á
        //‰ñ”ğ‚Ì–³“GƒtƒŒ[ƒ€‚ÌŠÔ‚Å‚©‚·‚ß‚½ğŒ‚ğ’Ç‰Á
        if (col.gameObject.tag == "Bullet"&&jumper.invicible)
        {
            TestBullet _t;
            _t = col.gameObject.GetComponentInParent<TestBullet>();
            if (_t != null)
            {
                lockonTarget = _t.GetParentObject();
                Debug.Log("LockOn!:" + lockonTarget);
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

        if (lockonTarget != null)
        {
            lockonPointer.SetActive(true);
            Vector3 dir = (lockonTarget.transform.position - this.transform.position);
            lockonPointer.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        }
        else
        {
            lockonPointer.SetActive(false);
        }
    }
}
