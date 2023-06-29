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
        }

        protected override void OnDetectUser(Transform userTf)
        {
        }
    }
}
