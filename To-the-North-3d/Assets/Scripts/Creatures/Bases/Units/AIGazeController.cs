using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public class AIGazeController : AbsAIStatusController
    {

        protected override void OnDetectPosition(Vector3 detectPos)
        {
            //baseCtrl.SetTargetToGaze(detectPos - baseCtrl.transform.position, 0, false);
        }

        protected override void OnDetectUser(Transform userTf)
        {
            //baseCtrl.SetTargetToGaze(userTf.position - baseCtrl.transform.position, 0, false);
        }

        protected override void OnDetectionUpdate()
        {
            if (!IsDuringAction) ResetTimer();
            if (baseCtrl.IsOrderDone)
            {
                Vector3 p = baseCtrl.transform.position;
                p.x += Random.Range(-6f, 6f);
                p.z += Random.Range(-6f, 6f);
                if (p == new Vector3(0, 0, 0)) return;
                baseCtrl.SetTargetToGaze(p, 3f, false);
            }
        }

        protected override AIStatusType AIType()
        {
            return AIStatusType.Gaze;
        }
    }
}
