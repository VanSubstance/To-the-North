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
            GlobalComponent.Modal.Popup.controller.AwakeModal(
                ModalType.INFO_NORMAL,
                contentToInit: "새로운 맵을 생성하시겠습니까??",
                callbackConfirm: () =>
                {
                    editorFunctions.TryAllocateMouseAction();
                    ToggleMainMenuUI(true);
                    editorFunctions.TryTurnEditorOn();
                    editorFunctions.TryResetEditor();
                    editorFunctions.TryAddCustomButtons();
                    curStatus = 1;
                }
            );
        }
        else
        {
            GlobalComponent.Modal.Popup.controller.AwakeModal<string>(
                ModalType.INPUT_TEXT,
                textConfirm: "불러오기",
                conditionConfirm: (inputText) =>
                {
                    return inputText.Length > 0;
                },
                callbackConfirm: (inputText) =>
                {
                    editorFunctions.TryAllocateMouseAction();
                    ToggleMainMenuUI(true);
                    editorFunctions.TryTurnEditorOn();
                    editorFunctions.TryResetEditor();
                    editorFunctions.TryAddCustomButtons();
                    editorFunctions.TryLoadData(inputText);
                    curStatus = 1;
                }
            );
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
