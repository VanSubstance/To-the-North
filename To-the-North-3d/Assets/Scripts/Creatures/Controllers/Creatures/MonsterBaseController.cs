using Assets.Scripts.Creatures.Bases;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {

        public override void OnDetectPosition(Vector3 targetPos)
        {
            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectPosition(targetPos);
            }
        }

        public override void OnDetectUser(Transform userTf)
        {
            if (IsRunaway)
            {
                statusType = Interfaces.AIStatusType.Runaway;
            }
            else if (Info.IsActiveBehaviour)
            {
                statusType = Interfaces.AIStatusType.Combat;
            }
            else
            {

            }

            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectUser(userTf);
            }
        }
    }
}
