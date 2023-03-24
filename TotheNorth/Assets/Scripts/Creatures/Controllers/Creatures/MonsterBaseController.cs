using System.Collections;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Conductions;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {
        public float timeOfMemory = 1f;
        public float secMemory = 0f;
        /// <summary>
        /// 유저를 식별하였을 때 작동하는 함수
        /// </summary>
        /// <param name="targetTf">Null이 아니다 = 유저가 눈에 보인다, Null이다 = 유저가 안보인다</param>
        public override void OnDetectUser(Transform targetTf)
        {
            if (squadBase != null)
            {
                // 부대 소속 유닛
                if (targetTf != null)
                {
                    // 유저 식별 시에만 전달
                    squadBase.DetectEnemy(targetTf.position);
                    statusType = Interfaces.AIStatusType.Combat;
                }
            }
            else
            {
                // 단독 유닛
                if (statusType != Interfaces.AIStatusType.Combat)
                {
                    // 최초일 경우
                    if (targetTf != null)
                    {
                        // 최초 유저 조우
                        statusType = Interfaces.AIStatusType.Combat;
                        StartCoroutine(CountMemory());
                    }
                    else
                    {
                        // 무시
                        return;
                    }
                }
                if (targetTf != null)
                {
                    secMemory = timeOfMemory;
                }
            }
            GetComponent<AICombatController>().SetTargetTf(targetTf);
        }

        private IEnumerator CountMemory()
        {
            while (secMemory > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                if (!isAllActDone())
                {
                    secMemory = timeOfMemory;
                }
                secMemory -= Time.deltaTime;
            }
            statusType = Interfaces.AIStatusType.Petrol;
        }
    }
}
