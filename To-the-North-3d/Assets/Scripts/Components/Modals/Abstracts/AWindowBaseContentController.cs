using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AWindowBaseContentController : MonoBehaviourControllByKey, IWindowModalContentController
{
    [SerializeField]
    private Vector2 sizeToUnit;

    protected void Awake()
    {
        InitComposition();
        ControllByKey(2);
    }
    /// <summary>
    /// 컨텐츠 비우기
    /// </summary>
    public abstract void ClearContent();

    /// <summary>
    /// 최초에 사용하는 컨텐츠 이니셜라이징 함수
    /// </summary>
    protected abstract void InitComposition();
}
