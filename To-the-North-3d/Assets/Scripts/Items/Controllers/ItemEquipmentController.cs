using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        private const string TAG = "장비 아이템:\n";

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

        public override void GridOnCheckIfItemExist(InventorySlotController slotController)
        {
        }
    }
}