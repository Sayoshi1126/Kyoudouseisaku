using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")] // Don't display in add component menu
[RequireComponent(typeof(CinemachinePipeline))]
[SaveDuringPlay]
public class CinemachinePan : CinemachineComponentBase
{
    public override bool IsValid
    {
        get { return enabled && LookAtTarget != null; }
    }

    public override CinemachineCore.Stage Stage
    {
        get { return CinemachineCore.Stage.Aim; }
    }

    [Range(-180, 180)]
    public float xAngleOffset, yAngleOffset;

    public override void MutateCameraState(ref CameraState curState, float deltaTime)
    {
        if (IsValid && curState.HasLookAt)
        {
            Vector3 dir = (curState.ReferenceLookAt - curState.CorrectedPosition);
            var offsetRotation = Quaternion.AngleAxis(-xAngleOffset, Vector3.up) * Quaternion.AngleAxis(-yAngleOffset, Vector3.right);
            dir.y = curState.PositionCorrection.y;
            if (dir.magnitude > Epsilon)
            {
                if (Vector3.Cross(dir.normalized, curState.ReferenceUp).magnitude < Epsilon)
                    curState.RawOrientation = Quaternion.FromToRotation(Vector3.forward, dir) * offsetRotation;
                else
                    curState.RawOrientation = Quaternion.LookRotation(dir, curState.ReferenceUp) * offsetRotation;
            }
        }
    }
}