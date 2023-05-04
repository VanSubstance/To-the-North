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

        public override bool OnItemOnOtherItem(ItemBaseInfo _targetItemInfo)
        {
            return true;
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
                    }
                    return;
                }
                if (Info is ItemWeaponInfo)
                {
                    // 무기
                    // 주 무기 슬롯부터 비어있는지 확인
                    if (!WindowInventoryController.equipmentCtrl.weapon1Ctrl.IsEquipped)
                    {
                        // 주무기가 비었다 -> 주무기 장착 + 주무기 손에 들기
                        Equip(WindowInventoryController.equipmentCtrl.weapon1Ctrl);
                        return;
                    }
                    if (!WindowInventoryController.equipmentCtrl.weapon2Ctrl.IsEquipped)
                    {
                        // 부 무기가 비엇다 -> 부무기 장착 + 주무기 손에 들기
                        Equip(WindowInventoryController.equipmentCtrl.weapon2Ctrl);
                        return;
                    }
                    // 둘 다 안비었다 -> 패스
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
                {
                    ItemRotate(true);
                }
                InitInfo(info, _targetSlot);
                _targetSlot.EquipItemInfo = Info;
                return true;
            }
            return false;
        }
    }
}