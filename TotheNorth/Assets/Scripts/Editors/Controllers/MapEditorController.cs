using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorController : MonoBehaviour, IBasicEditorController<MapInfo>
{
    private MapInfo mapInfo;
    void Start()
    {

    }
    void Update()
    {

    }


    // 인터페이스 구현
    public void TryApplyData(MapInfo dataToApply)
    {
        if (dataToApply == null) return; // 리셋
        // 데이터 적용
    }
    public MapInfo TryExtractData()
    {
        return mapInfo;
    }
    public MapInfo TryLoadData(string targetFilePath)
    {
        Debug.Log("데이터 불러오기:: " + targetFilePath);
        MapInfo info;
        if ((info = DataFunction.LoadObjectFromJson<MapInfo>(targetFilePath)) != null)
        {
            return info;
        }
        return null;
    }
    public void ToggleController(bool isOn)
    {
        gameObject.SetActive(isOn);
    }
    public void TryAddCustomButtons()
    {
        Debug.Log("커스텀 버튼들 추가하기");
    }

    public void TryAllocateMouseAction()
    {
        Debug.Log("마우스 이벤트 할당!");
    }
}
