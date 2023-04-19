using System.Linq;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
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
        public float delayAmongFire, timeReload = 2.5f, headHeight = .25f;
        [SerializeField]
        private float powerKnockback;
        [SerializeField]
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
        /// <param name="isAI">AI인지 아닌지</param>
        /// <returns>근거리 무기 or 탄환이 있는 원거리 무기 = 투사체, 탄환이 없다면 null 반환</returns>
        public ProjectileInfo GetProjectileInfo(bool isAI)
        {
            if (bulletType != ItemBulletType.None)
            {
                // 탄환 필요
                ItemBulletInfo bulletInfo;
                if (bulletType == ItemBulletType.Arrow)
                {
                    // 활 = 화살은 탄창이 없다
                    if (isAI)
                    {
                        bulletInfo = GlobalComponent.Path.GetMonsterBulletInfo(ItemBulletType.Arrow, 1);
                    }
                    else
                    {
                        // 가방에서 화살 있으면 걍 꺼내서 쏨
                        bulletInfo = InGameStatus.Item.LookforBullet(ItemBulletType.Arrow);
                        if (bulletInfo == null) return null; // 인벤토리에 화살 없음
                    }
                    bulletInfo.AmountCount--;
                }
                else if (magazine == null || !magazine.GetNextBullet(out bulletInfo))
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
        /// <returns>원래 장착되어있던 탄창</returns>
        public ItemMagazineInfo ReloadMagazine(ItemMagazineInfo _magazine)
        {
            if (magazine == null)
            {
                return null;
            }
            ItemMagazineInfo oldMagazine = Instantiate(magazine);
            magazine = _magazine;
            return oldMagazine;
        }

        public bool isMagazineRequired()
        {
            return
                !new ItemBulletType[] { ItemBulletType.Arrow, ItemBulletType.None }.Contains(bulletType);
        }
    }
}
