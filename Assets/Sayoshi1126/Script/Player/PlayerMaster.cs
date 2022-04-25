using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Jumper関連の速度や加速度を他のスクリプトから読めるようにしたり、Jumperの速度や加速度を保存する
/// </summary>
public class PlayerMaster : MonoBehaviour
{
    public float JumperLocalScale;
    public float Dir;
    public bool Running;
    [HideInInspector] public Vector2 JumperPos;
    [HideInInspector] public Vector2 JumperVel;
    [HideInInspector] public Vector2 JumperAxcel;

    [HideInInspector] public Vector2 CameraPos;
    [HideInInspector] public Vector2 CameraVel;

    [HideInInspector] public Rigidbody2D PlayerRigidbody2D;
    [HideInInspector] public Jumper jumper;
    [HideInInspector] public GameObject player_gameobject;

    public bool Jumping;
}