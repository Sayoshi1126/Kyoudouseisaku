using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Layerが「Enemy」になっているオブジェクトを探して一定時間追尾します。
//衝突処理はまだ入っていません。

public class TrackingBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float trackingTime = 1f;
    GameObject targetObj;
    Vector3 targetPos;
    void Start()
    {
        targetObj = SearchEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = gameObject.transform.rotation * new Vector3(speed * transform.localScale.x, 0, 0);
        gameObject.transform.position += velocity * Time.deltaTime;
        if (targetObj != null)
        {
            targetPos = targetObj.transform.position;
            if (trackingTime > 0)
            {
                trackingTime -= Time.deltaTime;
                Vector3 dir = (targetPos - this.transform.position);
                // ここで向きたい方向に回転させてます
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(Vector3.right, dir), Time.deltaTime * 10f * speed);
            }
        }

    }

    GameObject SearchEnemy()
    {
        float maxDistance = 12f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.rotation * Vector2.right, maxDistance, 1 << 9);

        if (hit.collider != null)
        {
            float distance = Vector2.Distance(hit.point, transform.position);
            Debug.DrawRay(transform.position, transform.rotation * Vector2.right * distance, Color.red, 0.5f, false);
            return hit.collider.gameObject;

        }
        else
        {
            Debug.DrawRay(transform.position, transform.rotation * Vector2.right * maxDistance, Color.green, 0.1f, false);

        }
        for (int i = 1; i < 8; i++)
        {
            Quaternion _q = Quaternion.Euler(0, 0, 5f * i) * transform.rotation;
            hit = Physics2D.Raycast(transform.position, _q * Vector2.right, maxDistance, 1 << 9);

            if (hit.collider != null)
            {
                float distance = Vector2.Distance(hit.point, transform.position);
                Debug.DrawRay(transform.position, _q * Vector2.right * distance, Color.red, 0.5f, false);
                return hit.collider.gameObject;

            }
            else
            {
                Debug.DrawRay(transform.position, _q * Vector2.right * maxDistance, Color.green, 0.1f, false);

            }

            _q = Quaternion.Euler(0, 0, -5f * i) * transform.rotation;
            hit = Physics2D.Raycast(transform.position, _q * Vector2.right, maxDistance, 1 << 9);

            if (hit.collider != null)
            {
                float distance = Vector2.Distance(hit.point, transform.position);
                Debug.DrawRay(transform.position, _q * Vector2.right * distance, Color.red, 0.5f, false);
                return hit.collider.gameObject;

            }
            else
            {
                Debug.DrawRay(transform.position, _q * Vector2.right * maxDistance, Color.green, 0.1f, false);

            }
        }
        return null;
    }
}
