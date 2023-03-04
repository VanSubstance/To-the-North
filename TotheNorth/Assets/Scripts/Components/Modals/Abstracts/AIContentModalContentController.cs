using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIContentModalContentController : MonoBehaviour, IContentModalContentController
{
    // 컨텐츠 비우기
    public abstract void ClearContent();

    public abstract void Toggle(int purpose);
}
