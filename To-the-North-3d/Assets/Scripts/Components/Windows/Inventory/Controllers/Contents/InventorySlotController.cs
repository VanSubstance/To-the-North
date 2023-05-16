using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    public int row;
    public int column;
    protected Image slotImage;

    /// <summary>
    /// 아이템 오브젝트
    /// 아이템이 정확히 해당 슬롯에 부착되어있을 때: 아이템 Transform을 보관
    /// 아이템이 다른 슬롯에 부탁되어있는데 해당 슬롯을 덮을 때: 정확히 부착되어있는 슬롯의 Transform을 보관 
    /// </summary>
    private Transform itemTf;

    /// <summary>
    /// 현재 해당 슬롯에 부착되어있는/덮는 아이템 오브젝트 반환
    /// 없을 시: null
    /// </summary>
    public Transform ItemTf
    {
        get
        {
            if (itemTf == null) return null;
            if (itemTf.GetComponent<AbsItemController>())
            {
                return itemTf;
            }
            return null;
        }
        set
        {
            itemTf = value;
            if (itemTf != null)
            {
                slotImage.sprite = ready;
            }
            else
            {
                slotImage.sprite = normal;
            }
        }
    }

    public ItemBaseInfo AttachedInfo
    {
        get
        {
            if (ItemTf == null) return null;
            return ItemTf.GetComponent<AItemBaseController>().info;
        }
    }

    [SerializeField]
    private Sprite normal, ready;
    public ContentType ContainerType
    {
        get
        {
            if (ContentBase != null) return ContentBase.ContentType;
            return ContentType.Undefined;
        }
    }

    public ContentBaseController ContentBase
    {
        get
        {
            if ((transform.parent.GetComponent<ContentBaseController>()) != null)
            {
                return transform.parent.GetComponent<ContentBaseController>();
            }
            if ((transform.parent.parent.GetComponent<ContentBaseController>()) != null)
            {
                return transform.parent.parent.GetComponent<ContentBaseController>();
            }
            return null;
        }
    }

    private bool isConsidered;
    public bool IsConsidered
    {
        set
        {
            isConsidered = value;
            if (isConsidered)
            {
                slotImage.sprite = ready;
            }
            else
            {
                if (itemTf == null)
                {
                    slotImage.sprite = normal;
                }
            }
        }
        get
        {
            return isConsidered;
        }
    }

    protected void Awake()
    {
        Init();
    }
    protected void Update()
    {
    }

    /// <summary>
    /// 격자 컨테이너에서 해당 칸의 row, column 값을 선언하는 함수
    /// </summary>
    /// <param name="_r">row</param>
    /// <param name="_c">column</param>
    public void SetLocation(int _r, int _c)
    {
        Init();
        row = _r;
        column = _c;
    }

    /// <summary>
    /// 초기화 함수
    /// </summary>
    private void Init()
    {
        if (slotImage != null) return;
        itemTf = null;
        slotImage = GetComponent<Image>();
        slotImage.sprite = normal;
        if (transform.parent.GetComponent<GridLayoutGroup>())
        {
            if (transform.CompareTag("HotKeySlot"))
            {
                GetComponent<BoxCollider>().size = new Vector3(100, 100, 1);
            } else
            {
                GetComponent<BoxCollider>().size = new Vector3(50, 50, 1);
            }
        } else
        {
            Vector2 t = GetComponent<RectTransform>().sizeDelta;
            GetComponent<BoxCollider>().size = new Vector3(t.x, t.y, 1);
        }
        IsConsidered = false;
    }

    public bool EnrollItemToSlot()
    {
        if (ContainerType.Equals(ContentType.Looting) || ContainerType.Equals(ContentType.Inventory))
        {
            ((ContentSlotController)ContentBase).itemsAttached.Add(AttachedInfo.InvenInfo);
            return true;
        }
        return false;
    }

    public bool DetachItemFromSlot()
    {
        if (ContainerType.Equals(ContentType.Looting) || ContainerType.Equals(ContentType.Inventory))
        {
            ((ContentSlotController)ContentBase).itemsAttached.Remove(AttachedInfo.InvenInfo);
            return true;
        }
        return false;
    }
}