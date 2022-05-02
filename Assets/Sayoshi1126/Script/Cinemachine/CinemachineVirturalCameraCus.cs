using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// cinemacineを基に機能を拡張
/// </summary>
public class CinemachineVirturalCameraCus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    private Transform _focusTarget;
    private Jumper _player;
    private CinemachineFramingTransposer _transposer;
    public float FocusDistance = 0.3f;//キャラクターの進行方向へ動く焦点とキャラクターの距離
    public float FocusSpeed = 0.01f;//焦点の移動速度
    private float _focus = 0.5f;

    public bool ForwardFocus = true;//焦点を進行方向によって移動させるか
    
    //未実装
    public bool ProjectedFocus = false;
    public bool PlatformSnapping = true;


    void Start()
    {     
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _focusTarget = _virtualCamera.Follow;
        _player = _virtualCamera.Follow.GetComponent<Jumper>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player == null && _focusTarget.tag == "player") _virtualCamera.Follow.GetComponent<Jumper>();

        if (_player.Dir != 0)
        {
            if (ForwardFocus)
            {
                _focus = FocusDistance * _player.JumperLocalScale;
            }        
        }

        //forward focus
        if (ForwardFocus&& _player.Running)
        {
            if (_player.JumperLocalScale > 0)//右に向いているとき
            {
                if (_focus > _transposer.m_TrackedObjectOffset.x)
                {
                    _transposer.m_TrackedObjectOffset.x += FocusSpeed;
 
                }
                else if (_focus <= _transposer.m_TrackedObjectOffset.x)
                {
                    _transposer.m_TrackedObjectOffset.x = _focus;
                }
            }
            else if(_player.JumperLocalScale < 0)//左に向いているとき
            {
                if (_focus < _transposer.m_TrackedObjectOffset.x)
                {
                    _transposer.m_TrackedObjectOffset.x -= FocusSpeed;
                }
                else if (_focus >= _transposer.m_TrackedObjectOffset.x)
                {
                    _transposer.m_TrackedObjectOffset.x = _focus;
                }
            }
        }
    }
}
