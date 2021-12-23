using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineVirturalCameraCus : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineFramingTransposer transposer;
    [SerializeField] private float focusDistance = 0.3f;
    [SerializeField] float focusSpeed = 0.01f;
    private float focus = 0.5f;

    [SerializeField] private bool ForwardFocus = true;
    [SerializeField] private bool ProjectedFocus = false;
    [SerializeField] private bool platformSnapping = true;

    private bool lastJumping;
    private bool stopCamera;

    private float DZW;//dead zone weight
    private float DZH;
    private float SZW;//soft zone weight
    private float SZH;


    void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        stopCamera = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Settings.Instance.dir != 0)
        {
            if (ForwardFocus)
            {
                focus = 0.5f - focusDistance * Settings.Instance.dir;
            }
            else if (ProjectedFocus)
            {
                focus = 0.5f - focusDistance * Settings.Instance.jumperVX / Settings.Instance.maxVx;
                if(Settings.Instance.jumperVX==0&&ForwardFocus)
                {
                    focus = 0.5f - focusDistance * Settings.Instance.dir;
                }
            }
            else
            {
                focus = 0.5f;
            }        
        }


        //projected focus
        if(focus<0.5f)
        {
            if (focus < transposer.m_ScreenX)
            {
                transposer.m_ScreenX -= focusSpeed;
            }
            else
            {
                transposer.m_ScreenX = focus;
            }
        }
        else if(focus>0.5f)
        {
            if (focus > transposer.m_ScreenX)
            {
                transposer.m_ScreenX += focusSpeed;
            }
            else
            {
                transposer.m_ScreenX = focus;
            }
        }

        //platformSnapping
        if(lastJumping!=Settings.Instance.jumping&&platformSnapping)
        {
            if(Settings.Instance.jumping==true)
            {
                //platform snapping start
                stopCamera = true;
                changeZone(1,1,1,1);
                
            }
            else
            {
                //platform snapping end
                stopCamera = false;
                changeZone(DZW,DZH,SZW,SZH);
            }
        }

        if(transposer.m_DeadZoneWidth!=DZW || transposer.m_DeadZoneHeight!=DZH || transposer.m_SoftZoneWidth != SZW || transposer.m_SoftZoneHeight != SZH)
        {
            if( transposer.m_DeadZoneHeight != 1 ||transposer.m_SoftZoneHeight != 1)
            {
                DZW=transposer.m_DeadZoneWidth;
                DZH=transposer.m_DeadZoneHeight;
                SZW=transposer.m_SoftZoneWidth;
                SZH=transposer.m_SoftZoneHeight;

            }
        }

        lastJumping = Settings.Instance.jumping;
    }

    void changeZone(float dzw,float dzh,float szw,float szh)
    {
        transposer.m_DeadZoneHeight = dzh;
        //transposer.m_DeadZoneWidth = dzw;
        transposer.m_SoftZoneHeight = szh;
        //transposer.m_SoftZoneWidth = szw;
    }
}
