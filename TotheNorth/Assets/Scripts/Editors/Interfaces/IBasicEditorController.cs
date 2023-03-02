using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicEditorController<TReturn>
{
    public TReturn TryLoadData(string targetFileName);
    public void TryApplyData(TReturn dataToApply);
    public TReturn TryExtractData();
    public void ToggleController(bool isOn);
    public void TryAddCustomButtons();
    public void TryAllocateMouseAction();
}