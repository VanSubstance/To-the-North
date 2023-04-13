using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController
    {
        private readonly string TAG = "장비 아이템:\n";
        [SerializeField]
        private ItemEquipmentInfo info;

        protected override bool CheckItemTag(InventorySlotController slot)
        {
            if (slot.slotType == SlotType.Inventory || slot.slotType == SlotType.Equipment ||
                slot.slotType == SlotType.Rooting || slot.slotType == SlotType.Shop ||
                slot.slotType == SlotType.Quick)
            {
                return true;
            }
            return false;
        }

        protected override ItemBaseInfo GetBaseInfo()
        {
            return info.GetClone();
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
