using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// x������y���ւ̏œ_�̈ړ��𐧌�����ۂɎg�p����I�u�W�F�N�g
/// ��������ꍇ�A�J�����̏œ_�����̃I�u�W�F�N�g�Ɍ����A�I�u�W�F�N�g��x������y���ɑ΂��Ă̂�
/// �v���C���[�Ɠ������W�ֈړ�����悤�ɂȂ�
/// �������������Ƃ���SpriteRenderer���I���ɂ���
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

    public void SetActiveFocusObj(bool freezeX,bool freezeY, bool state, GameObject target)
    {
        gameObject.SetActive(state);
        _targetObj = target;
        _freeseX = freezeX;
        _freeseY = freezeY;

    }
}
