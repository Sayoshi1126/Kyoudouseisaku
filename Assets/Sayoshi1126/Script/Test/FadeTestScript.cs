using UnityEngine;
/// <summary>
/// �V�[���J�ڋ@�\�̃e�X�g�p
/// </summary>
public class FadeTestScript : MonoBehaviour
{
    public Fade fade;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fade.FadeIn(0.5f, () => print("�t�F�[�h�C������"));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            fade.FadeOut(0.5f, () => print("�t�F�[�h�A�E�g����"));
        }
    }
}