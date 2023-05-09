using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Armor Info", menuName = "Data Objects/Items/Equipments/Armor", order = int.MaxValue)]
    public class ItemArmorInfo : ItemEquipmentInfo
    {
        [HideInInspector]
        public new EquipmentType equipmentType
        {
            get
            {
                return EquipmentType.Armor;
            }
        }
        public EquipBodyType equipPartType;
        [SerializeField]
        [Range(0, 3)]
        private int classPenetration, classImpact, classHeat;
        public int ClassPenetration
        {
            set
            {
                classPenetration = value;
            }

            get
            {
                return classPenetration >= 0 ? classPenetration : 0;
            }
        }
        public int ClassImpact
        {
            set
            {
                classImpact = value;
            }

            get
            {
                return classImpact >= 0 ? classImpact : 0;
            }
        }
        public int ClassHeat
        {
            get
            {
                return classHeat;
            }

            set
            {
                classHeat = value;
            }
        }

        public static ItemArmorInfo GetPlainArmor()
        {
            ItemArmorInfo res = CreateInstance<ItemArmorInfo>();
            res.classPenetration = 0;
            res.classImpact = 0;
            res.classHeat = 0;
            return res;
        }
    }
}
