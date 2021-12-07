using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject target;
    [SerializeField] float span;
    float wait;
    void Start()
    {
        if (span <= 0) span = 1;
        wait = Random.Range(0, span);
    }

    // Update is called once per frame
    void Update()
    {
        wait -= Time.deltaTime;//弾の出る間隔分待機
        if (wait < 0)
        {
            wait = span;
            GameObject b = Instantiate(bulletPrefab);//弾を生成
            b.transform.position = this.transform.position;
            Vector3 dir = (target.transform.position - this.transform.position);
            // ここで向きたい方向に回転させてます
            b.transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
            TestBullet _testBullet = b.GetComponent<TestBullet>();
            _testBullet.SetParentObject(this.gameObject);
        }
    }
}
