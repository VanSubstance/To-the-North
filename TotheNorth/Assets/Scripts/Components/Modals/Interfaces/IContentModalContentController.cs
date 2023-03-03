using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContentModalContentController
{
    public void clearContent();
    void InitContent<T>(T contentToInit);
}