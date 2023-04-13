using UnityEngine;
using static GlobalComponent.Common;

namespace Assets.Scripts.Items
{
    public class ItemMaterialController : AItemBaseController<ItemMaterialInfo>
    {
        private readonly string TAG = "재료 아이템:\n";

        protected override bool CheckItemTag(InventorySlotController slot)
        {
            if (slot.slotType == SlotType.Inventory || slot.slotType == SlotType.Rooting ||
                slot.slotType == SlotType.Ground || slot.slotType == SlotType.Shop)
            {
                return true;
            }
            return false;
        }

        protected override void OnDoubleClick()
        {
            Debug.unityLogger.Log(TAG, "더블클릭!");
        }

        protected override void OnHover()
        {
            Debug.unityLogger.Log(TAG, "호버링");
        }

        protected override void OnMouseClickWithKeyPress()
        {
            Debug.unityLogger.Log(TAG, "키 누른 상태로 클릭");
        }

        protected override void OnMouseEnterWithKeyPress()
        {
            Debug.unityLogger.Log(TAG, "키 누른 상태로 진입");
        }
    }
}
