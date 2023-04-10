using System.Collections;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    internal class AIWanderController : AIConductionController
    {
        private new void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (baseController.statusType == AIStatusType.Wander)
            {
                if (!baseController.isAllActDone()) return;
                baseController.SetTargetToTrack(
                    new Vector2(
                        transform.position.x + Random.Range(-5f, 5f),
                        transform.position.y + Random.Range(-5f, 5f)
                        ),
                    Random.Range(0f, 3f),
                    true
                    );
            }
        }
    }
}
