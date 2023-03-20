using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures.Objects
{
    [System.Serializable]
    internal class AIActInfo
    {
        public AIMoveInfo moveInfo;
        public AIGazeInfo gazeInfo;

        public AIActInfo()
        {
        }

        public AIActInfo(AIActInfo info)
        {
            moveInfo = info.moveInfo;
            gazeInfo = info.gazeInfo;
        }

        public AIActInfo(AIMoveInfo moveInfo, AIGazeInfo gazeInfo)
        {
            this.moveInfo = moveInfo;
            this.gazeInfo = gazeInfo;
        }

        public AIMoveInfo GetMoveInfo()
        {
            return moveInfo;
        }

        public AIGazeInfo GetGazeInfo()
        {
            return gazeInfo;
        }
    }
}
