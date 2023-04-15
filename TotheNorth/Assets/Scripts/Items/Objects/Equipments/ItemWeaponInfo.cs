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
        private ItemMagazineInfo magazine;

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
        /// 투사체 반환 함수
        /// </summary>
        /// <returns>근거리 무기 or 탄환이 있는 원거리 무기 = 투사체, 탄환이 없다면 null 반환</returns>
        public ProjectileInfo GetProjectileInfo()
        {
            if (bulletType != ItemBulletType.None)
            {
                if (magazine == null) return null;
                ItemBulletInfo bulletInfo;
                // 탄환 필요
                if (!magazine.GetNextBullet(out bulletInfo))
                {
                    // 탄환 없음
                    return null;
                }
                // 탄환 있음
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
            // 투사체 없음
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
        ///  재장전 함수
        /// </summary>
        /// <param name="_magazine"></param>
        public void ReloadMagazine(ItemMagazineInfo _magazine)
        {
            magazine = _magazine;
        }
    }
}
