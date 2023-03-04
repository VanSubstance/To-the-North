using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupModalController : MonoBehaviour
{
    [SerializeField]
    private Button btnCancel, btnConfirm;
    [SerializeField]
    private TextMeshProUGUI textCancel, textConfirm;
    [SerializeField]
    private Transform popupContentContainerTf;
    private Transform popupContentControllerTf;
    // Start is called before the first frame update
    void Start()
    {
        btnCancel.onClick.AddListener(() =>
        {
            ToggleModal(false);
            ClearModal();
        });
        GlobalComponent.Modal.Popup.controller = this;
        GlobalStatus.Loading.System.PopupModalController = true;
        ToggleModal(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearModal()
    {
        textCancel.text = string.Empty;
        textConfirm.text = string.Empty;
        btnConfirm.onClick.RemoveAllListeners();
        Destroy(popupContentControllerTf.gameObject);
    }
    public void AwakeModal<TContent, TReturn>(
        ModalType modalType,
        TContent contentToInit,
        string textCancel = "취소", string textConfirm = "확인",
        System.Func<TReturn, bool> conditionConfirm = null,
        System.Action<TReturn> callbackConfirm = null
        )
    {
        // 모달 초기화
        this.textCancel.text = textCancel;
        this.textConfirm.text = textConfirm;
        SetContentByModalType(modalType);
        popupContentControllerTf.GetComponent<IPopupModalContentController>().InitContent(contentToInit);
        TReturn inputRet;
        // 만약에 해당 타입의 모달이 받아야하는 값이 있다 :
        // 값 받기 ->
        // 성공 여부 조건 확인 ->
        // 완료 콜백에 전달
        btnConfirm.onClick.AddListener(() =>
        {
            if (conditionConfirm != null && !conditionConfirm((inputRet = popupContentControllerTf.GetComponent<IPopupModalContentController>().ReturnValueForCallback<TReturn>())))
            {
                // 조건이 있고, 조건을 통과 못함
                // 실패 ->
                // 에러 메세지 송충
                GlobalComponent.Common.Info.controller.AddMessageIntoQueue(new InfoStat("조건을 다시 한번 확인하세요!", InfoType.ERROR));
            }
            else
            {
                // 콜백 함수 호출
                // 모달 닫기
                // 모달 초기화
                callbackConfirm(popupContentControllerTf.GetComponent<IPopupModalContentController>().ReturnValueForCallback<TReturn>());
                ToggleModal(false);
                ClearModal();
            }
        });
        ToggleModal(true);
    }
    private void SetContentByModalType(ModalType modalType)
    {
        // 모달 컨텐츠 컴포넌트 생성
        popupContentControllerTf = Instantiate(GlobalComponent.Modal.Popup.contentPrefabs[modalType]);
        popupContentControllerTf.SetParent(popupContentContainerTf);
        popupContentControllerTf.localPosition = Vector3.zero;
        popupContentControllerTf.localScale = Vector3.one;
        popupContentControllerTf.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        popupContentControllerTf.GetComponent<RectTransform>().offsetMax = Vector2.one;
    }
    private void ToggleModal(bool isOpen = true)
    {
        gameObject.SetActive(isOpen);
    }
}
