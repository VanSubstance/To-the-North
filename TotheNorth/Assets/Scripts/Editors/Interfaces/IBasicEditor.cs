public interface IBasicEditor
{
    public void TryAllocateMouseAction();
    public void TryTurnEditorOn();
    public void TryTurnEditorOff();
    public void TryResetEditor();
    public void TryLoadData(string targetFileName);
    public void TryAddCustomButtons();
}