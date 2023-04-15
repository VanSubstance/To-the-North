using UnityEngine;

namespace Assets.Scripts.Items.Objects
{

    [CreateAssetMenu(fileName = "Food Info", menuName = "Data Objects/Items/Consumable/Food", order = int.MaxValue)]
    public class ItemFoodInfo : ItemConsumableInfo
    {
        [HideInInspector]
        public new ConsumbableType consumbableType
        {
            get
            {
                return ConsumbableType.Food;
            }
        }
    }
}
