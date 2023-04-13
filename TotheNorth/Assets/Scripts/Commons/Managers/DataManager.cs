using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    internal class DataManager : MonoBehaviour
    {
        [SerializeField]
        private ItemInventoryInfo[] inventoryInfo;
        private void Awake()
        {
            InGameStatus.Item.inventory.AddRange(inventoryInfo);
            GlobalStatus.Loading.System.InventoryLoading = true;
        }
    }
}
