using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModalPopupContentManager : MonoBehaviour
{
    [SerializeField]
    Transform popupModalContentInputText, popupModalContentText;
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
        GlobalComponent.Modal.Popup.contentPrefabs[ModalType.INFO_NORMAL] = popupModalContentText;
        GlobalComponent.Modal.Popup.contentPrefabs[ModalType.INPUT_TEXT] = popupModalContentInputText;
        GlobalStatus.Loading.System.PopupModalContentControllers = true;
    }
}
