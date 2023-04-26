using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableController : AItemBaseController<ItemConsumableInfo>
    {
        private readonly string TAG = "소모성 아이템:\n";

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
            Debug.unityLogger.Log(TAG, "키 누른 상태로 클릭");
        }

        protected override void OnMouseEnterWithKeyPress()
        {
            Debug.unityLogger.Log(TAG, "키 누른 상태로 진입");
        }

        public override void GridOnCheckIfItemExist(InventorySlotController slotController)
        {
            if (((ItemConsumableInfo)info).consumbableType == ConsumbableType.Bullet)
            {
                Debug.Log("옮기는 것: 탄환");
                Debug.Log("강 민 준:: 정확히 붙어있는 칸 빼고 다른 칸에 올리면 Null 뜸");
                switch (slotController.AttachedInfo.GetType().Name)
                {
                    case "ItemMagazineInfo":
                        //Debug.Log("아래 있는 것: 탄창");
                        ((ItemMagazineInfo)slotController.AttachedInfo).LoadMagazine((ItemBulletInfo)info);
                        break;
                    case "ItemWeaponInfo":
                    case "ItemArmorInfo":
                    case "ItemFoodInfo":
                    case "ItemBulletInfo":
                    case "ItemMaterialInfo":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
