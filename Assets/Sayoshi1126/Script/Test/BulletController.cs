using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// ñ¢äÆê¨
/// </summary>
public class BulletController : MonoBehaviour
{
    GameObject bullet_object;
    int bulletNum = 30;
    float shot_time = 5;
    private void Start()
    {
        for(int i = 0; i < bulletNum; i++)
        {
            Instantiate(bullet_object);
        }
    }

    void BulletUpdate()
    {
        
    }

    void InstanteBullet()
    {
        
    }
}
