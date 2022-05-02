using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Script.Utilities;

/// <summary>
/// �Q�[���v���C��ʃv���g�^�C�v�̃V�[���J�ڂ𑀍삷��̂Ɏg�p
/// </summary>
public class SceneLoaderTest : SingletonMonoBehaviour<SceneLoaderTest>
{
    // Start is called before the first frame update
    void Start()
    {
        //SceneLoader.LoadScene(GameScenes.SampleScene);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.LoadScene(GameScenes.TitleScene);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneLoader.LoadScene(GameScenes.SampleScene);
        }
    }
}
