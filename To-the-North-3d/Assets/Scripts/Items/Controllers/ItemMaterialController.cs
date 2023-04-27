using Assets.Scripts.Components.Popups;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemMaterialController : AItemBaseController
    {
        private readonly string TAG = "재료 아이템:\n";

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
        }

        public override void OnItemDownWithKeyPress()
        {
        }
    }
}
