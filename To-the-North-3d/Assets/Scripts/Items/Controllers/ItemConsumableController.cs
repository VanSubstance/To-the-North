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

        protected override void OnDoubleClick()
        {
            CommonGameManager.Instance.ApplyConsumable(Info);
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
                        // 퀵슬롯에 옮겼는데 퀵슬롯에 기존 아이템이 있다
                        // = 두개 자리 바꾸기
                        // 1. 기존 퀵슬롯 아이템 해제
                        _targetItemInfo.Ctrl.ItemDetach();
                        // 2. 인벤토리에 자동정렬로 넣기
                        _targetItemInfo.Ctrl.SendToInventory();
                        // 3. 현재 이 아이템 퀵슬롯에 등록
                        ItemAttach(qSlot);
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
