using Assets.Scripts.Items;
using Assets.Scripts.Components.Hovers;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class DataManager : MonoBehaviour
    {
        // 위치를 지정하는 아이템 리스트
        public ItemInventoryInfo[] inventoryInfo;
        [SerializeField]
        // 자동 정렬되는 아이템 리스트
        public ItemBaseInfo[] itemsAutoAlign;
        [SerializeField]
        public ItemArmorInfo helmat, mask, body, leg, back;
        [SerializeField]
        public ItemWeaponInfo hand;

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

        private void LoadInventoryWithAuto()
        {
            foreach (ItemBaseInfo _info in itemsAutoAlign)
            {
                InGameStatus.Item.PushItemToInventory(Instantiate(_info));
            }
        }

        private void LoadEquipment()
        {
            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Helmat, helmat ? Instantiate(helmat) : null);
            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Mask, mask ? Instantiate(mask) : null);
            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Body, body ? Instantiate(body) : null);
            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Leg, leg ? Instantiate(leg) : null);
            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.BackPack, back ? Instantiate(back) : null);

            //InGameStatus.Item.SetEquipmentInfo(EquipBodyType.Hand, hand ? Instantiate(hand) : null);
        }
    }
}
