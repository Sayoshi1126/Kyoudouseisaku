using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    // Start is called before the first frame update
    public KeyCode JumpButton { get; set; }
    public KeyCode AttackButton { get; set; }
    public KeyCode ShotButton { get; set; }

    protected override void Awake()
    {
        base.Awake();
        JumpButton = KeyCode.Space;
        AttackButton = KeyCode.F;
        ShotButton = KeyCode.V;
    }

}
