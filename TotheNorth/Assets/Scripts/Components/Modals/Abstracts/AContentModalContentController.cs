using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AContentModalController<TContent> : MonoBehaviour, IContentModalContentController
{
    [SerializeField]
    private Vector2 sizeToUnit;
    private Button btnX;
    private int curStatus = 0;
    void Start()
    {
        InitComposition();
        Toggle(2);
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
                break;
            case 3:
                // 눈에 보이는 중
                TrackingMove();
                break;
        }
    }

    // 모달 토글
    // purpose: 0 <- 그냥 토글, 1 <- 열기, 2 <- 닫기
    public void Toggle(int purpose = 0)
    {
        bool toOpen = purpose == 1 ? true : purpose == 2 ? false :
            gameObject.activeSelf ? false : true;
        curStatus = toOpen ? 3 : 2;
        ;
        gameObject.SetActive(toOpen);
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
        btnX = transform.GetChild(0).GetChild(0).GetComponent<Button>();
        btnX.onClick.AddListener(() =>
        {
            Toggle(2);
        });
    }
    // 컨텐츠 비우기
    public abstract void clearContent();
    // 컨텐츠 적용하기
    public abstract void InitContentByType(TContent contentToInit);
}
