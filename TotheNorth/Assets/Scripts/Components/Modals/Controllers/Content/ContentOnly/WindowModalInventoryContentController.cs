using UnityEngine;
using TMPro;

public class WindowModalInventoryContentController : AWindowModalController<ModalContentTestStat>
{
    private bool isInit = false;
    public sealed override void ClearContent()
    {
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        Transform temp = base.GetContentContainerTf();
        isInit = true;
    }

    public sealed override void InitContentByType(ModalContentTestStat contentToInit)
    {
        ClearContent();
    }
}
