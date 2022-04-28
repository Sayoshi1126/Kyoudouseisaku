using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Utilities.SceneTransition;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] TimeScaleManager TimeScalaManagerObj;
    [SerializeField] ControllerManager ControllerManagerObj;
    [SerializeField] TransitionManager TransitionManagerObj;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        if (this != Instance) return;

        Instantiate(TimeScalaManagerObj);
        Instantiate(TransitionManagerObj);
        Instantiate(ControllerManagerObj);
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
