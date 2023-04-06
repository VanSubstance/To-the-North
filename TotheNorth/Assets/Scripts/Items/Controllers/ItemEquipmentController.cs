using UnityEngine;

namespace Assets.Scripts.Items.Controllers
{
    class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        private readonly string TAG = "장비 아이템:\n";

        protected override bool CheckItemTag(string slotType)
        {
            if (slotType == "Inventory" || slotType == "Equipment" || slotType == "QuickSlot" || slotType == "Rooting")
            {
                return true;
            }
            return false;
        }

        protected override void InitExtraContent(ItemEquipmentInfo content)
        {
            throw new System.NotImplementedException();
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
