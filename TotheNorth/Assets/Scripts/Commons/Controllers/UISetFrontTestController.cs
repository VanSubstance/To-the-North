using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetFrontTestController : MonoBehaviour
{
    public RectTransform frontUITransform;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        frontUITransform.SetAsLastSibling();
    }
}
