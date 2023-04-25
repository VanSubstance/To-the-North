using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentSlotController : ContentBaseController
    {
        [SerializeField]
        private InventorySlotController slotTf;
        private readonly Vector2 sizeSlot = new Vector2(10, 14);
        protected void Awake()
        {
            for (int i = 0; i < sizeSlot.y; i++)
            {
                for (int j = 0; j < sizeSlot.x; j++)
                {
                    Instantiate(slotTf, transform).SetLocation(i, j);
                }
            }
        }
    }
}
