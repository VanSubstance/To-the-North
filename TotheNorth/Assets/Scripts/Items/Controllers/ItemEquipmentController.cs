using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        private readonly string TAG = "장비 아이템:\n";

        protected override bool CheckItemTag(InventorySlotController slot, bool isGridOn)
        {
            if (slot.slotType == SlotType.Inventory ||
                slot.slotType == SlotType.Rooting ||
                slot.slotType == SlotType.Shop ||
                slot.slotType == SlotType.Quick && isGridOn == false)
            {
                return true;
            }
            if (info is ItemWeaponInfo)
            {
                if (slot.slotType == SlotType.Equipment && isGridOn == false &&
                    slot.equipType == EquipBodyType.Left ||
                    slot.slotType == SlotType.Equipment && isGridOn == false &&
                    slot.equipType == EquipBodyType.Right)
                {
                    return true;
                }
            }
            if (info is ItemArmorInfo)
            {
                if (slot.slotType == SlotType.Equipment && isGridOn == false &&
                    slot.equipType == EquipBodyType.Body)
                {
                    return true;
                }
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
