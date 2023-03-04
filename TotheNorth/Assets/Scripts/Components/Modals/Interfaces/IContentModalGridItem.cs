public interface IContentModalGridItem
{
    public void InitContent<T>(T contentToInit);
    public void InstallOnSlot(IContentModalGridSlot targetSlot);

    public void SetCallbackAfterClick(System.Action actionCallback);
}