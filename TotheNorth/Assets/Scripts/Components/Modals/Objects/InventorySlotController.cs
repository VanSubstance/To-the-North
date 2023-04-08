using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    public int row;
    public int column;
    public bool isAttached = false;
    public bool isAttachReady = false;
    public SlotType slotType;
    public EquipType equipType;
    public Sprite normal;
    public Sprite ready;
    private Image slotImage;
    private void Start()
    {
        slotImage = GetComponent<Image>();
    }
    private void Update()
    {
        if (isAttachReady)
        {
            slotImage.sprite = ready;
        }
        else
        {
            slotImage.sprite = normal;
        }
    }
}

public enum SlotType
{
    Inventory,
    Rooting,
    Equipment,
    Shop,
    Quick,
    Ground
}

public enum EquipType
{
    Head,
    Mask,
    Body,
    Leg,
    Right,
    Left,
    Back,
}