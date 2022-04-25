using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの残像を作る
/// </summary>
public class EchoEffect : MonoBehaviour
{
    // Start is called before the first frame update

    float _elapsedTime;
    [SerializeField] float _echoGenerateSpan;

    public GameObject EchoObject;
    private Jumper _player;
    private SpriteRenderer _playerSprite;

    private SpriteRenderer _echoSprite;
    void Start()
    {
        _player = GetComponent<Jumper>();
        _playerSprite = GetComponent<SpriteRenderer>();

        _echoSprite = EchoObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if(new Vector2(_player.JumperVel.x,_player.JumperVel.y) != Vector2.zero)//プレイヤーが動いてる時だけ生成
        {
            if (_elapsedTime <= 0)//ある程度時間が過ぎれば残像用のオブジェクトを生成
            {
                //spawn echo game object
                _echoSprite.sprite = _playerSprite.sprite;
                _echoSprite.color = new Color(255,0,0);
                GameObject instance = (GameObject)Instantiate(EchoObject, transform.position, Quaternion.identity);
                EchoObject.transform.localScale = new Vector2(_player.transform.localScale.x,1);
                Destroy(instance.gameObject, 1f);
                _elapsedTime = _echoGenerateSpan;

            }
            else
            {
                _elapsedTime -= Time.deltaTime;
            }
        }
    }
}
