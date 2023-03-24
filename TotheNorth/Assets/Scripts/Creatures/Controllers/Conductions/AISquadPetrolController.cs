using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Maps.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Conductions
{
    class AISquadPetrolController : AISquadConductionController
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
            if (baseController.statusType == AIStatusType.Petrol)
            {
                if (baseController.isAllActDone())
                {
                    Transform targetTf;
                    if (trackQueue.TryDequeue(out targetTf))
                    {
                        TrackBaseController trackBase = targetTf.GetComponent<TrackBaseController>();
                        if (trackBase.isToMove)
                        {
                            baseController.MoveToTarget(targetTf.position);
                        }
                        if (trackBase.isToGaze)
                        {
                            baseController.SetTargetToGaze(targetTf.position, trackBase.timeStay);
                        }
                    }
                    else
                    {
                        ReloadPetrolQueue();
                    }
                }
            }
        }

        /// <summary>
        /// 패트롤 큐 재장전
        /// </summary>
        private void ReloadPetrolQueue()
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

        public Transform[] GetPetrolTracks()
        {
            return petrolTracks;
        }
    }
}
