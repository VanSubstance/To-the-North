using Assets.Scripts.Battles;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Weapon Info", menuName = "Data Objects/Items/Equipments/Weapon", order = int.MaxValue)]
    internal class ItemWeaponInfo : ItemEquipmentInfo
    {
        public int range;
        // 아래 코드는 예시용임!
        // 리팩토링이 반드시 필요함!!
        public ProjectileInfo projectileInfo;
        public float delayAmongFire;
        public float powerKnockback;
        public float PowerPenetration
        {
            set
            {
                PowerPenetration = value;
            }
            get
            {
                return PowerPenetration >= 0 ? PowerPenetration : 0;
            }
        }
        public float PowerImpact
        {
            set
            {
                PowerImpact = value;
            }
            get
            {
                return PowerImpact >= 0 ? PowerImpact : 0;
            }
        }
    }
}
