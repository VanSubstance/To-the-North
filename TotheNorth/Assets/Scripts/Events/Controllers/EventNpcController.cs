using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventNpcController : AEventBaseController
    {
        private AAIBaseController baseController;
        private void Awake()
        {
            baseController = GetComponent<AAIBaseController>();
        }
        public override void OnInteraction()
        {
            Debug.Log("Npc 상호작용");
            baseController.PauseOrResumeAct(true);
        }
    }
}
