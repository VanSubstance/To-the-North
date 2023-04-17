namespace Assets.Scripts.Items
{
    /// <summary>
    /// 실제 데미지 계산을 할 때 쓰이는 데이터 오브젝트
    /// </summary>
    /// 
    public class AttackInfo
    {
        public int powerPenetration;
        public int powerImpact;
        public float powerKnockback;

        public int damagePenetration, damageImpact;

        /// <summary>
        /// 무기 정보에서 공격 정보를 생성하는 함수
        /// </summary>
        /// <param name="noBulletWeapon">탄환이 없는 무기 정보</param>
        /// <returns></returns>
        public static AttackInfo GetAttackInfo(ItemWeaponInfo noBulletWeapon)
        {
            if (noBulletWeapon.bulletType != ItemBulletType.None) return null;
            AttackInfo res = new()
            {
                powerPenetration = noBulletWeapon.PowerPenetration,
                powerImpact = noBulletWeapon.PowerImpact,
                powerKnockback = noBulletWeapon.PowerKnockback,
                damagePenetration = noBulletWeapon.damagePenetration,
                damageImpact = noBulletWeapon.damageImpact,
            };
            return res;
        }


        /// <summary>
        /// 무기 정보에서 공격 정보를 생성하는 함수
        /// </summary>
        /// <param name="withBulletWeapon">탄환이 있는 무기 정보</param>
        /// <param name="bulletInfo">탄환 정보</param>
        /// <returns></returns>
        public static AttackInfo GetAttackInfo(ItemWeaponInfo withBulletWeapon, ItemBulletInfo bulletInfo)
        {
            if (withBulletWeapon.bulletType == ItemBulletType.None) return GetAttackInfo(withBulletWeapon);
            AttackInfo res = new()
            {
                powerPenetration = withBulletWeapon.PowerPenetration + bulletInfo.powerPenetration,
                powerImpact = withBulletWeapon.PowerImpact + bulletInfo.powerImpact,
                powerKnockback = withBulletWeapon.PowerKnockback,
                damagePenetration = withBulletWeapon.damagePenetration + bulletInfo.damagePenetration,
                damageImpact = withBulletWeapon.damageImpact + bulletInfo.damageImpact,
            };
            return res;
        }
    }
}
