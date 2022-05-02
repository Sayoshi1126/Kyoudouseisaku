using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Utilities;

/// <summary>
/// Buttonにアタッチして使う
/// 今後Button以外にも応用予定
/// </summary>
public class UIFunciton : MonoBehaviour
{
    [SerializeField] GameScenes _scene;
    [SerializeField] GameObject []_activePanel;
    [SerializeField] GameObject []_disActivePanel;

    /// <summary>
    /// _sceneシーンに遷移する
    /// </summary>
    public void GoToSceneButton()
    {
        SceneLoader.LoadScene(_scene);
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// _panelメニューを表示もしくは閉じる
    /// _panelSetActiveにより表示か非表示のどちらか設定
    /// </summary>
    public void SetActivePanel()
    {
        foreach (GameObject i in _disActivePanel)
        {
            i.SetActive(false);
        }
        foreach (GameObject i in _activePanel)
        {
            i.SetActive(true);
        }
    }
}
