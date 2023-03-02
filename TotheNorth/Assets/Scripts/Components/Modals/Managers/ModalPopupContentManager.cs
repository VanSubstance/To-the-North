using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalPopupContentManager : MonoBehaviour
{
    [SerializeField]
    Transform popUpModalControllerPrefab, popupModalContentInputText, popupModalContentText;
    // Start is called before the first frame update
    void Start()
    {
        setModalContentControllersIntoGlobal();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setModalContentControllersIntoGlobal()
    {
        Transform popupModal = Instantiate(popUpModalControllerPrefab);
        popupModal.SetParent(GameObject.FindWithTag("UI Container").transform);
        popupModal.localPosition = Vector3.zero;
        popupModal.localScale = Vector3.one;
        popupModal.GetComponent<RectTransform>().sizeDelta = new Vector2(640f, 320f);
        GlobalComponent.Modal.Popup.contentPrefabs[ModalType.INFO_NORMAL] = popupModalContentText;
        GlobalComponent.Modal.Popup.contentPrefabs[ModalType.INPUT_TEXT] = popupModalContentInputText;
        GlobalStatus.Loading.System.PopupModalContentControllers = true;
    }
}
