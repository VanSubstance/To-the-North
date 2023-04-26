using Assets.Scripts.Items;
using Assets.Scripts.Components.Windows.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    public int row;
    public int column;
    public SlotType slotType;
    protected Image slotImage;

    /// <summary>
    /// ������ ������Ʈ
    /// �������� ��Ȯ�� �ش� ���Կ� �����Ǿ����� ��: ������ Transform�� ����
    /// �������� �ٸ� ���Կ� ��Ź�Ǿ��ִµ� �ش� ������ ���� ��: ��Ȯ�� �����Ǿ��ִ� ������ Transform�� ���� 
    /// </summary>
    private Transform itemTf;

    /// <summary>
    /// ���� �ش� ���Կ� �����Ǿ��ִ�/���� ������ ������Ʈ ��ȯ
    /// ���� ��: null
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
            return ItemTf.GetComponent<AItemBaseController<object>>().baseInfo;
        }
    }

    [SerializeField]
    private Sprite normal, ready;
    public ContentType ContainerType
    {
        get
        {
            ContentBaseController t;
            if ((t = transform.parent.GetComponent<ContentBaseController>()) != null)
            {
                return transform.parent.GetComponent<ContentBaseController>().ContentType;
            }
            if ((t = transform.parent.parent.GetComponent<ContentBaseController>()) != null)
            {
                return transform.parent.parent.GetComponent<ContentBaseController>().ContentType;
            }
            return ContentType.Undefined;
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
    /// ���� �����̳ʿ��� �ش� ĭ�� row, column ���� �����ϴ� �Լ�
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
    /// �ʱ�ȭ �Լ�
    /// </summary>
    private void Init()
    {
        if (slotImage != null) return;
        itemTf = null;
        slotImage = GetComponent<Image>();
        slotImage.sprite = normal;
        if (transform.parent.GetComponent<GridLayoutGroup>())
        {
            GetComponent<BoxCollider>().size = new Vector3(50, 50, 1);
        } else
        {
            Vector2 t = GetComponent<RectTransform>().sizeDelta;
            GetComponent<BoxCollider>().size = new Vector3(t.x, t.y, 1);
        }
        IsConsidered = false;
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