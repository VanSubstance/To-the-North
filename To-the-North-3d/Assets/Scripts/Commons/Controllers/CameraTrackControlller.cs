using Assets.Scripts.Commons;
using Assets.Scripts.Creatures;
using Assets.Scripts.Users;
using Assets.Scripts.SoundEffects;
using UnityEngine;

public class CameraTrackControlller : MonoBehaviour, ISoundable
{
    public static Vector3 MousePosOnTerrain;
    public static Vector3
        targetPos = Vector3.zero,
        headHorPos = Vector3.zero, headVerPos = Vector3.zero;

    private static int speedTracking = 2;

    private AudioSource Speaker, SpeakerEnvironment;
    [SerializeField]
    private AudioClip audGroundIn, audGroundOut, audUnderGround;

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
        SoundEffectManager.AddAudioSource(transform, true, out Speaker);
        SoundEffectManager.AddAudioSource(transform, true, out SpeakerEnvironment);
        Speaker.maxDistance = 100;
        SpeakerEnvironment.maxDistance = 100;
    }

    private void Start()
    {
        PlaySoundByType(SoundType.GroundOut);
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
        if (Camera.main.orthographicSize < 5f) Camera.main.orthographicSize = 5f;
        if (Camera.main.orthographicSize > 10f) Camera.main.orthographicSize = 10f;
    }

    private void TrackMousePosOnTerrain()
    {
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, out RaycastHit hit, 100, 1 << 19))
        {
            MousePosOnTerrain = hit.point;
        }
    }

    public void PlaySound(AudioClip _clip = null)
    {
        if (_clip != null)
        {
            Speaker.clip = _clip;
        }
        Speaker.Play();
    }

    public void StopSound()
    {
        Speaker.Stop();
    }

    public void PlaySoundByType(SoundType _type)
    {
        if (_type.Equals(IsSoundInPlaying())) return;
        switch (_type)
        {
            case SoundType.None:
                StopSound();
                break;
            case SoundType.GroundIn:
                PlaySound(audGroundIn);
                break;
            case SoundType.GroundOut:
                PlaySound(audGroundOut);
                break;
            case SoundType.UnderGround:
                PlaySound(audUnderGround);
                break;
        }
    }

    public SoundType IsSoundInPlaying()
    {
        if (!Speaker.isPlaying) return SoundType.None;
        AudioClip c = Speaker.clip;
        if (c.Equals(audGroundIn)) return SoundType.GroundIn;
        if (c.Equals(audGroundOut)) return SoundType.GroundOut;
        if (c.Equals(audUnderGround)) return SoundType.UnderGround;
        return SoundType.None;
    }
}
