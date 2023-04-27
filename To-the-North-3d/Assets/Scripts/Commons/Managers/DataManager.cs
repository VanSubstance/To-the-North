using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using UnityEngine;
using System;

namespace Assets.Scripts.Commons
{
    internal class DataManager : MonoBehaviour
    {
        // 위치를 지정하는 아이템 리스트
        public ItemInventoryInfo[] inventoryInfo;
        [SerializeField]
        // 자동 정렬되는 아이템 리스트
        public ItemBaseInfo[] itemsAutoAlign;
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
        private void Start()
        {
            LoadInventory();
            LoadInventoryWithAuto();
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
                WindowInventoryController.Instance.GenerateItemObject(ContentType.Inventory, c);
            }
        }

        private void LoadInventoryWithAuto()
        {
            foreach (ItemBaseInfo _info in itemsAutoAlign)
            {
                WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Inventory, Instantiate(_info));
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
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.BackPack, back ? Instantiate(back) : null);

            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Right, right ? Instantiate(right) : null);
            InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Left, left ? Instantiate(left) : null);
        }
    }
}
