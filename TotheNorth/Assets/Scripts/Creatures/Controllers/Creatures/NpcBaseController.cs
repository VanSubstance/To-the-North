using System.Collections;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class NpcBaseController : AIBaseController
    {
        private float secMemory = 0f;
        private Transform userTf;
        /// <summary>
        /// 유저를 식별하였을 때 작동하는 함수
        /// NPC는 유저가 시야를 벗어나도 마저 쳐다본다
        /// </summary>
        /// <param name="targetTf">유저 Transform</param>
        public override void OnDetectUser(Transform targetTf)
        {
            if (statusType != Interfaces.AIStatusType.None)
            {
                // 최초일 경우
                if (targetTf != null)
                {
                    // 최초 유저 조우
                    statusType = Interfaces.AIStatusType.None;
                    userTf = targetTf;
                    StartCoroutine(countMemory());
                }
                else
                {
                    // 무시
                    return;
                }
            }
            if (targetTf != null)
            {
                secMemory = 1f;
            }
        }

        private IEnumerator countMemory()
        {
            while (secMemory > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                targetToGaze = userTf.position;
                secMemory -= Time.deltaTime;
            }
            statusType = Interfaces.AIStatusType.Petrol;
        }
    }
}
