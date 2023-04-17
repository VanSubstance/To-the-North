using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        private const string TAG = "장비 아이템:\n";

        protected override bool CheckItemTag(InventorySlotController slot, bool isGridOn)
        {
            switch (slot.slotType)
            {
                case SlotType.Inventory:
                case SlotType.Rooting:
                case SlotType.Shop:
                    return true;

                case SlotType.Quick when !isGridOn:
                    return true;

                case SlotType.Equipment when info is ItemWeaponInfo:
                    if (!isGridOn && (slot.equipType == EquipBodyType.Left || slot.equipType == EquipBodyType.Right))
                        return true;
                    break;

                case SlotType.Equipment when info is ItemArmorInfo:
                    if (!isGridOn && slot.equipType == EquipBodyType.Body)
                        return true;
                    break;
            }
            return false;
        }

        protected override void OnDoubleClick()
        {
            Debug.Log($"{TAG}더블클릭!");
        }

        protected override void OnHover()
        {
            HoverItemInfoContainerController.Instance.OnHoverEnter(info);
        }

        protected override void OnHoverExit()
        {
            HoverItemInfoContainerController.Instance.OnHoverExit();
        }

        protected override void OnMouseClickWithKeyPress()
        {
            Debug.Log($"{TAG}키 누른 상태로 클릭");
        }

        protected override void OnMouseEnterWithKeyPress()
        {
            Debug.Log($"{TAG}키 누른 상태로 진입");
        }
    }
}