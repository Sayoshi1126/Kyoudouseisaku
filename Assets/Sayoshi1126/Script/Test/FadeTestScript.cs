using UnityEngine;
/// <summary>
/// シーン遷移機構のテスト用
/// </summary>
public class FadeTestScript : MonoBehaviour
{
    public Fade fade;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            fade.FadeIn(0.5f, () => print("フェードイン完了"));
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            fade.FadeOut(0.5f, () => print("フェードアウト完了"));
        }
    }
}