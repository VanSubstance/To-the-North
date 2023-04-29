using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public class AICombatController : AbsAIStatusController
    {

        protected override void OnDetectPosition(Vector3 detectPos)
        {
            baseCtrl.SetTargetToMove(detectPos, 0, false);
        }

        protected override void OnDetectUser(Transform userTf)
        {
            if (userTf != null)
            {
                baseCtrl.SetTargetToMove(userTf.position, 0, false);
            }
            baseCtrl.SetTargetToAttack(userTf);
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
