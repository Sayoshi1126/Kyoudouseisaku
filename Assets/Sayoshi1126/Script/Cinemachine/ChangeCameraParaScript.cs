using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
/// <summary>
/// 範囲内に入ったプレイヤーを追従するcinemacineCameraのパラメタを変更する
/// </summary>
public class ChangeCameraParaScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _cameraObject;
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineFramingTransposer _transposer;
    private CinemachineVirturalCameraCus _virtualCameraCus;
    private BoxCollider2D _boxCollider2d;

    [SerializeField] private Vector3 _trackedObjectOffset=new Vector3(0,1.31f,0);
    [SerializeField] private float _x_Damping=1;
    [SerializeField] private float _y_Damping=1;
    [SerializeField] private float _deadZoneWidth=0.13f;
    [SerializeField] private float _biasY=0;
    [SerializeField] private bool _forwardFocus=true;
    [SerializeField] private bool _freezeX=false;
    [SerializeField] private bool _freezeY=false;
    private CameraFreezePivotObject _focusObj;
    void Start()
    {
        _cameraObject = GameObject.Find("CM vcam1");
        if (_cameraObject == null)
        {
            Debug.Log("cannot find camera object");
        }
        _virtualCamera = _cameraObject.GetComponent<CinemachineVirtualCamera>();
        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _virtualCameraCus = _cameraObject.GetComponent<CinemachineVirturalCameraCus>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        if (transform.childCount > 0)
        {
            Debug.Log("getChild");
            _focusObj = transform.GetChild(0).GetComponent<CameraFreezePivotObject>();
        }
    }

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            CameraParaUpdate();
            if (_focusObj!=null&&(_freezeX||_freezeY))
            {
                _focusObj.SetActiveFocusObj(_freezeX,_freezeY,true,collision.gameObject);
                _virtualCamera.Follow = _focusObj.gameObject.transform;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (_focusObj != null)
            {
                _focusObj.SetActiveFocusObj(_freezeX, _freezeY,false, collision.gameObject);
                _virtualCamera.Follow = collision.gameObject.gameObject.transform;
            }
        }
    }

    /// <summary>
    /// CinemachineVirtualCameraのパラメータをインスペクタの値にしたがって更新する
    /// </summary>
    void CameraParaUpdate()
    {
        _transposer.m_TrackedObjectOffset = _trackedObjectOffset;
        _transposer.m_DeadZoneWidth = _deadZoneWidth;
        _transposer.m_BiasY = _biasY;
        _virtualCameraCus.ForwardFocus = _forwardFocus;
        _transposer.m_XDamping = _x_Damping;
        _transposer.m_YDamping = _y_Damping;
    }
}
