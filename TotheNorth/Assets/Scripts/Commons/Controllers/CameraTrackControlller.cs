using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

public class CameraTrackControlller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    private void LateUpdate()
    {
        Vector3 curDistortionVector = CalculationFunctions.DirFromAngle(InGameStatus.User.Movement.curdegree) * (InGameStatus.User.Detection.Sight.range - InGameStatus.User.Detection.Sight.rangeMin);
        transform.position = new Vector3(
            GlobalComponent.Common.userTf.position.x,
            GlobalComponent.Common.userTf.position.y,
            -20f) + curDistortionVector;
    }
}
