using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �Q�[�����s����ɑI������Ă�{�^����ݒ�
/// </summary>

public class FirstSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    private void Start()
    {
        DefaultButton.Select();
    }
}