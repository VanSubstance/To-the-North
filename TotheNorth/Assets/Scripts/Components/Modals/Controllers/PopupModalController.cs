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
    // 인풋이 없는 정보 전달용 모달
    public void AwakeModal<T>(ModalType modalType, T contentToInit, string textCancel = "취소", string textConfirm = "확인", System.Action callbackConfirm = null)
    {
        // 모달 초기화
        this.textCancel.text = textCancel;
        this.textConfirm.text = textConfirm;
        setContentByModalType(modalType);
        // 모달 컨텐츠 적용
        popupContentControllerTf.GetComponent<IPopupModalContentController>().InitContent(contentToInit);
        btnConfirm.onClick.AddListener(() =>
        {
            callbackConfirm();
            ToggleModal(false);
            ClearModal();
        });
        ToggleModal(true);
    }
    // 인풋이 있는 모달
    public void AwakeModal<T>(ModalType modalType, string textCancel = "취소", string textConfirm = "확인", System.Func<T, bool> conditionConfirm = null, System.Action<T> callbackConfirm = null)
    {
        // 모달 초기화
        this.textCancel.text = textCancel;
        this.textConfirm.text = textConfirm;
        setContentByModalType(modalType);
        T inputRet;
        // 만약에 해당 타입의 모달이 받아야하는 값이 있다 :
        // 값 받기 ->
        // 성공 여부 조건 확인 ->
        // 완료 콜백에 전달
        btnConfirm.onClick.AddListener(() =>
        {
            if (conditionConfirm != null && !conditionConfirm((inputRet = popupContentControllerTf.GetComponent<IPopupModalContentController>().ReturnValueForCallback<T>())))
            {
                // 조건이 있고, 조건을 통과 못함
                // 실패 ->
                // 에러 메세지 송충
                GlobalComponent.Modal.Info.controller.AddMessageIntoQueue(new InfoStat("조건을 다시 한번 확인하세요!", InfoType.ERROR));
            }
            else
            {
                // 콜백 함수 호출
                // 모달 닫기
                // 모달 초기화
                callbackConfirm(popupContentControllerTf.GetComponent<IPopupModalContentController>().ReturnValueForCallback<T>());
                ToggleModal(false);
                ClearModal();
            }
        });
        ToggleModal(true);
    }
    private void setContentByModalType(ModalType modalType)
    {
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
