using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupModalContentInputTextController : MonoBehaviour, IPopupModalContentController
{
    private TMP_InputField textInput;
    // Start is called before the first frame update
    void Start()
    {
        textInput = GetComponent<TMP_InputField>();
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
        return (T)(object)null;
    }

    public void InitContent<T>(T contentToInit)
    {
        return;
    }
}
