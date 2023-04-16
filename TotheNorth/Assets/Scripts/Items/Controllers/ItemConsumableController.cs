using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableController : AItemBaseController<ItemConsumableInfo>
    {
        private readonly string TAG = "소모성 아이템:\n";

        protected override bool CheckItemTag(InventorySlotController slot, bool isGridOn)
        {
            switch (slot.slotType)
            {
                case SlotType.Inventory:
                case SlotType.Rooting:
                case SlotType.Shop:
                case SlotType.Ground:
                    return true;

                case SlotType.Quick when !isGridOn:
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
