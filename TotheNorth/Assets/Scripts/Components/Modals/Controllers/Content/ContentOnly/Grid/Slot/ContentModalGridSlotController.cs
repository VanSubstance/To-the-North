using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentModalGridSlotController : MonoBehaviour, IContentModalGridSlot
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ClearContent()
    {
        Debug.Log("해당 슬롯에 설치된 객체 삭제하기");
    }

}
