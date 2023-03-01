using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupModalContentTextController : MonoBehaviour, IPopupModalContentController
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public T ReturnValueForCallback<T>()
    {
        return (T)(object)null;
    }
    public void InitContent(string contentToInit)
    {
        text.text = contentToInit;
    }

    public void InitContent<T>(T contentToInit)
    {
        if (text == null)
            text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (typeof(T) == typeof(string))
        {
            Debug.Log(contentToInit);
            text.text = (string)(object)contentToInit;
        }
        return;
    }
}
