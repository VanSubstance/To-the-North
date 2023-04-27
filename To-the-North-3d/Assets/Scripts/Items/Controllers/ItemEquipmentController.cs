using Assets.Scripts.Components.Popups;
using UnityEngine;
using Assets.Scripts.Components.Windows.Inventory;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController
    {
        private const string TAG = "장비 아이템:\n";

        private ItemEquipmentInfo Info
        {
            get
            {
                return (ItemEquipmentInfo)info;
            }
        }

        protected override void OnDoubleClick()
        {
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
        }

        protected override void OnMouseEnterWithKeyPress()
        {
        }

        public override void OnItemOnOtherItem(ItemBaseInfo _targetItemInfo)
        {
        }

        public override void OnItemDownWithKeyPress()
        {
            if (info is not ItemMagazineInfo)
            {
                // 장착 가능 장비일 시
                if (Info is ItemArmorInfo)
                {
                    // 방어구
                    switch (((ItemArmorInfo)Info).equipPartType)
                    {
                        case EquipBodyType.Helmat:
                            if (Equip(WindowInventoryController.equipmentCtrl.helmetCtrl)) return;
                            break;
                        case EquipBodyType.Mask:
                            if (Equip(WindowInventoryController.equipmentCtrl.maskCtrl)) return;
                            break;
                        case EquipBodyType.Body:
                            if (Equip(WindowInventoryController.equipmentCtrl.bodyCtrl)) return;
                            break;
                        case EquipBodyType.BackPack:
                            if (Equip(WindowInventoryController.equipmentCtrl.backpackCtrl)) return;
                            break;
                        case EquipBodyType.Right:
                            break;
                        case EquipBodyType.Left:
                            break;
                    }
                    return;
                }
                if (Info is ItemWeaponInfo)
                {
                    // 무기
                    if (!WindowInventoryController.equipmentCtrl.handRCtrl.IsEquipped)
                    {
                        if (((ItemWeaponInfo)Info).handType == EquipHandType.Multiple)
                        {
                            // 양손 무기
                            Equip(WindowInventoryController.equipmentCtrl.handRCtrl);
                        }
                        else
                        {
                            // 한손 무기
                            Equip(WindowInventoryController.equipmentCtrl.handRCtrl);
                        }
                        return;
                    }
                    if (!WindowInventoryController.equipmentCtrl.handLCtrl.IsEquipped)
                    {
                        // 한손 무기
                        Equip(WindowInventoryController.equipmentCtrl.handLCtrl);
                        return;
                    }
                    return;
                }
            }
        }

        private bool Equip(EquipmentSlotController _targetSlot)
        {
            if (!_targetSlot.IsEquipped)
            {
                ItemDetach();
                if (isRotate)
                    ItemRotate();
                InitInfo(info, _targetSlot);
                _targetSlot.EquipItemInfo = Info;
                return true;
            }
            return false;
        }
    }
}