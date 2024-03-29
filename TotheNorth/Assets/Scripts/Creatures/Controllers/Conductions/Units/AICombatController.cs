using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Conductions
{
    internal class AICombatController : AIConductionController
    {
        public int numberOfGazeAfterLost = 3;
        private Transform targetTf;
        private Vector3? lastPosition;
        private bool isNowLost = false, isTimerOn = false;
        private int afterLost = 0;
        private float timerSelfControl = 0f;

        public bool isInSelfControl = false;
        private new void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (!isInSelfControl)
            {
                return;
            }
            if (baseController.statusType == AIStatusType.Combat)
            {
                if (targetTf != null)
                {
                    // 유저가 시야에 있을 때
                    // 계속 새로고침하면서 추격
                    if (!isTimerOn)
                    {
                        StartCoroutine(CoroutineSelfControll());
                    }
                    timerSelfControl = 5f;
                    baseController.SetTargetToTrack(targetTf.position, 0, false);
                    baseController.SetTargetToGaze(targetTf.position, 0);
                }
                else
                {
                    // 유저가 안보일 때
                    // = 유저의 마지막 위치만 알고있을 때
                    if (isNowLost)
                    {
                        baseController.SetTargetToTrack((Vector3)lastPosition, 0, true);
                        //baseController.SetTargetToGaze(lastPosition, 0);
                        isNowLost = false;
                    }
                    else
                    {
                        if (baseController.isAllActDone())
                        {
                            if (afterLost < numberOfGazeAfterLost)
                            {
                                baseController.SetTargetToGaze(null, 2);
                                afterLost++;
                            }
                        }
                    }
                }
                return;
            }
        }

        public void SetTargetTf(Transform _targetTf)
        {
            targetTf = _targetTf;
            if (targetTf != null)
            {
                lastPosition = targetTf.position;
                isNowLost = true;
                afterLost = 0;
            }
        }

        private IEnumerator CoroutineSelfControll()
        {
            isTimerOn = true;
            while (timerSelfControl > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                timerSelfControl -= Time.deltaTime;
            }
            isInSelfControl = false;
            isTimerOn = false;
        }
    }
}
