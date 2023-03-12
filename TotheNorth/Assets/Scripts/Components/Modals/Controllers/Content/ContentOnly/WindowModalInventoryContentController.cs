using UnityEngine;

public class WindowModalInventoryContentController : AWindowModalController<InventorySlot>
{
    private bool isInit = false;
    private InventorySlot[,] slots = new InventorySlot[10, 13];
    public sealed override void ClearContent()
    {
    }

    public sealed override void InitCompositionByType()
    {
        if (isInit) return;
        Transform temp = base.GetContentContainerTf().GetChild(0);
        for (int col = 0; col < 13; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                slots[row, col] = temp.GetChild((col * 10) + row).GetComponent<InventorySlot>();
                slots[row, col].row = row;
                slots[row, col].column = col;
            }
        }
        isInit = true;
    }

    public sealed override void InitContentByType(InventorySlot contentToInit)
    {
        if (isInit == false) InitCompositionByType();
        ClearContent();
    }
}
