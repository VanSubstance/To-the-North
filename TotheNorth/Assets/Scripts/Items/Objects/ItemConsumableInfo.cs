
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Consumable Info", menuName = "Data Objects/Items/Consumable", order = int.MaxValue)]
    public class ItemConsumableInfo : ItemBaseInfo
    {
        public new ItemType itemType
        {
            get
            {
                return ItemType.Consumable;
            }
        }
        // 소모성 아이템 필요 인포
        public ConsumbableType consumbableType;
    }
}
