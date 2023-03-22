using System.Collections.Generic;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Conductions
{
    internal class AITraceController : AIConductionController
    {
        private Transform targetTf;
        private Vector3? lastPosition;
        private bool isNowLost = false;
        private new void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (baseController.statusType == Interfaces.AIStatusType.Trace)
            {
                if (targetTf != null)
                {
                    // 유저가 시야에 있을 때
                    // 계속 새로고침하면서 추격
                    baseController.targetToMove = targetTf.position;
                    baseController.targetToGaze = targetTf.position;
                }
                else
                {
                    // 유저가 안보일 때
                    // = 유저의 마지막 위치만 알고있을 때
                    if (isNowLost)
                    {
                        baseController.targetToMove = lastPosition;
                        baseController.targetToGaze = lastPosition;
                        isNowLost = false;
                    }
                }
            }
        }

        public void SetTargetTf(Transform _targetTf)
        {
            targetTf = _targetTf;
            if (targetTf != null)
            {
                lastPosition = targetTf.position;
                isNowLost = true;
            }
        }
    }
}
