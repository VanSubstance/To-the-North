using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private ContentModalContentTestController tes;
    void Start()
    {
        testInitContentModal();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void testInitContentModal()
    {
        // 컨텐츠 팝업 초기화 테스트
        tes.InitContentByType(new ModalContentTestStat("테스트", "중입니다", "되는지 보죠"));
    }

    private void TestMouseEventSetting()
    {
        GlobalStatus.Util.MouseEvent.Left.setActions(
            actionDown: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 왼쪽 - 다운");
            },
            actionDrag: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 왼쪽 - 드래그");
            },
            actionUp: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 왼쪽 - 업");
            },
            actionClick: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 왼쪽 - 클릭");
            }
            );
        GlobalStatus.Util.MouseEvent.Right.setActions(
            actionDown: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 오른쪽 - 다운");
            },
            actionDrag: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 오른쪽 - 드래그");
            },
            actionUp: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 오른쪽 - 업");
            },
            actionClick: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 오른쪽 - 클릭");
            }
            );
        GlobalStatus.Util.MouseEvent.Middle.setActions(
            actionDown: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 드래그 - 다운");
            },
            actionDrag: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 드래그 - 드래그");
            },
            actionUp: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 드래그 - 업");
            },
            actionClick: (targetTf, curM) =>
            {
                Debug.Log("마우스 - 드래그 - 클릭");
            }
            );
    }
}
