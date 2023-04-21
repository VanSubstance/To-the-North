using Assets.Scripts.Creatures.Bases;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {
        public override void OnDetectUser(Vector3? targetPos)
        {
            if (targetPos == null) return;
            statusType = Interfaces.AIStatusType.Combat;
            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectUser((Vector3)targetPos);
            }
        }
    }
}
