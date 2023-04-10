using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Components.Infos;
using Assets.Scripts.Commons;

public class CommonGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform fadeImagePrefab, userPrefab, smogForScreenPrefab,
        pauseWindowPrefab, inventoryWindowPrefab,
        panelForLeftTop,
        projectileManager, trajectoryManager,
        screenHitManager
        ;

    private Image fadeImage;
    private int curStatus = 0;

    private ScreenHitEffectManager _screenHitManager;

    // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
    private static CommonGameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CommonGameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CommonGameManager)) as CommonGameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (!GlobalStatus.Loading.System.CommonGameManager)
        {
            StartCoroutine(GenerateInitialComponents());
        }
        else
        {
            switch (curStatus)
            {
                case 0:
                    FadeScreen(false);
                    curStatus = 1;
                    break;
                case 1:
                    if (GlobalStatus.Loading.System.isSystemLoadingDone())
                    {
                        GetComponent<InfoMessageManager>().AddMessageIntoQueue(new InfoStat(GlobalStatus.curScene, InfoType.ERROR));
                        curStatus = 2;
                    }
                    break;
                case 2:
                    break;
            }
        }
    }

    private IEnumerator GenerateInitialComponents()
    {
        if (curStatus == 10) yield break;
        curStatus = 10;
        while (GameObject.Find("UI") == null)
        {
            yield return new WaitForSeconds(0.01f);
        }
        Transform uiTf = GameObject.Find("UI").transform;
        // 페이드아웃 이미지 추가
        Transform imageForFade = Instantiate(fadeImagePrefab, uiTf);
        imageForFade.localPosition = Vector3.zero;
        imageForFade.localScale = Vector3.one;
        fadeImage = imageForFade.GetComponent<Image>();

        if (GameObject.Find("Field") != null)
        {
            // 필드 있음 = 유저가 있어야 함

            // esc 모달 추가
            Transform windowForPause = Instantiate(pauseWindowPrefab, uiTf);
            windowForPause.localPosition = Vector3.zero;
            KeyToggleManager keyAdded = uiTf.AddComponent<KeyToggleManager>();
            keyAdded.InitContent(KeyCode.Escape, windowForPause.GetComponent<MonoBehaviourControllByKey>());

            // 인벤토리 모달 추가
            Transform windowForInventory = Instantiate(inventoryWindowPrefab, uiTf);
            windowForPause.localPosition = Vector3.zero;
            keyAdded = uiTf.AddComponent<KeyToggleManager>();
            keyAdded.InitContent(KeyCode.I, windowForInventory.GetComponent<MonoBehaviourControllByKey>());

            // 화면 필터 이미지 추가
            Transform imageForSmog = Instantiate(smogForScreenPrefab, uiTf);
            imageForFade.localPosition = Vector3.zero;
            imageForSmog.SetAsLastSibling();

            // 유저 위치
            Transform userGo = Instantiate(userPrefab);
            userGo.localScale = Vector3.one;
            userGo.position = new Vector3(GlobalStatus.userInitPosition[0], GlobalStatus.userInitPosition[1]);
            GlobalStatus.userInitPosition = new float[] { 0, 0 };
            GlobalComponent.Common.userTf = userGo;

            // 필요한 UI
            Transform panelLeftTop = Instantiate(panelForLeftTop, uiTf);
            panelLeftTop.localScale = Vector3.one;
            panelLeftTop.localPosition = new Vector3(-960, 540, 0);
            InGameStatus.User.status.hpBar = panelLeftTop.GetComponent<UINumericController>().barForHp;
            InGameStatus.User.status.staminaBar = panelLeftTop.GetComponent<UINumericController>().barForStamina;

            // 투사체 풀
            if (GameObject.Find("Projectiles") == null)
            {
                Transform temp = Instantiate(projectileManager);
                temp.name = "Projectiles";
            }

            // 궤적 풀
            if (GameObject.Find("Trajectories") == null)
            {
                Transform temp = Instantiate(trajectoryManager);
                temp.name = "Trajectories";
            }

            // 화면 피격 풀
            if (uiTf.Find("On Hit") == null)
            {
                _screenHitManager = Instantiate(screenHitManager, uiTf).GetComponent<ScreenHitEffectManager>();
                _screenHitManager.transform.name = "On Hit";
            }
        }

        GlobalStatus.Loading.System.CommonGameManager = true;
        imageForFade.SetAsFirstSibling();
        curStatus = 0;
    }

    /// <summary>
    /// 화면 페이드 인/아웃
    /// </summary>
    /// <param name="isFadein">true -> 화면 감추기 (페이드인); false -> 화면 나타내기 (페이드아웃)</param>
    /// <param name="actionAfter">화면 애니메이션 이후 실행할 함수</param>
    /// <param name="actionBefore">화면 애니메이션 이전 실행할 함수</param>
    public void FadeScreen(bool isFadein, System.Action actionAfter = null, System.Action actionBefore = null)
    {
        FadeObject(fadeImage.transform, isFadein, 1f, actionAfter, actionBefore);
    }

    public void FadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null, System.Action actionBefore = null)
    {
        StartCoroutine(CoroutineFadeObject(targetTf, isFadeIn, accelSpeed, afterAction, actionBefore));
    }
    private IEnumerator CoroutineFadeObject(Transform targetTf, bool isFadeIn, float accelSpeed, System.Action afterAction = null, System.Action actionBefore = null)
    {
        float goalOpacity = isFadeIn ? 1.0f : 0.0f, curOpacity = isFadeIn ? 0.0f : 1.0f;
        if (actionBefore != null) actionBefore();
        targetTf.SetAsLastSibling();
        while (isFadeIn ? curOpacity < goalOpacity : curOpacity > goalOpacity)
        {
            yield return new WaitForSeconds(0.01f);
            curOpacity = curOpacity + (0.01f * (GlobalSetting.accelSpeed * (isFadeIn ? 1f : -1f)) * accelSpeed);
            if (targetTf == null) break;
            if (targetTf.GetComponent<Image>() != null) targetTf.GetComponent<Image>().color = new Color(
                targetTf.GetComponent<Image>().color.r,
                targetTf.GetComponent<Image>().color.g,
                targetTf.GetComponent<Image>().color.b,
                curOpacity);
            if (targetTf == null) break;
            if (targetTf.GetComponent<TextMeshProUGUI>() != null) targetTf.GetComponent<TextMeshProUGUI>().color = new Color(
                targetTf.GetComponent<TextMeshProUGUI>().color.r,
                targetTf.GetComponent<TextMeshProUGUI>().color.g,
                targetTf.GetComponent<TextMeshProUGUI>().color.b,
                curOpacity);
        }
        targetTf.SetAsFirstSibling();
        if (afterAction != null) afterAction();
    }

    public void MoveObject(Transform targetTf, DirectionType direction, float accelAmount, float distanceToMove, System.Action afterAction = null)
    {
        StartCoroutine(CoroutineMoveObject(targetTf, direction, accelAmount, distanceToMove, afterAction));
    }
    private IEnumerator CoroutineMoveObject(Transform targetTf, DirectionType direction, float accelAmount, float distanceToMove, System.Action afterAction = null)
    {
        float cnt = 1f;
        Vector3 dirVector = Vector3.zero;
        switch (direction)
        {
            case DirectionType.UP:
                dirVector = Vector3.up;
                break;
            case DirectionType.DOWN:
                dirVector = Vector3.down;
                break;
            case DirectionType.LEFT:
                dirVector = Vector3.left;
                break;
            case DirectionType.RIGHT:
                dirVector = Vector3.right;
                break;
        }
        while (cnt > 0f)
        {
            yield return new WaitForSeconds(0.01f);
            cnt -= 0.01f * GlobalSetting.accelSpeed * accelAmount;
            if (targetTf == null) break;
            targetTf.Translate(dirVector * distanceToMove * 0.01f * GlobalSetting.accelSpeed * accelAmount);
        }
        if (afterAction != null) afterAction();
    }

    public void MoveScene(string targetSceneName)
    {
        FadeScreen(true, () =>
        {
            GlobalStatus.resetLoading();
            GlobalStatus.nextScene = targetSceneName;
            SceneManager.LoadScene("Loading");
        });
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SaveGame()
    {

    }

    /// <summary>
    /// 피격 관련 광역 이벤트 처리 함수
    /// </summary>
    /// <param name="degree">피격 발생 각도 (화면 중간 right 반시계 기준 각도)</param>
    public void OnHit(float degree)
    {
        _screenHitManager.OnHit(degree);
    }
}
