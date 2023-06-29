using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public class AIGazeController : AbsAIStatusController
    {

        protected override void OnDetectPosition(Vector3 detectPos)
        {
        }

        protected override void OnDetectUser(Transform userTf)
        {
        }

        protected override void OnDetectionUpdate()
        {
            if (!IsDuringAction) ResetTimer();
            if (baseCtrl)
            {
                Vector3 p = baseCtrl.transform.position;
                p.x += Random.Range(-6f, 6f);
                p.z += Random.Range(-6f, 6f);
                if (p == new Vector3(0, 0, 0)) return;
            }
        }

        protected override AIStatusType AIType()
        {
            return AIStatusType.Gaze;
        }
    }
}
