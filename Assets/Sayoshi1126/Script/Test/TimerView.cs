using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class TimerView : MonoBehaviour
{
    //���ꂼ��C���X�^���X�̓C���X�y�N�^�r���[����ݒ�

    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private Text counterText; //uGUI��Text

    void Start()
    {
        //�^�C�}�̃J�E���^���ω������C�x���g���󂯂�uGUI Text���X�V����
        timeCounter.OnTimeChanged.Subscribe((int time) =>
        {
            //���݂̃^�C�}�l��UI�ɔ��f����
            counterText.text = time.ToString();
        });
    }
}