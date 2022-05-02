using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// x軸又はy軸への焦点の移動を制限する際に使用するオブジェクト
/// このオブジェクトの範囲に入ったプレイヤーオブジェクトの座標を追って、x軸もしくはy軸に沿った動きをする
/// 動きを見たいときはSpriteRendererをオンにする
/// </summary>
public class CameraFreezePivotObject : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _targetObj;
    bool _freeseX;
    bool _freeseY;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_targetObj != null)
        {
            Vector2 focusPosition_ = transform.position;
            float x_ = focusPosition_.x;
            float y_ = focusPosition_.y;
            if (!_freeseX)
            {
                x_ = _targetObj.transform.position.x;
            }

            if (!_freeseY)
            {
                y_ = _targetObj.transform.position.y;
            }

            focusPosition_ = new Vector2(x_, y_);
            transform.position = focusPosition_;
        }
        else
        {
            Debug.Log("cannot access target object");
        }
    }

    /// <summary>
    /// カメラに追わせるためのこのオブジェクトをアクティブにする
    /// </summary>
    /// <param name="freezeX"></param>
    /// <param name="freezeY"></param>
    /// <param name="state"></param>
    /// <param name="target"></param>
    public void SetActiveFocusObj(bool freezeX, bool freezeY, bool state, GameObject target)
    {
        gameObject.SetActive(state);
        _targetObj = target;
        _freeseX = freezeX;
        _freeseY = freezeY;

    }
}
