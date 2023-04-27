using Assets.Scripts.Components.Popups;
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
            Debug.unityLogger.Log(TAG, "더블클릭!");
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
            if (Info.consumbableType.Equals(ConsumbableType.Bullet))
            {
                if (_targetItemInfo is ItemMagazineInfo)
                {
                    // 탄환 -> 탄창
                    ((ItemMagazineInfo)_targetItemInfo).LoadMagazine((ItemBulletInfo)Info);
                }
            }
        }

        public override void OnItemDownWithKeyPress()
        {
        }
    }
}
