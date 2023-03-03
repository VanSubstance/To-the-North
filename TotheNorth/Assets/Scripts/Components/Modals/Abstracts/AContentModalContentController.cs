using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AContentModalController<TContent> : MonoBehaviour, IContentModalContentController
{
    [SerializeField]
    private Vector2 sizeToUnit;
    private Button btnX;
    private Transform contentContainerTf, headerTf;
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
            case 0:
                InitComposition();
                break;
            case 1:
                // 초기화 종료
                break;
            case 2:
                // 눈에 안보이는 중
                break;
            case 3:
                // 눈에 보이는 중
                break;
        }
    }

    // 모달 토글
    // purpose: 0 <- 그냥 토글, 1 <- 열기, 2 <- 닫기
    public void Toggle(int purpose = 0)
    {
        bool toOpen = purpose == 1 ? true : purpose == 2 ? false :
            gameObject.activeSelf ? false : true;
        ;
        gameObject.SetActive(toOpen);
    }

    private void InitComposition()
    {
        if (curStatus == 1) return;
        headerTf = transform.GetChild(0);
        contentContainerTf = transform.GetChild(1);
        transform.GetComponent<RectTransform>().sizeDelta = sizeToUnit * GlobalSetting.unitSize;
        btnX = headerTf.GetChild(0).GetComponent<Button>();
        btnX.onClick.AddListener(() =>
        {
            Toggle(2);
        });
        InitCompositionByType();
        curStatus = 1;
    }

    public Transform GetContentContainer()
    {
        if (contentContainerTf == null) InitComposition();
        return contentContainerTf;
    }

    public abstract void InitCompositionByType();
    // 컨텐츠 비우기
    public abstract void ClearContent();
    // 컨텐츠 적용하기
    public abstract void InitContentByType(TContent contentToInit);
}
