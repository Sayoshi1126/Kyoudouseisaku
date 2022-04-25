using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

public class MyEMTransition : MonoBehaviour
{
    public Fade fade;
    public UnityEvent onTransitionComplete = new UnityEvent();

    //トランジション完了イベントを発行する核となるSubjectのインスタンス
    private Subject<Unit> _transitionCompletedSubject = new Subject<Unit>();

    private void Start()
    {
        fade.GetComponent<Fade>();
    }
    public IObservable<Unit> OnTransitionComplete
    {
        get { return _transitionCompletedSubject; }
    }

    public void FadeIn()
    {
        fade.FadeIn(0.5f);
        onTransitionComplete.Invoke();
        Debug.Log("fadein");
    }

    public void FadeOut()
    {
        Debug.Log("fadeout");
        fade.FadeOut(0.5f);
        onTransitionComplete.Invoke();
    }
}
