using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineFreezePivot : CinemachineExtension
{
    // �eStage�̃J�������[�N������ɌĂ΂��R�[���o�b�N
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {

    }
}
