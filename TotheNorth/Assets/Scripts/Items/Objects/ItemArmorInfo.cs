using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Armor Info", menuName = "Data Objects/Items/Equipments/Armor", order = int.MaxValue)]
    internal class ItemArmorInfo : ItemEquipmentInfo
    {
        public int ClassPenetration
        {
            set
            {
                ClassPenetration = value;
            }

            get
            {
                return ClassPenetration >= 0 ? ClassPenetration : 0;
            }
        }
        public int ClassImpact
        {
            set
            {
                ClassImpact = value;
            }

            get
            {
                return ClassImpact >= 0 ? ClassImpact : 0;
            }
        }
    }
}
