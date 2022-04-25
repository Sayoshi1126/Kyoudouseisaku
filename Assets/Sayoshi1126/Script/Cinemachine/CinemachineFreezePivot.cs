using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineFreezePivot : CinemachineExtension
{
    // 各Stageのカメラワーク処理後に呼ばれるコールバック
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {

    }
}
