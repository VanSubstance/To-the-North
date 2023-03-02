using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorManager : BasicEditorManager
{
    [SerializeField]
    private MapEditorController mapEditorController;
    public override void StartGame(bool isGameNew)
    {
        if (isGameNew)
        {
            GlobalComponent.Modal.Popup.controller.AwakeModal<string, bool>(
                ModalType.INFO_NORMAL,
                contentToInit: "새로운 맵을 생성하시겠습니까??",
                callbackConfirm: (allwaysTrue) =>
                {
                    mapEditorController.TryAllocateMouseAction();
                    ToggleMainMenuUI(true);
                    mapEditorController.ToggleController(true);
                    mapEditorController.TryApplyData(null);
                    mapEditorController.TryAddCustomButtons();
                }
            );
        }
        else
        {
            GlobalComponent.Modal.Popup.controller.AwakeModal<bool, string>(
                ModalType.INPUT_TEXT,
                contentToInit: false,
                textConfirm: "불러오기",
                conditionConfirm: (inputText) =>
                {
                    return inputText.Length > 0;
                },
                callbackConfirm: (inputText) =>
                {
                    mapEditorController.TryAllocateMouseAction();
                    ToggleMainMenuUI(true);
                    mapEditorController.ToggleController(true);
                    mapEditorController.TryAddCustomButtons();
                    mapEditorController.TryApplyData(mapEditorController.TryLoadData(inputText));
                }
            );
        }
    }
}
