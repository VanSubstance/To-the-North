using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    class CreatureHitController : MonoBehaviour
    {
        private ICreatureBattle battleFunction;
        [SerializeField]
        private Transform ownerTf;
        public Transform Owner
        {
            get
            {
                return ownerTf;
            }
        }
        private float timeVib = .5f, timeLeft;

        private void Awake()
        {
            battleFunction = ownerTf.GetComponent<ICreatureBattle>();
        }

        /// <summary>
        /// 피격당했을 때 작동하는 함수
        /// </summary>
        /// <param name="partType">피격당한 부위</param>
        /// <param name="armorInfo">피격당한 부위의 방어구 정보</param>
        /// <param name="attackInfo">공격의 정보</param>
        /// <param name="hitDir">공격을 받은 방향</param>
        public void OnHit(ItemArmorInfo armorInfo, AttackInfo attackInfo, Vector3 hitDir)
        {
            int[] damage = BattleCalcFunciton.GetDamageTotalToApply(armorInfo, attackInfo);
            battleFunction.OnHit(PartHitController.DecideHitPart(), armorInfo, attackInfo, damage, hitDir);
            if (timeLeft > 0)
            {
                timeLeft = timeVib;
            }
            else
            {
            }
        }
    }
}
