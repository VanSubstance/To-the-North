using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    internal class DataManager : MonoBehaviour
    {
        [SerializeField]
        private ItemInventoryInfo[] inventoryInfo;
        [SerializeField]
        private ItemArmorInfo helmat, mask, body, leg, back;
        [SerializeField]
        private ItemWeaponInfo right, left;
        private void Awake()
        {
            LoadInventory();
        }

        private void Update()
        {
            if (GlobalStatus.Loading.System.CommonGameManager &&
                !GlobalStatus.Loading.System.InventoryLoading)
            {
                LoadEquipment();
                GlobalStatus.Loading.System.InventoryLoading = true;
            }
        }

        private void LoadInventory()
        {
            InGameStatus.Item.inventory.AddRange(inventoryInfo);
        }

        private void LoadEquipment()
        {
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Helmat, helmat);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Mask, mask);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Body, body);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Leg, leg);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Back, back);

            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Right, right);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Left, left);
        }
    }
}
