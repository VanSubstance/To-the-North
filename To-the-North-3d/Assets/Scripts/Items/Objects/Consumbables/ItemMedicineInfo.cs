using UnityEngine;
using Assets.Scripts.Users;

namespace Assets.Scripts.Items
{

    [CreateAssetMenu(fileName = "Medicine Info", menuName = "Data Objects/Items/Consumable/Medicine", order = int.MaxValue)]
    public class ItemMedicineInfo : ItemConsumableInfo
    {
        [HideInInspector]
        public new ConsumbableType consumbableType
        {
            get
            {
                return ConsumbableType.Medicine;
            }
        }

        [SerializeField]
        private float hp;
        public float Hp
        {
            get
            {
                return hp;
            }
            set
            {
                hp = value;
            }
        }

        [SerializeField]
        public MedicineConditionEffect[] effects;
    }

    [System.Serializable]
    public class MedicineConditionEffect
    {
        public ConditionType targetCondition;
        public int countToRemove;
    }
}
