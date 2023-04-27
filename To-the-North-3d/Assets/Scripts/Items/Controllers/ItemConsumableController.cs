using Assets.Scripts.Components.Infos;
using Assets.Scripts.Components.Popups;
using Assets.Scripts.Components.Windows.Inventory;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableController : AItemBaseController
    {
        private readonly string TAG = "소모성 아이템:\n";

        private ItemConsumableInfo Info
        {
            get
            {
                return (ItemConsumableInfo)info;
            }
        }

        private void ReplaceWithQuickSlot(QuickSlotController qSlot)
        {
            if (qSlot.IsEquipped)
            {
                AItemBaseController b = qSlot.AttachedInfo.Ctrl;
                // 기존 퀵슬롯 아이템 인벤토리로 보내기
                b.ItemDetach();
                b.SendToInventory();
            }
            // 아이템 퀵슬롯에 장착
            ItemDetach();
            ItemAttach(qSlot);
        }

        protected override void OnDoubleClick()
        {
            CommonGameManager.Instance.ApplyConsumable(Info);
        }

        protected override void OnHover()
        {
            HoverItemInfoContainerController.Instance.OnHoverEnter(info);
            if (Info.consumableType == ConsumbableType.Bullet) return;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    ReplaceWithQuickSlot(UIQuickController.Instance.Quicks[0]);
                    HoverItemInfoContainerController.Instance.OnHoverExit();
                    return;
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    ReplaceWithQuickSlot(UIQuickController.Instance.Quicks[1]);
                    HoverItemInfoContainerController.Instance.OnHoverExit();
                    return;
                }
                if (Input.GetKey(KeyCode.Alpha3))
                {
                    ReplaceWithQuickSlot(UIQuickController.Instance.Quicks[2]);
                    HoverItemInfoContainerController.Instance.OnHoverExit();
                    return;
                }
            }
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
            switch (Info.consumableType)
            {
                case ConsumbableType.Bullet:
                    if (_targetItemInfo is ItemMagazineInfo mgInfo)
                    {
                        // 탄환 -> 탄창
                        mgInfo.LoadMagazine((ItemBulletInfo)Info);
                    }
                    return true;
                case ConsumbableType.Food:
                case ConsumbableType.Medicine:
                    if (_targetItemInfo.Ctrl.CurSlot is QuickSlotController qSlot)
                    {
                        ReplaceWithQuickSlot(qSlot);
                        return false;
                    }
                    return true;
            }
            return true;
        }

        public override void OnItemDownWithKeyPress()
        {
        }
    }
}
