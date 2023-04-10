using System.Collections;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    internal class AIRunawayController : AIConductionController
    {
        private bool isTimerOn = false;
        private float timerRunaway, timerRunawayFull = 3f;
        private new void Awake()
        {
            base.Awake();
            timerRunaway = 0f;
        }

        /// <summary>
        /// 도주 명령 함수
        /// </summary>
        /// <param name="posFrom">으로부터 도망칠 방향</param>
        public void RunawayFrom(Vector3 dirFrom)
        {
            baseController.ClearAllAct();
            baseController.SetTargetToTrack(
            transform.position +
                (transform.position - dirFrom).normalized * 30,
                4f,
                true
                );
            timerRunaway = 0f;
            if (isTimerOn)
                return;
            StartCoroutine(CoroutineRunaway());
        }

        /// <summary>
        /// 도주 명령 함수
        /// </summary>
        /// <param name="posFrom">으로 도망칠 방향</param>
        public void RunawayTo(Vector3 dirTo)
        {
            baseController.ClearAllAct();
            baseController.SetTargetToTrack(
                transform.position +
                dirTo.normalized * 30,
                4f,
                true
                );
            timerRunaway = 0f;
            if (isTimerOn)
                return;
            StartCoroutine(CoroutineRunaway());
        }

        private IEnumerator CoroutineRunaway()
        {
            isTimerOn = true;
            while (timerRunaway < timerRunawayFull)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                timerRunaway += Time.deltaTime;
            }
            baseController.statusType = AIStatusType.Wander;
            baseController.SetTargetToTrack(
            transform.position,
            4f,
                true
                );
            isTimerOn = false;
        }
    }
}
