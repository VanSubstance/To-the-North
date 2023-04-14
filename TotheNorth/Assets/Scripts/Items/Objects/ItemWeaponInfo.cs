using Assets.Scripts.Battles;
using Assets.Scripts.Items.Objects;
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
        public int range, spd, damagePenetration, damageImpact;
        private ProjectileInfo projectileInfo;
        public float delayAmongFire, headHeight = .25f;
        [SerializeField]
        private float powerKnockback;
        private int powerPenetration, powerImpact;
        [SerializeField]
        private TrajectoryType trajectoryType;

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
        public int PowerPenetration
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
        public int PowerImpact
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

        /// <summary>
        /// 탄환이 없는 무기의 투사체 반환 함수
        /// </summary>
        /// <returns></returns>
        public ProjectileInfo GetProjectileInfo()
        {
            if (bulletType != ItemBulletType.None)
            {
                Debug.Log("탄환 정보가 필요합니다!");
                return null;
            }
            if (projectileInfo != null) return projectileInfo;
            projectileInfo = new()
            {
                TrajectoryType = trajectoryType,
                Spd = spd,
                Range = range,
                Height = headHeight,
                PowerKnockback = powerKnockback,
                AttackInfo = AttackInfo.GetAttackInfo(this)
            };
            return projectileInfo;
        }

        /// <summary>
        /// 탄환이 있는 무기 투사체 반환 함수
        /// </summary>
        /// <returns></returns>
        public ProjectileInfo GetProjectileInfo(ItemBulletInfo bulletInfo)
        {
            if (bulletType == ItemBulletType.None)
            {
                Debug.Log("탄환 정보가 필요 없습니다!");
                return GetProjectileInfo();
            }
            if (bulletInfo.AmountCount == 0)
            {
                Debug.Log("탄환이 없습니다!");
                return null;
            }
            bulletInfo.AmountCount--;
            projectileInfo = new()
            {
                TrajectoryType = trajectoryType,
                Spd = spd * bulletInfo.powerSpd,
                Range = range,
                Height = headHeight,
                PowerKnockback = powerKnockback,
                AttackInfo = AttackInfo.GetAttackInfo(this, bulletInfo)
            };
            return projectileInfo;
        }
    }
}
