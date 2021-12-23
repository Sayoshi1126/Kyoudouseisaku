using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bg : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject camera;

    float lastJumperX;
    float lastJumperY;

    public float bgScrollRatio = 0.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bgScrollRatio = Settings.Instance.bgScrollRatio;
        float x = camera.transform.position.x - lastJumperX;
        float y = camera.transform.position.y - lastJumperY;

        transform.position = new Vector2(transform.position.x+x*bgScrollRatio,transform.position.y+y*bgScrollRatio);
        
        lastJumperX = camera.transform.position.x;
        lastJumperY = camera.transform.position.y;
    }
}
