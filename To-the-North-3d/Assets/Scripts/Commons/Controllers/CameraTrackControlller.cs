using Assets.Scripts.Commons.Constants;
using UnityEngine;
using Assets.Scripts.Users;

public class CameraTrackControlller : MonoBehaviour
{
    public static Vector3 MousePosOnTerrain;
    public static Vector3
        targetPos = Vector3.zero,
        headHorPos = Vector3.zero, headVerPos = Vector3.zero;

    private static int speedTracking = 2;

    private static CameraTrackControlller _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CameraTrackControlller Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CameraTrackControlller)) as CameraTrackControlller;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        Camera.main.orthographicSize = 8;
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!InGameStatus.User.isPause)
        {
            TrackZoom();
            TrackMousePosOnTerrain();
        }
    }

    private void LateUpdate()
    {
        float weight = 1;
        if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedCameraTracking))
        {
            weight /= 2;
        }
        transform.Translate(
            weight * speedTracking * Time.deltaTime * new Vector3(
                UserBaseController.Instance.x - transform.position.x + targetPos.x + headHorPos.x + headVerPos.x,
                0,
                UserBaseController.Instance.z - transform.position.z + targetPos.z + headHorPos.z + headVerPos.z
                )
            );
    }

    private void TrackZoom()
    {
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 3;
        if (Camera.main.orthographicSize < 6) Camera.main.orthographicSize = 6;
        if (Camera.main.orthographicSize > 10) Camera.main.orthographicSize = 10;
    }

    private void TrackMousePosOnTerrain()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Physics.Raycast(mousePos, Camera.main.transform.forward, out RaycastHit hit, 100, 1 << 19))
        {
            MousePosOnTerrain = hit.point;
        }
    }
}
