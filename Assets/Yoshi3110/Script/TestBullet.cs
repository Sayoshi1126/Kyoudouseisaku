using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject parentObj;
    [SerializeField] float speed = 1;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = gameObject.transform.rotation * new Vector3(speed, 0, 0);
        gameObject.transform.position += velocity * Time.deltaTime;
    }

    public void SetParentObject(GameObject _g)//敵が弾を生成した時に、敵側からこの関数を呼び出す。
    {
        parentObj = _g;
    }
    public GameObject GetParentObject()//プレイヤーが弾をかすめた時に、プレイヤー側からこの関数を呼び出す。
    {
        return parentObj;
    }
}
