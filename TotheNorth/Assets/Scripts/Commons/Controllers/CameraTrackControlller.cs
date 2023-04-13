using UnityEngine;

public class CameraTrackControlller : MonoBehaviour
{
    public static Vector3
        targetPos = Vector3.zero,
        headHorPos = Vector3.zero, headVerPos = Vector3.zero;

    private void Awake()
    {
        Camera.main.orthographicSize = 8;
    }

    private void Update()
    {
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 3;
        if (Camera.main.orthographicSize < 6) Camera.main.orthographicSize = 6;
        if (Camera.main.orthographicSize > 10) Camera.main.orthographicSize = 10;
    }

    private void LateUpdate()
    {
        transform.Translate(
            2 * Time.deltaTime * new Vector2(
                GlobalComponent.Common.userController.x - transform.position.x + targetPos.x + headHorPos.x + headVerPos.x,
                GlobalComponent.Common.userController.y - transform.position.y + targetPos.y + headHorPos.y + headVerPos.y
                )
            );
    }
}
