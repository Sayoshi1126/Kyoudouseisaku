using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 書くUIのPanelを開いたときに最初に選択されているボタンを設定
/// </summary>

public class ChangeDefaultSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    void OnEnable()
    {
        DefaultButton.Select();
    }
}
