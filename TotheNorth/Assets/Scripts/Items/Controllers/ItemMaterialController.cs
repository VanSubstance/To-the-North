using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemMaterialController : AItemBaseController
    {
        private readonly string TAG = "재료 아이템:\n";
        [SerializeField]
        private ItemMaterialInfo info;

        protected override bool CheckItemTag(SlotType slotType)
        {
            if (slotType == SlotType.Inventory || slotType == SlotType.Rooting ||
                slotType == SlotType.Ground || slotType == SlotType.Shop)
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
