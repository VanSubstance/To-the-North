using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    class AISquadConductionController : MonoBehaviour
    {
        protected AISquadBaseController baseController;
        protected void Awake()
        {
            baseController = GetComponent<AISquadBaseController>();
        }

        public AISquadBaseController GetAIBase()
        {
            return baseController;
        }
    }
}
