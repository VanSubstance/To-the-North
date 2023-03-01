using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorImplementation : IBasicEditor
{
    public void TryAllocateMouseAction()
    {
        Debug.Log("마우스 이벤트 할당");
    }

    public void TryLoadData()
    {
        Debug.Log("데이터 불러오기");
    }

    public void TryResetEditor()
    {
        Debug.Log("에디터 리셋");
    }

    public void TryTurnEditorOff()
    {
        Debug.Log("에디터 끄기");
    }

    public void TryTurnEditorOn()
    {
        Debug.Log("에디터 켜기");
    }
    public void TryAddCustomButtons()
    {
        Debug.Log("커스텀 버튼들 추가하기");
    }

}
