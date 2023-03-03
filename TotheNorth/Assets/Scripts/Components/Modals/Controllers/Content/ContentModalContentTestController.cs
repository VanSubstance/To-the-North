using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentModalContentTestController : AContentModalController<string>
{
    public sealed override void clearContent()
    {
        Debug.Log("비우기이이");
    }

    public sealed override void InitContentByType(string contentToInit)
    {
        Debug.Log("컨텐츠 적용:: " + contentToInit);
    }
}
