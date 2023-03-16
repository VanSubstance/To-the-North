using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    // public Image slotImage;
    public int row;
    public int column;
    public bool isAttached;
    public bool isAttachReady;
    private void Start()
    {
        isAttached = false;
        isAttachReady = false;
    }
}