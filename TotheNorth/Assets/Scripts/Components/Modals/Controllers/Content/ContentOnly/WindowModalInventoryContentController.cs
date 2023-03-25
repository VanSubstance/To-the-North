using Unity.VisualScripting;
using UnityEngine;

public class WindowModalInventoryContentController : AWindowModalController<InventorySlotController>
{
    public GameObject slotPrefab;
    public GameObject itemPrefabSmall;
    public GameObject itemPrefabBig;
    public Transform slotParentTF;
    public Transform itemParentTF;
    private bool isInit = false;
    public sealed override void ClearContent()
    {
    }

    public void testItemInit()
    {
        GameObject item1 = Instantiate(itemPrefabSmall, itemParentTF);
        item1.transform.position = new Vector3
            (InventoryManager.inventorySlots[0, 0].transform.position.x,
            InventoryManager.inventorySlots[0, 0].transform.position.y,
            itemParentTF.transform.position.z);
        GameObject item2 = Instantiate(itemPrefabSmall, itemParentTF);
        item2.transform.position = new Vector3
            (InventoryManager.inventorySlots[5, 3].transform.position.x,
            InventoryManager.inventorySlots[5, 3].transform.position.y,
            itemParentTF.transform.position.z);
        GameObject item3 = Instantiate(itemPrefabBig, itemParentTF);
        item3.transform.position = new Vector3
            (InventoryManager.inventorySlots[8, 8].transform.position.x,
            InventoryManager.inventorySlots[8, 8].transform.position.y,
            itemParentTF.transform.position.z);
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        // Transform temp = base.GetContentContainerTf().GetChild(0);
        for (int col = 0; col < 13; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                GameObject tempSlot = Instantiate(slotPrefab, slotParentTF);
                RectTransform slotTransform = tempSlot.GetComponent<RectTransform>();
                slotTransform.anchoredPosition = new Vector2(row * 60f, col * -60f);
                tempSlot.name = "InventorySlot(" + row + "," + col + ")";
                InventoryManager.inventorySlots[row, col] = tempSlot.GetComponent<InventorySlotController>();
                InventoryManager.inventorySlots[row, col].row = row;
                InventoryManager.inventorySlots[row, col].column = col;
            }
        }
        testItemInit();
        isInit = true;
    }

    public sealed override void InitContentByType(InventorySlotController contentToInit)
    {
        if (isInit == false) InitCompositionByType();
        ClearContent();
    }
}
