using System.Collections;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 100からカウントダウンし値を通知するサンプル
/// </summary>
public class TimeCounter : MonoBehaviour
{
    //イベントを発行する核となるインスタンス
    private Subject<int> timerSubject = new Subject<int>();

    //イベントの購読側だけを公開
    public IObservable<int> OnTimeChanged
    {
        get { return timerSubject; }
    }

    void Start()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        //100からカウントダウン
        var time = 100;
        while (time > 0)
        {
            time--;

            //イベントを発行
            timerSubject.OnNext(time);

            //1秒待つ
            yield return new WaitForSeconds(1);
        }

    }
}