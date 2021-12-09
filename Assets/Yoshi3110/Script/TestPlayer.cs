using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    [SerializeField] GameObject lockonPointer;
    GameObject lockonTarget;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.position += new Vector3(x * Time.deltaTime, y * Time.deltaTime, 0) * speed;
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

    void OnTriggerEnter2D(Collider2D col)
    {
        //かすめた時の処理の例
        if (col.gameObject.tag == "Bullet")
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

    void OnCollisionEnter2D(Collision2D col)
    {
        //弾にぶつかった時の処理
        if (col.gameObject.tag == "Bullet")
        {
            Debug.Log("Damage!");
            Destroy(col.gameObject);
        }
    }
}
