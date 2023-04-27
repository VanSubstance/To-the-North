using UnityEngine;

namespace Assets.Scripts.Items
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

        [SerializeField]
        private int hunger, thirst, temperature;
        public int Hunger
        {
            get
            {
                return hunger;
            }
        }
        public int Thirst
        {
            get
            {
                return thirst;
            }
        }
        public int Temperature
        {
            get
            {
                return temperature;
            }
        }
    }
}
