using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����UI��Panel���J�����Ƃ��ɍŏ��ɑI������Ă���{�^����ݒ�
/// </summary>

public class ChangeDefaultSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    void OnEnable()
    {
        DefaultButton.Select();
    }
}
