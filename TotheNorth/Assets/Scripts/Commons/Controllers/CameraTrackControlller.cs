using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

public class CameraTrackControlller : MonoBehaviour
{
    public static Vector3
        pointDistort = Vector3.zero;
    private Vector3
        curPointDistort = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void LateUpdate()
    {
        transform.position =
            new Vector3(
                GlobalComponent.Common.userTf.position.x,
                GlobalComponent.Common.userTf.position.y,
                -20f
                ) +
            new Vector3(
                curPointDistort.x,
                curPointDistort.y,
                0
                );
        if (!InGameStatus.User.Detection.Sight.isControllInRealTime)
        {
            // Vector3 curDistortionVector = CalculationFunctions.DirFromAngle(InGameStatus.User.Movement.curdegree) * (InGameStatus.User.Detection.Sight.range - InGameStatus.User.Detection.Sight.rangeMin);
            // 왜곡값 줄이기
            if (curPointDistort.magnitude < 0.1f)
            {
                curPointDistort = Vector3.zero;
            }
            else
            {
                curPointDistort *= 15 / 16f;
            }
        }
        else
        {
            curPointDistort += pointDistort * 1 / 16f;
            pointDistort *= 15 / 16f;
        }
    }
}
