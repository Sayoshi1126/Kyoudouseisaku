using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Utilities;

/// <summary>
/// Button�ɃA�^�b�`���Ďg��
/// ����Button�ȊO�ɂ����p�\��
/// </summary>
public class UIFunciton : MonoBehaviour
{
    [SerializeField] GameScenes _scene;
    [SerializeField] GameObject []_activePanel;
    [SerializeField] GameObject []_disActivePanel;

    /// <summary>
    /// _scene�V�[���ɑJ�ڂ���
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
    /// _panel���j���[��\���������͕���
    /// _panelSetActive�ɂ��\������\���̂ǂ��炩�ݒ�
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
