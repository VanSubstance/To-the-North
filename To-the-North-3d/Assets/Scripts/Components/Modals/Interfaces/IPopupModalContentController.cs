public interface IPopupModalContentController
{
    public T ReturnValueForCallback<T>();
    public void InitContent<T>(T contentToInit);
}