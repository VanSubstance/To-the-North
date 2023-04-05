using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Conductions.Squads
{
    internal class AISquadCombatController : AISquadConductionController
    {
        public float timeOfMemory = 10f;
        private bool isTriggered = false;
        public float timerMemory = 0f;
        private new void Awake()
        {
            base.Awake();
        }
        private void Update()
        {

            if (baseController.statusType == AIStatusType.Combat)
            {
                if (!isTriggered)
                {
                    // 최초 조우 = 전투 개시
                    timerMemory = timeOfMemory;
                    baseController.SetAllUnitsStatus(AIStatusType.Combat);
                    StartCoroutine(CoroutineTimer());
                }
                // 전투중
                if (baseController.GetIsDetected())
                {
                    // 부대원들에게 타겟 위치 전달
                    baseController.SetTargetToTrack((Vector3)baseController.detectPos);
                    // 추가로 식별됨
                    timerMemory = timeOfMemory;
                }
            }
        }

        private IEnumerator CoroutineTimer()
        {
            isTriggered = true;
            while (timerMemory > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                timerMemory -= Time.deltaTime;
            }
            while (!baseController.isAllActDone())
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            baseController.SetAllUnitsStatus(AIStatusType.None);
            baseController.statusType = AIStatusType.Petrol;
            baseController.detectPos = null;
            baseController.GetIsDetected();
            isTriggered = false;
            timerMemory = 0f;
        }
    }
}
