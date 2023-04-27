using System.Collections;
using Assets.Scripts.Commons;
using Assets.Scripts.Components.Infos;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GlobalComponent.Common;

public class CommonGameManager : MonoBehaviour
{
    [SerializeField]
    private Transform fadeImagePrefab, userPrefab, filterForScreenPrefab,
        pauseWindowPrefab, inventoryWindowPrefab,
        panelForHpSp, panelForCondition, panelForWelfare, panelForQuick,
        projectileManager, trajectoryManager,
        screenHitManager,
        hoveringItemInfo
        ;

    private Image fadeImage;
    private int curStatus = 0;

    private ScreenHitEffectManager _screenHitManager;
    private CameraHitEffectController _cameraHitController;
    private ScreenHitFilterController _screenHitFilterController;

    private static CommonGameManager _instance;
    // 인스턴스에 접근하기 위한 프로퍼티
    public static CommonGameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(CommonGameManager)) as CommonGameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public ScreenHitEffectManager ScreenHitManager
    {
        get
        {
            return _screenHitManager;
        }
    }

    public CameraHitEffectController CameraHitController
    {
        get
        {
            return _cameraHitController;
        }
    }

    public ScreenHitFilterController ScreenHitFilterController
    {
        get
        {
            return _screenHitFilterController;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            StartCoroutine(GenerateInitialComponents());
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
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
        if (uiTf.GetComponent<UIManager>().isInit)
        {
            CameraTrackControlller.Instance.transform.position = new Vector3(GlobalStatus.userInitPosition[0], 0, GlobalStatus.userInitPosition[1]);
            UserBaseController.Instance.position = new Vector3(GlobalStatus.userInitPosition[0], 2, GlobalStatus.userInitPosition[1]);
            GlobalStatus.userInitPosition = new float[] { 0, 0 };
            GlobalStatus.Loading.System.CommonGameManager = true;
            curStatus = 0;
            yield break;
        }
        uiTf.GetComponent<UIManager>().isInit = true;
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

            // 인벤토리 모달 추가
            Transform windowForInventory = Instantiate(inventoryWindowPrefab, uiTf);
            //windowForPause.localPosition = Vector3.zero;
            //keyAdded = uiTf.AddComponent<KeyToggleManager>();
            //keyAdded.InitContent(KeyCode.I, windowForInventory.GetComponent<IControllByKey>());

            Transform hovering = Instantiate(hoveringItemInfo, uiTf);
            KeyToggleManager keyAdded = uiTf.AddComponent<KeyToggleManager>();
            keyAdded.InitContent(KeyCode.I, hovering.GetComponent<IControllByKey>());

            // 화면 필터 이미지 추가
            Transform imageForSmog = Instantiate(filterForScreenPrefab, uiTf);
            _screenHitFilterController = imageForSmog.GetComponent<ScreenHitFilterController>();
            imageForFade.localPosition = Vector3.zero;
            imageForSmog.SetAsLastSibling();

            // 유저 위치
            Transform userGo = Instantiate(userPrefab);
            userGo.localScale = Vector3.one;
            userGo.position = new Vector3(GlobalStatus.userInitPosition[0], .25f, GlobalStatus.userInitPosition[1]);
            GlobalStatus.userInitPosition = new float[] { 0, 0 };

            // 체력 UI
            Transform panelLeftTop = Instantiate(panelForHpSp, uiTf);
            panelLeftTop.localScale = Vector3.one;
            InGameStatus.User.status.hpBar = panelLeftTop.GetComponent<UIHpSpController>().barForHp;
            InGameStatus.User.status.staminaBar = panelLeftTop.GetComponent<UIHpSpController>().barForStamina;
            //panelLeftTop.SetAsFirstSibling();

            // 상태 이상 표기용 UI
            Transform panelCondition = Instantiate(panelForCondition, uiTf);
            panelCondition.localScale = Vector3.one;

            // 건강 UI
            Transform PanelForWelfare = Instantiate(panelForWelfare, uiTf);
            PanelForWelfare.localScale = Vector3.one;
            InGameStatus.User.status.hungerBar = PanelForWelfare.GetComponent<UIWelfareController>().barForHunger;
            InGameStatus.User.status.thirstBar = PanelForWelfare.GetComponent<UIWelfareController>().barForThirst;
            InGameStatus.User.status.temperatureBar = PanelForWelfare.GetComponent<UIWelfareController>().barForTemperature;
            InGameStatus.User.status.temperatureBar.LiveInfo = -50;

            // 퀵슬롯 UI
            Transform PanelForQuick = Instantiate(panelForQuick, uiTf);
            PanelForQuick.localScale = Vector3.one;

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

            // 화면 피격 풀 + 카메라 피격 컨트롤러
            if (uiTf.Find("On Hit") == null)
            {
                _screenHitManager = Instantiate(screenHitManager, uiTf).GetComponent<ScreenHitEffectManager>();
                _screenHitManager.transform.name = "On Hit";
                _cameraHitController = GameObject.Find("Camera Container").transform.GetChild(0).GetComponent<CameraHitEffectController>();
            }
        }

        GlobalStatus.Loading.System.CommonGameManager = true;
        imageForFade.SetAsFirstSibling();

        SceneManager.sceneLoaded += (scene, mode) => FadeScreen(false);
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
    /// <param name="damage">[0]: 전체 데미지, [1]: 관통 데미지, [2]: 충격 데미지</param>
    public void OnHit(float degree, int[] damage)
    {
        // 데미지 계산
        InGameStatus.User.status.ApplyDamage(damage[0]);
        _screenHitManager.OnHit(degree);
        _cameraHitController.OnHit(damage[2]);
        _screenHitFilterController.OnHit(damage[1]);
    }

    /// <summary>
    /// 소모성 아이템 (식량, 의약품) 사용 함수
    /// </summary>
    /// <param name="_info"></param>
    public void ApplyConsumable(ItemConsumableInfo _info)
    {
        if (InGameStatus.User.isInConsume) return;
        StartCoroutine(CoroutineConsume(_info));
    }
    private IEnumerator CoroutineConsume(ItemConsumableInfo _info)
    {
        InGameStatus.User.isInConsume = true;
        _info.Use(1);
        UserBaseController.Instance.progress.CurProgress = 0;
        float tRemaining = _info.SecondConsume, p = 1;
        if (_info is ItemFoodInfo)
        {
            ItemFoodInfo fInfo = Instantiate((ItemFoodInfo)_info);
            // 음식일 경우
            while (tRemaining > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                p = Time.deltaTime / tRemaining;
                UserBaseController.Instance.progress.CurProgress += p;
                tRemaining -= Time.deltaTime;
                // 초당 적용
                InGameStatus.User.status.ApplyHunger(fInfo.Hunger * p);
                InGameStatus.User.status.ApplyThirst(fInfo.Thirst * p);
                InGameStatus.User.status.ApplyTemperature(fInfo.Temperature * p);
                fInfo.Hunger *= (1 - p);
                fInfo.Thirst *= (1 - p);
                fInfo.Temperature *= (1 - p);
            }
        }
        else if (_info is ItemMedicineInfo)
        {
            ItemMedicineInfo mInfo = Instantiate((ItemMedicineInfo)_info);
            // 의약품일 경우
            while (tRemaining > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                p = Time.deltaTime / tRemaining;
                UserBaseController.Instance.progress.CurProgress += p;
                tRemaining -= Time.deltaTime;
                // 초당 적용
                InGameStatus.User.status.ApplyDamage(-mInfo.Hp * p);
                mInfo.Hp *= (1 - p);
            }
            foreach (MedicineConditionEffect effect in mInfo.effects)
            {
                UserBaseController.Instance.CureCondition(effect.targetCondition, effect.countToRemove);
            }
        }
        InGameStatus.User.isInConsume = false;
    }
}
