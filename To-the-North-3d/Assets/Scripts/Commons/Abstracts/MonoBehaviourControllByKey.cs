using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MonoBehaviourControllByKey : MonoBehaviour, IControllByKey
{
    /// <summary>
    /// 열기
    /// </summary>
    public void Open()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 닫기
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ControllByKey(int purpose)
    {
        bool toOpen = purpose == 1 ? true : purpose == 2 ? false :
            gameObject.activeSelf ? false : true;
        ;
        transform.SetAsLastSibling();
        gameObject.SetActive(toOpen);
    }
}
