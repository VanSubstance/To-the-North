using System;
using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    class CreatureHitController : MonoBehaviour
    {
        [SerializeField]
        private Transform vibrateTf;
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
        private float timeVib = .5f, timeLeft, powerVib;

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
        public void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, Vector3 hitDir)
        {
            int[] damage = BattleCalcFunciton.GetDamageTotalToApply(armorInfo, attackInfo);
            battleFunction.OnHit(partType, armorInfo, attackInfo, damage, hitDir);
            if (damage[2] <= 10)
            {
                powerVib = 0.2f;
            }
            else if (damage[2] < 30)
            {
                powerVib = 0.5f;
            }
            else
            {
                powerVib = 1f;
            }
            if (timeLeft > 0)
            {
                timeLeft = timeVib;
            }
            else
            {
                try
                {
                    if (gameObject.activeSelf)
                        StartCoroutine(CoroutineVibrate());
                }
                catch (Exception)
                {
                    // hit 죽음
                }
            }
        }

        private IEnumerator CoroutineVibrate()
        {
            timeLeft = timeVib;
            while (timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime;
                vibrateTf.localPosition = CalculationFunctions.DirFromAngle(UnityEngine.Random.Range(0, 360)) * powerVib;
                powerVib *= 0.7f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            vibrateTf.localPosition = Vector3.zero;
            timeLeft = 0;
            powerVib = 0f;
        }
    }
}
