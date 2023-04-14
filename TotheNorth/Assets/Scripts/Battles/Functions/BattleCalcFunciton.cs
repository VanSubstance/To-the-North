using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public static class BattleCalcFunciton
    {
        /// <summary>
        /// 실제 적용될 데미지 계산 함수
        /// 데미지 계산을 진행하며 상황에 따라 방어구 내구도에도 영향을 미친다
        /// </summary>
        /// <param name="armorInfo">방어구 정보</param>
        /// <param name="attackInfo">공격 정보</param>
        /// <returns>[0]: 전체 데미지, [1]: 관통 데미지, [2]: 충격 데미지</returns>
        public static int[] GetDamageTotalToApply(ItemArmorInfo armorInfo, AttackInfo attackInfo)
        {
            int damagePenetration, damageImpact;
            damagePenetration = IsPenetrationSuccess(armorInfo, attackInfo.powerPenetration) ? attackInfo.damagePenetration : 0;
            damageImpact = GetDamageImpactToApply(armorInfo, attackInfo.powerImpact, attackInfo.damageImpact);
            return new int[]
            {
                damagePenetration + damageImpact,
                damagePenetration,
                damageImpact,
            };
        }

        /// <summary>
        /// 관통 성공 확률 계산 함수
        /// </summary>
        /// <param name="x">관통 방어 클래스</param>
        /// <param name="k">관롱력</param>
        /// <returns>관통 성공 확률</returns>
        public static float GetProbabilityOfPenetration(int x, float k)
        {
            return
                MathF.Min(
                    1 / (
                        x + 1 - (k / 100f)
                        )
                    - 0.1f
                    + (k / 200f)
                    , 1
                );
        }

        /// <summary>
        /// 관통 성공 여부 반환 함수
        /// 관통 성공 시, 관통 확률에 따른 방어구의 내구도 차등 손실
        /// </summary>
        /// <param name="x">관통 방어 클래스</param>
        /// <param name="k">관롱력</param>
        /// <returns></returns>
        public static bool IsPenetrationSuccess(ItemArmorInfo armorInfo, int k)
        {
            float prob = GetProbabilityOfPenetration(armorInfo.ClassPenetration, k);
            if (UnityEngine.Random.Range(0f, 1f) <= prob)
            {
                armorInfo.Durability -= UnityEngine.Random.Range(0f, 0.5f) * prob;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 충격 감소율 계산 함수
        /// </summary>
        /// <param name="x">충격 방어 클래스</param>
        /// <param name="k">충격력</param>
        /// <returns></returns>
        public static float GetReductionRateOfImpact(int x, int k)
        {
            return
                0.05f
                * (
                    MathF.Pow(x,
                        (2 - (k / 100f))
                    )
                )
                + (
                    (100 - k)
                    / 1000f
                );
        }

        /// <summary>
        /// 충격 감소율이 적용된 실제 전달될 데미지 계산 함수
        /// 충격 감소율 수준에 따라 방어구의 내구도 차등 손실
        /// </summary>
        /// <param name="x">충격 방어 클래스</param>
        /// <param name="k">충격력</param>
        /// <param name="damage">적용 전 충격 데미지</param>
        /// <returns></returns>
        public static int GetDamageImpactToApply(ItemArmorInfo armorInfo, int k, int damage)
        {
            float rate = GetReductionRateOfImpact(armorInfo.ClassImpact, k);
            armorInfo.Durability -= UnityEngine.Random.Range(0f, 0.5f) * (1 - rate);
            return (int)(damage * (1 - rate));
        }
    }
}
