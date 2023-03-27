using UnityEngine;
using TMPro;

public class WindowModalContentTestController : AWindowModalController<ModalContentTestStat>
{
    private TextMeshProUGUI ugui01, ugui02, ugui03;
    private bool isInit = false;
    public sealed override void ClearContent()
    {
        ugui01.text = string.Empty;
        ugui02.text = string.Empty;
        ugui03.text = string.Empty;
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        Transform temp = base.GetContentContainerTf();
        ugui01 = temp.GetChild(0).GetComponent<TextMeshProUGUI>();
        ugui02 = temp.GetChild(1).GetComponent<TextMeshProUGUI>();
        ugui03 = temp.GetChild(2).GetComponent<TextMeshProUGUI>();
        isInit = true;
    }

    public sealed override void InitContentByType(ModalContentTestStat contentToInit)
    {
        if (ugui01 == null) InitCompositionByType();
        ClearContent();
        ugui01.text = contentToInit.text01;
        ugui02.text = contentToInit.text02;
        ugui03.text = contentToInit.text03;
    }
}
