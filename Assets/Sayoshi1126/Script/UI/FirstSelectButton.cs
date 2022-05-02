using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ゲーム実行直後に選択されてるボタンを設定
/// </summary>

public class FirstSelectButton : MonoBehaviour
{
    public Button DefaultButton;
    private void Start()
    {
        DefaultButton.Select();
    }
}