using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupModalContentTextController : MonoBehaviour, IPopupModalContentController
{
    private TextMeshProUGUI text;
    private TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        title = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public T ReturnValueForCallback<T>()
    {
        return (T)(object)false;
    }

    public void InitContent<T>(T contentToInit)
    {
        if (text == null)
            text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (typeof(T) == typeof(string))
        {
            text.text = (string)(object)contentToInit;
        }
        return;
    }

    public void InitTitle<T>(T titleToInit)
    {
        if (title == null)
            title = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (typeof(T) == typeof(string))
        {
            title.text = (string)(object)titleToInit;
        }
        return;
    }
}
