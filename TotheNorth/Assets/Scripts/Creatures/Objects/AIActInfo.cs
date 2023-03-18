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
        public AIActType type;
        public float x, y, z;

        public AIMoveInfo GetMoveInfo()
        {
            return new AIMoveInfo(x, y, z);
        }

        public AIGazeInfo GetGazeInfo()
        {
            return new AIGazeInfo((int)x, y, z);
        }
    }
}
