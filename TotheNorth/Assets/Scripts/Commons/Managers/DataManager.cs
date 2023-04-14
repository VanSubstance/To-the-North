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
            ItemInventoryInfo c;
            foreach (ItemInventoryInfo inf in inventoryInfo)
            {
                c = new ItemInventoryInfo
                {
                    pos = inf.pos,
                    itemInfo = Instantiate(inf.itemInfo)
                };
                InGameStatus.Item.inventory.Add(c);
            }
        }

        private void LoadEquipment()
        {
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Helmat, helmat ? Instantiate(helmat) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Mask, mask ? Instantiate(mask) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Body, body ? Instantiate(body) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Leg, leg ? Instantiate(leg) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Back, back ? Instantiate(back) : null);

            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Right, right ? Instantiate(right) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Left, left ? Instantiate(left) : null);
        }
    }
}
