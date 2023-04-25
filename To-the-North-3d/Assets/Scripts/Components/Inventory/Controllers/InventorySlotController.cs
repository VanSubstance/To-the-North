using Assets.Scripts.Items;
using Assets.Scripts.Components.Windows.Inventory;
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
    protected Image slotImage;
    public Transform itemTF;

    [SerializeField]
    private Sprite normal, ready;
    public ContentType ContainerType
    {
        get
        {
            return transform.parent.GetComponent<ContentBaseController>().ContentType;
        }
    }

    protected void Awake()
    {
        Init();
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

    public ItemBaseInfo attachedInfo;

    public void SetLocation(int _r, int _c)
    {
        Init();
        row = _r;
        column = _c;
    }

    private void Init()
    {
        if (slotImage != null) return;
        slotImage = GetComponent<Image>();
        GetComponent<BoxCollider>().size = new Vector3(50, 50, 1);
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