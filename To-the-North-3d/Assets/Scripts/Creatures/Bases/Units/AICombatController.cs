using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public class AICombatController : AbsAIStatusController
    {

        protected override void OnDetectPosition(Vector3 detectPos)
        {
        }

        protected override void OnDetectUser(Transform userTf)
        {
        }

        protected override void OnDetectionUpdate()
        {
            baseCtrl.CheckAim();
        }

        protected override AIStatusType AIType()
        {
            return AIStatusType.Combat;
        }
    }
}
