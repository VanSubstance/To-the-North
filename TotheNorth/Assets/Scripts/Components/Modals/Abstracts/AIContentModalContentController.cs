using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AIContentModalContentController : MonoBehaviourControllByKey, IContentModalContentController
{
    [SerializeField]
    private Vector2 sizeToUnit;

    private Button btnX;
    private int curStatus = 0;
    private Transform contentContainerTf, headerTf;
    void Start()
    {
        InitComposition();
        ControllByKey(2);
    }

    public void InitComposition()
    {
        if (curStatus == 1) return;
        curStatus = 1;
        headerTf = transform.GetChild(0);
        contentContainerTf = transform.GetChild(1);
        transform.GetComponent<RectTransform>().sizeDelta = sizeToUnit * GlobalSetting.unitSize;
        btnX = headerTf.GetChild(0).GetComponent<Button>();
        btnX.onClick.AddListener(() =>
        {
            ControllByKey(2);
        });
        InitCompositionByType();
    }

    // 모달 토글
    // purpose: 0 <- 그냥 토글, 1 <- 열기, 2 <- 닫기
    public sealed override void ControllByKey(int purpose = 0)
    {
        bool toOpen = purpose == 1 ? true : purpose == 2 ? false :
            gameObject.activeSelf ? false : true;
        ;
        gameObject.SetActive(toOpen);
    }

    public Transform GetContentContainerTf()
    {
        return contentContainerTf;
    }

    // 추상 함수

    // 컨텐츠 비우기
    public abstract void ClearContent();

    // 해당 특정 모달 오브젝트 생성
    public abstract void InitCompositionByType();
}
