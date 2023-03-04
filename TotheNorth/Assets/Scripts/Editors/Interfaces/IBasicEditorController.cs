using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBasicEditorController<TReturn>
{
    public void InitEditor();
    public TReturn TryLoadData(string targetFileName);
    public void TryApplyData(TReturn dataToApply);
    public TReturn TryExtractData();
    public void ToggleController(bool isOn);
    public void TryAllocateMouseAction();
}