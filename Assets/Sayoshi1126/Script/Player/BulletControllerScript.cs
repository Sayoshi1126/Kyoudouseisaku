using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BulletControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float _bulletSpeedX;
    [SerializeField] private int _destroyFrames;
    private Rigidbody2D _rigidbody2d;
    private CircleCollider2D _bulletCollider2d;
    public int ATKValue;
    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Observable.TimerFrame(_destroyFrames,FrameCountType.FixedUpdate).Subscribe(_ => gameObject.SetActive(false));
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject targetObj = collision.gameObject;
        if(collision.tag == "enemy")
        {
            EnemyScriptMaster enemy = targetObj.GetComponent<EnemyScriptMaster>();
            enemy.Damaged(gameObject.name, new Vector2(0,0), ATKValue);
            gameObject.SetActive(false);
        }
        else if(collision.tag == "ground")
        {
            gameObject.SetActive(false);
        }
    }

    public void SetBulletDirection(Vector2 direction)
    {
        _rigidbody2d.velocity = new Vector2(direction.x * _bulletSpeedX, 0);
    }

}
