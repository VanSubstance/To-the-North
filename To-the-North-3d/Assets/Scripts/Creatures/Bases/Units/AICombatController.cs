using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public class AICombatController : AbsAIStatusController
    {
        [SerializeField]
        private float timeMaxMemory;
        private float timeLiveMemory;
        private void Awake()
        {
            timeLiveMemory = 0;
        }

        private void Update()
        {
            if (baseCtrl.statusType != Interfaces.AIStatusType.Combat) return;
            if (timeMaxMemory <= timeLiveMemory)
            {
                baseCtrl.statusType = Interfaces.AIStatusType.None;
                timeLiveMemory = 0;
                return;
            }
            timeLiveMemory += Time.deltaTime;
        }

        public override void DetectUser(Vector3? detectPos)
        {
            if (detectPos == null) return;
            if (timeLiveMemory != 0) timeLiveMemory = 0;
            baseCtrl.SetTargetToMove(detectPos, 0, false);
        }
    }
}
