using UnityEngine;

namespace Assets.Scripts.Items
{
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
        public float powerPenetration;
        /// <summary>
        /// 충격 가중치 (한연산)
        /// 기본값: 0
        /// </summary>
        public float powerImpact;
        /// <summary>
        /// 속도 가중치 (곱연산)
        /// 기본값 : 1
        /// </summary>
        public float powerSpd;
    }
}
