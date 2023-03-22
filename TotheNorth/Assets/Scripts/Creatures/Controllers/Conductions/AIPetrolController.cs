using System.Collections.Generic;
using Assets.Scripts.Creatures.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIPetrolController : AAIConductionController
    {
        [SerializeField]
        private Transform[] petrolTracks;
        private Queue<Transform> trackQueue;
        private new void Awake()
        {
            base.Awake();
            trackQueue = new Queue<Transform>();
            ReloadPetrolQueue();
        }

        private void Update()
        {
            if (baseController.isAllActDone())
            {
                Transform targetTf;
                if (trackQueue.TryDequeue(out targetTf))
                {
                    baseController.targetToMove = targetTf;
                    baseController.targetToGaze = targetTf;
                }
                else
                {
                    ReloadPetrolQueue();
                }
            }
        }

        /// <summary>
        /// 패트롤 큐 재장전
        /// </summary>
        public void ReloadPetrolQueue()
        {
            if (Vector2.Distance(transform.position, petrolTracks[0].position) < Vector2.Distance(transform.position, petrolTracks[petrolTracks.Length - 1].position))
            {
                for (int i = 0; i < petrolTracks.Length; i++)
                {
                    trackQueue.Enqueue(petrolTracks[i]);
                }
            }
            else
            {
                for (int i = petrolTracks.Length - 1; i >= 0; i--)
                {
                    trackQueue.Enqueue(petrolTracks[i]);
                }
            }
        }
    }
}
