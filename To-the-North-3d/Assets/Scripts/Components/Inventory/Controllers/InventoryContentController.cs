using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using UnityEngine;

public class InventoryContentController : AWindowBaseContentController
{
    public GameObject slotPrefab;
    public Transform itemPrefab;
    public Transform rightGridTF;
    public Transform rightItemTF;
    public Transform leftGridTF;
    public Transform leftItemTF;
    public Transform movingSpaceTF;
    private bool isInit = false;

    private new void Awake()
    {
        base.Awake();
        //GenerateItemObjects();
    }

    /// <summary>
    /// 테스트용 아이템 객체들 생성
    /// </summary>
    private void testItemInit()
    {
        ItemGenerateController tempGenerator;
        foreach (ItemInventoryInfo info in InGameStatus.Item.inventory)
        {
            // 풀링 시스템이 고안되고 나면, 여기서 인스턴시에이트가 아닌 존재하는 오브젝트를 불러와야 함
            tempGenerator = Instantiate(itemPrefab, rightGridTF).GetComponent<ItemGenerateController>();
            tempGenerator.transform.position = new Vector3
            (WindowInventoryController.InventorySlots[info.row, info.col].transform.position.x,
            WindowInventoryController.InventorySlots[info.row, info.col].transform.position.y,
            rightGridTF.transform.position.z);
            //tempGenerator.InitItem(info.itemInfo, info);
        }
    }

    public override void ClearContent()
    {
    }

    protected override void InitComposition()
    {
        if (isInit) return;
        // Transform temp = base.GetContentContainerTf().GetChild(0);
        for (int col = 0; col < 12; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                GameObject tempSlot = Instantiate(slotPrefab, rightGridTF);
                RectTransform slotTransform = tempSlot.GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(row * 60f, col * -60f);
                tempSlot.name = $"InventorySlot({row},{col})";
                WindowInventoryController.InventorySlots[row, col] = tempSlot.GetComponent<InventorySlotController>();
                WindowInventoryController.InventorySlots[row, col].row = row;
                WindowInventoryController.InventorySlots[row, col].column = col;
                WindowInventoryController.InventorySlots[row, col].slotType = SlotType.Inventory;
            }
        }
        for (int col = 0; col < 12; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                GameObject tempSlot = Instantiate(slotPrefab, leftGridTF);
                RectTransform slotTransform = tempSlot.GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(row * 60f, col * -60f);
                tempSlot.name = $"rootSlot({row},{col})";
                WindowInventoryController.LootSlots[row, col] = tempSlot.GetComponent<InventorySlotController>();
                WindowInventoryController.LootSlots[row, col].row = row;
                WindowInventoryController.LootSlots[row, col].column = col;
                WindowInventoryController.LootSlots[row, col].slotType = SlotType.Rooting;
            }
        }
        // 아이템들의 TF를 넣어주고, 이후 아이템 이동 시 사용
        //InventoryManager.leftInventoryTF = leftItemTF;
        //InventoryManager.rightInventoryTF = rightItemTF;
        //InventoryManager.movingSpaceTF = movingSpaceTF;
        testItemInit();
        isInit = true;
    }

    private void OnEnable()
    {
        InGameStatus.User.isPause = true;
    }
    private void OnDisable()
    {
        InGameStatus.User.isPause = false;
    }
}
