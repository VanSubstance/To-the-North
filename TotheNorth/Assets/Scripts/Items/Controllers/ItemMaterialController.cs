using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemMaterialController : AItemBaseController<ItemMaterialInfo>
    {
        private readonly string TAG = "재료 아이템:\n";

        protected override bool CheckItemTag(InventorySlotController slot, bool isGridOn)
        {
            switch (slot.slotType)
            {
                case SlotType.Inventory:
                case SlotType.Rooting:
                case SlotType.Shop:
                case SlotType.Ground:
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
    }
}
