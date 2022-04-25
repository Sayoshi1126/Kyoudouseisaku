using System.Collections;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 100����J�E���g�_�E�����l��ʒm����T���v��
/// </summary>
public class TimeCounter : MonoBehaviour
{
    //�C�x���g�𔭍s����j�ƂȂ�C���X�^���X
    private Subject<int> timerSubject = new Subject<int>();

    //�C�x���g�̍w�Ǒ����������J
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
        //100����J�E���g�_�E��
        var time = 100;
        while (time > 0)
        {
            time--;

            //�C�x���g�𔭍s
            timerSubject.OnNext(time);

            //1�b�҂�
            yield return new WaitForSeconds(1);
        }

    }
}