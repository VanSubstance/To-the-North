using TMPro;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentEquipmentController : ContentBaseController
    {
        public TextMeshProUGUI helmetT, maskT, bodyT, backpackT, weaponPriT, weaponSecT;
        public EquipmentSlotController helmetCtrl, maskCtrl, bodyCtrl, backpackCtrl, weapon1Ctrl, weapon2Ctrl;

        private void Awake()
        {
            GlobalComponent.Common.Text.Inventory.Equipment.helmet = helmetT;
            GlobalComponent.Common.Text.Inventory.Equipment.mask = maskT;
            GlobalComponent.Common.Text.Inventory.Equipment.body = bodyT;
            GlobalComponent.Common.Text.Inventory.Equipment.backpack = backpackT;
            GlobalComponent.Common.Text.Inventory.Equipment.weaponPrimary = weaponPriT;
            GlobalComponent.Common.Text.Inventory.Equipment.weaponSecondary = weaponSecT;
        }
    }
}
