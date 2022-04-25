using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
/// <summary>
/// �q�b�g�X�g�b�v��W���X�g������ȂǃQ�[���̎��Ԃ̑��x�𒲐�����
/// </summary>
public class TimeScaleManager : SingletonMonoBehaviour<TimeScaleManager>
{
    // Start is called before the first frame update
    // Update is called once per frame
    public bool _witchTime{get; set;}
    bool _hitStop;

    void Update()
    {
        //�W���X�g��𐬌���(WitchTime)��q�b�g�X�g�b�v���Ɏ��Ԃ�x������
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
    /// �q�b�g�X�g�b�v���������Ƃ��ɓǂ�
    /// Time�̒l�������Ԃ��x���Ȃ�
    /// </summary>
    /// <param name="Time"></param>
    public void HitStop(int Time)
    {
        _hitStop = true;
        Observable.TimerFrame(Time).Subscribe(_ => _hitStop = false);
    }

}
