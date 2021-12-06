using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class echoEffect : MonoBehaviour
{
    // Start is called before the first frame update

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject echo;
    private Jumper player;
    private SpriteRenderer playerSprite;

    private SpriteRenderer echoSprite;
    void Start()
    {
        player = GetComponent<Jumper>();
        playerSprite = GetComponent<SpriteRenderer>();

        echoSprite = echo.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if(player.moveInput != Vector2.zero)
        {
            if (timeBtwSpawns <= 0)
            {
                //spawn echo game object
                echoSprite.sprite = playerSprite.sprite;
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                echo.transform.localScale = new Vector2(player.transform.localScale.x,1);
                Destroy(instance.gameObject, 1f);
                timeBtwSpawns = startTimeBtwSpawns;

            }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }
}
