using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class QuickSlotController : InventorySlotController
    {
        private ItemConsumableInfo Info
        {
            get
            {
                return (ItemConsumableInfo)AttachedInfo;
            }
        }
        public bool IsEquipped
        {
            get
            {
                return Info != null;
            }
        }

        public void Use()
        {
            if (Info == null) return;
            CommonGameManager.Instance.ApplyConsumable(Info);
        }
    }
}
