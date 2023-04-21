using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    internal class AIRunawayController : AbsAIStatusController
    {
        protected override AIStatusType AIType()
        {
            return AIStatusType.Runaway;
        }

        protected override void OnDetectionUpdate()
        {
        }

        protected override void OnDetectPosition(Vector3 detectPos)
        {
            baseCtrl.SetTargetToMove(baseCtrl.transform.position + 
                ((baseCtrl.transform.position - detectPos) * 2),
                0,
                false
                );
        }

        protected override void OnDetectUser(Transform userTf)
        {
            baseCtrl.SetTargetToMove(baseCtrl.transform.position +
                ((baseCtrl.transform.position - userTf.position) * 2),
                0,
                false
                );
        }
    }
}
