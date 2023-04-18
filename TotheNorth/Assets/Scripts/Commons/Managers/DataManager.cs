using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    internal class DataManager : MonoBehaviour
    {
        [SerializeField]
        public ItemInventoryInfo[] inventoryInfo;
        [SerializeField]
        public ItemArmorInfo helmat, mask, body, leg, back;
        [SerializeField]
        public ItemWeaponInfo right, left;

        private static DataManager _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static DataManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(DataManager)) as DataManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }
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

        public void updateEquipment()
        {
            LoadEquipment();
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
