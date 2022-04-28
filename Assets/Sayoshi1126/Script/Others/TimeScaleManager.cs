using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// ヒットストップやジャスト回避時などゲームの時間の速度を調整する
/// </summary>
public class TimeScaleManager : SingletonMonoBehaviour<TimeScaleManager>
{
    // Start is called before the first frame update
    // Update is called once per frame
    public bool _witchTime{get; set;}
    bool _hitStop;

    void Update()
    {
        //ジャスト回避成功時(WitchTime)やヒットストップ時に時間を遅くする
        if(_witchTime )
        {
            Time.timeScale = 0.4f;
        }
        else if (_hitStop == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// ヒットストップさせたいときに読む
    /// Timeの値だけ時間が遅くなる
    /// </summary>
    /// <param name="Time"></param>
    public void HitStop(int Time)
    {
        _hitStop = true;
        Observable.TimerFrame(Time).Subscribe(_ => _hitStop = false);
    }

}
