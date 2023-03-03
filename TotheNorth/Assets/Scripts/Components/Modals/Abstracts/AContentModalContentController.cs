using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AContentModalController<TContent> : MonoBehaviour, IContentModalContentController
{
    [SerializeField]
    private Vector2 sizeToUnit;
    private int curStatus = 0;
    void Start()
    {
        InitComposition();
        Toggle(false);
        StartTracking();
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 1:
                break;
            case 2:
                // 눈에 안보이는 중
                // = 키 토글만 추적
                break;
            case 3:
                // 눈에 보이는 중
                // = 키 토글 + 이동 둘 다 추적
                break;
        }
    }

    // 모달 토글
    private void Toggle(bool isOpen = true)
    {
        curStatus = isOpen ? 3 : 2;
        gameObject.SetActive(isOpen);
    }

    // 추적 시작
    private void StartTracking()
    {
        StartCoroutine(CoroutineTracking());
        curStatus = 2;
    }

    // 추적 코루틴
    private IEnumerator CoroutineTracking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            // 키 토글 추적
            TrackingToggleKey();
            if (curStatus == 3)
            {
                // 마우스 이동 추적
                TrackingMove();
            }
        }
    }
    // 토글 키 추적
    private void TrackingToggleKey()
    {

    }
    // 이동 마우스 이벤트 추적
    private void TrackingMove()
    {

    }

    public void InitContent<T>(T contentToInit)
    {
        if (typeof(T).Equals(typeof(TContent)))
        {
            InitContentByType((TContent)(object)contentToInit);
        }
        return;
    }

    private void InitComposition()
    {
        transform.GetComponent<RectTransform>().sizeDelta = sizeToUnit * GlobalSetting.unitSize;
    }
    // 컨텐츠 비우기
    public abstract void clearContent();
    // 컨텐츠 적용하기
    public abstract void InitContentByType(TContent contentToInit);
}
