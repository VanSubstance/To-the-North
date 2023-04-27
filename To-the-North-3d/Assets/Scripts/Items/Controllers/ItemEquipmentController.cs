using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        private const string TAG = "장비 아이템:\n";

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

        public override void GridOnCheckIfItemExist(InventorySlotController slotController)
        {
        }
    }
}