using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEditorController<TReturn> : MonoBehaviour, IBasicEditorController<TReturn>
{
    private TReturn infoVO;
    // 인터페이스 구현
    public TReturn TryExtractData()
    {
        return infoVO;
    }
    public TReturn TryLoadData(string targetFilePath)
    {
        Debug.Log("데이터 불러오기:: " + targetFilePath);
        TReturn info;
        if ((info = DataFunction.LoadObjectFromJson<TReturn>(targetFilePath)) != null)
        {
            return info;
        }
        return (TReturn)(object)null;
    }
    public void ToggleController(bool isOn)
    {
        gameObject.SetActive(isOn);
    }

    public void InitEditor()
    {
        // 마우스 이벤트 부여
        TryAllocateMouseAction();
        // 해당 에디터 화면 켜기
        ToggleController(true);

        // 기물 초기화
        InitComponents();
    }

    // 기물 초기화
    public abstract void InitComponents();
    // 데이터 적용
    public abstract void TryApplyData(TReturn dataToApply);
    // 마우스 이벤트 적용
    public abstract void TryAllocateMouseAction();
}
