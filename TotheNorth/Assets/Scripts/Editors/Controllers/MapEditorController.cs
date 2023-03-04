using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorController : BaseEditorController<MapInfoVO>
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 기물 초기화
    public override void InitComponents()
    {
        Debug.Log("필요한 컴포넌트 생성 작업");
    }

    public override void TryApplyData(MapInfoVO dataToApply)
    {
        Debug.Log("데이터 적용 시도:: " + dataToApply);
    }

    public override void TryAllocateMouseAction()
    {
        Debug.Log("마우스 이벤트 적용");
    }
}
