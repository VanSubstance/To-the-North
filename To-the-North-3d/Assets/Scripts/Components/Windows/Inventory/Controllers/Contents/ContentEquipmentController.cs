using UnityEngine;
namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentEquipmentController: ContentBaseController
    {
        [SerializeField]
        private EquipmentSlotController helmetCtrl, maskCtrl, bodyCtrl, backpackCtrl, handLCtrl, handRCtrl;
    }
}
