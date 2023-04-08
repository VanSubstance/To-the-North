using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableController : AItemBaseController
    {
        private readonly string TAG = "소모성 아이템:\n";

        [SerializeField]
        private ItemConsumableInfo info;
        protected override bool CheckItemTag(SlotType slotType)
        {
            if (slotType == SlotType.Inventory || slotType == SlotType.Quick ||
                slotType == SlotType.Rooting || slotType == SlotType.Shop ||
                slotType == SlotType.Ground)
            {
                return true;
            }
            return false;
        }

        protected override ItemBaseInfo GetBaseInfo()
        {
            return info.GetClone();
        }

        protected override void OnDoubleClick()
        {
            Debug.unityLogger.Log(TAG, "더블클릭!");
        }

        protected override void OnHover()
        {
            Debug.unityLogger.Log(TAG, "호버링");
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
