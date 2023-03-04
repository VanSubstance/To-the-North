using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupModalContentInputTextController : MonoBehaviour, IPopupModalContentController
{
    private TMP_InputField textInput;
    private TextMeshProUGUI title;
    // Start is called before the first frame update
    void Start()
    {
        textInput = GetComponent<TMP_InputField>();
        title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public T ReturnValueForCallback<T>()
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)textInput.text;
        }
        return (T)(object)false;
    }

    public void InitContent<T>(T contentToInit)
    {
        return;
    }
    public void InitTitle<T>(T titleToInit)
    {
        if (title == null)
            title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (typeof(T) == typeof(string))
        {
            title.text = (string)(object)titleToInit;
        }
        return;
    }
}
