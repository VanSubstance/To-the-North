using Assets.Scripts.Creatures.Bases;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {

        public override void OnDetectPosition(Vector3 targetPos)
        {
            statusType = Interfaces.AIStatusType.Combat;
            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectPosition(targetPos);
            }
        }

        public override void OnDetectUser(Transform userTf)
        {
            statusType = Interfaces.AIStatusType.Combat;
            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectUser(userTf);
            }
        }
    }
}
