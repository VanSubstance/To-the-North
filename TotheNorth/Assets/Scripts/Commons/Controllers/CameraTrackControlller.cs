using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

public class CameraTrackControlller : MonoBehaviour
{
    public static Vector3
        targetDir = Vector3.zero;

    void Start()
    {
    }

    private void LateUpdate()
    {
        transform.Translate(
            new Vector2(
                GlobalComponent.Common.userTf.position.x - transform.position.x + targetDir.x,
                GlobalComponent.Common.userTf.position.y - transform.position.y + targetDir.y
                )
                * Time.deltaTime * 2
            );
    }
}
