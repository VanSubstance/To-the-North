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
            2 * Time.deltaTime * new Vector2(
                GlobalComponent.Common.userTf.position.x - transform.position.x + targetPos.x + headHorPos.x + headVerPos.x,
                GlobalComponent.Common.userTf.position.y - transform.position.y + targetPos.y + headHorPos.y + headVerPos.y
                )
            );
    }
}
