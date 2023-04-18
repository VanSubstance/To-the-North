using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    public int row;
    public int column;
    public bool isAttached = false;
    public bool isAttachReady = false;
    public SlotType slotType;
    public EquipBodyType equipType;
    public Sprite normal;
    public Sprite ready;
    protected Image slotImage;
    public Transform itemTF;
    protected void Start()
    {
        slotImage = GetComponent<Image>();
    }
    protected void Update()
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