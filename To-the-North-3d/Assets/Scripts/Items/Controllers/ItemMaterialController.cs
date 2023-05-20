using Assets.Scripts.Components.Hovers;
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
            HoverItemInfoController.Instance.OnHoverEnter(info);
        }

        protected override void OnHoverExit()
        {
            HoverItemInfoController.Instance.OnHoverExit();
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
        }
    }
}
