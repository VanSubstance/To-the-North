using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public interface ICreatureBattle
    {
        /// <summary>
        /// 크리쳐가 공격받았을 때 처리하는 함수
        /// </summary>
        /// <param name="partType">피격 부위</param>
        /// <param name="armorInfo">피격 부위 방어구 정보</param>
        /// <param name="attackInfo">공격 정보</param>
        /// <param name="damage">[0]: 전체 데미지, [1]: 관통 데미지, [2]: 충격 데미지</param>
        /// <param name="hitDir">피격 방향 (상대 좌표)</param>
        public void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir);
    }
}
