using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

public class CameraTrackControlller : MonoBehaviour
{
    public static Vector3
        targetPos = Vector3.zero,
        headHorPos = Vector3.zero, headVerPos = Vector3.zero;

    void Start()
    {
    }

    private void LateUpdate()
    {
        transform.Translate(
            new Vector2(
                GlobalComponent.Common.userTf.position.x - transform.position.x + targetPos.x + headHorPos.x + headVerPos.x,
                GlobalComponent.Common.userTf.position.y - transform.position.y + targetPos.y + headHorPos.y + headVerPos.y
                )
                * Time.deltaTime * 2
            );
    }
}
