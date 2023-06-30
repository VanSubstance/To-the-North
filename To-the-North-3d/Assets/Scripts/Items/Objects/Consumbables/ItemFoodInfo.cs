using UnityEngine;

namespace Assets.Scripts.Items
{

    [CreateAssetMenu(fileName = "Food Info", menuName = "Data Objects/Items/Consumable/Food", order = int.MaxValue)]
    public class ItemFoodInfo : ItemConsumableInfo
    {
        public ConsumeType consumeType;
        [SerializeField]
        private float hunger, thirst, temperature;
        public float Hunger
        {
            get
            {
                return hunger;
            }
            set
            {
                hunger = value;
            }
        }
        public float Thirst
        {
            get
            {
                return thirst;
            }
            set
            {
                thirst = value;
            }
        }
        public float Temperature
        {
            get
            {
                return temperature;
            }
            set
            {
                temperature = value;
            }
        }

        public enum ConsumeType
        {
            Burgur, Chip, Drink
        }
    }
}
