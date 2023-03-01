using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEditorManager : MonoBehaviour
{
    [SerializeField]
    private Button btnMainStartNew, btnMainStartLoad, btnMainExit;
    [SerializeField]
    private Transform TfMainMenu;
    [SerializeField]
    private EditorType editorType;
    private IBasicEditor editorFunctions;
    private int curStatus = 0;
    // Start is called before the first frame update
    void Start()
    {
        switch (editorType)
        {
            case EditorType.MAP:
                editorFunctions = new MapEditorImplementation();
                break;
        }
        btnMainStartNew.onClick.AddListener(() =>
        {
            StartGame(true);
        });
        btnMainStartLoad.onClick.AddListener(() =>
        {
            StartGame(false);
        });
        btnMainExit.onClick.AddListener(() =>
        {
            ExitGame();
        });
    }

    // Update is called once per frame
    void Update()
    {
        switch (curStatus)
        {
            case 0:
                // 로딩 대기중
                break;
            case 1:
                // 메인 메뉴
                break;
            case 2:
                // 에디팅
                break;
        }
    }

    private void ToggleMainMenuUI(bool isHide)
    {

    }

    public void StartGame(bool isGameNew)
    {
        if (isGameNew)
        {
            editorFunctions.TryAllocateMouseAction();
            ToggleMainMenuUI(true);
            editorFunctions.TryTurnEditorOn();
            editorFunctions.TryResetEditor();
            editorFunctions.TryAddCustomButtons();
            curStatus = 1;
        }
        else
        {
            // 파일 이름 받는 모달 필요
            //GlobalComponent.modalPopup.setType(ModalType.TextInput);
            //GlobalComponent.modalPopup.setInfo(textConfirm: "불러오기");
            //GlobalComponent.modalPopup.openModal(onConfirm: (inputText) => {
            //    TryAllocateMouseAction();
            //    ToggleMainMenuUI(true);
            //    TryTurnEditorOn();
            //    TryResetEditor();
            //    TryAddCustomButtons();
            //    TryLoadData();
            //    curStatus = 1;
            //};
        }
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
