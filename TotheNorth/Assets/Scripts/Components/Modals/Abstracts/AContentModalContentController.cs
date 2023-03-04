using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AContentModalController<TContent> : AIContentModalContentController
{
    // 컨텐츠 적용하기
    public abstract void InitContentByType(TContent contentToInit);
}
