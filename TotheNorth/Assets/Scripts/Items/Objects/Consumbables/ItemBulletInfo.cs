using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Bullet Info", menuName = "Data Objects/Items/Consumable/Bullet", order = int.MaxValue)]
    public class ItemBulletInfo : ItemConsumableInfo
    {
        [HideInInspector]
        public new ConsumbableType consumbableType
        {
            get
            {
                return ConsumbableType.Bullet;
            }
        }

        public ItemBulletType bulletType;
        /// <summary>
        /// 관통 가중치 (한연산)
        /// 기본값: 0
        /// </summary>
        public int powerPenetration;
        /// <summary>
        /// 충격 가중치 (한연산)
        /// 기본값: 0
        /// </summary>
        public int powerImpact;
        /// <summary>
        /// 넉백 가중치 (한연산)
        /// 기본값: 0
        /// </summary>
        public int powerKnockback;
        /// <summary>
        /// 속도 가중치 (곱연산)
        /// 기본값 : 1
        /// </summary>
        public float powerSpd = 1;
        /// <summary>
        /// 관통 데미지 가중치 (합연산)
        /// 기본값: 0
        /// </summary>
        public int damagePenetration;
        /// <summary>
        /// 충격 데미지 가중치 (합연산)
        /// 기본값: 0
        /// </summary>
        public int damageImpact;
        [SerializeField]
        /// <summary>
        /// 현재 잔여량
        /// 최대치: 60
        /// </summary>
        private int amountCurrent = 60;

        public int AmountCount
        {
            get
            {
                return amountCurrent;
            }
            set
            {
                amountCurrent = value >= 0 ? value : 0;
            }
        }
    }
}
