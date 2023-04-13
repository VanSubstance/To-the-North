using Assets.Scripts.Battles;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Weapon Info", menuName = "Data Objects/Items/Equipments/Weapon", order = int.MaxValue)]
    public class ItemWeaponInfo : ItemEquipmentInfo
    {
        [HideInInspector]
        public new EquipmentType equipmentType
        {
            get
            {
                return EquipmentType.Weapon;
            }
        }

        public EquipHandType handType;
        public ItemBulletType bulletType;
        public int range;
        // 아래 코드는 예시용임!
        // 리팩토링이 반드시 필요함!!
        public ProjectileInfo projectileInfo;
        public float delayAmongFire;
        [SerializeField]
        private float powerKnockback, powerPenetration, powerImpact;
        public float PowerKnockback
        {
            set
            {
                powerKnockback = value;
            }
            get
            {
                return powerKnockback >= 0 ? powerKnockback : 0;
            }
        }
        public float PowerPenetration
        {
            set
            {
                powerPenetration = value;
            }
            get
            {
                return powerPenetration >= 0 ? powerPenetration : 0;
            }
        }
        public float PowerImpact
        {
            set
            {
                powerImpact = value;
            }
            get
            {
                return powerImpact >= 0 ? powerImpact : 0;
            }
        }
    }
}
