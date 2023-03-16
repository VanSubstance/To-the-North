public interface IWindowModalGridItem
{
    public void InitContent<T>(T contentToInit);
    public void InstallOnSlot(IWindowModalGridSlot targetSlot);
    public void SetCallbackAfterClick(System.Action actionCallback);
}