using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Creatures.Objects
{
    [System.Serializable]
    internal class AIGazeInfo
    {
        public int degree;
        public float secWait, spdTurn;

        public AIGazeInfo()
        {
        }

        public AIGazeInfo(int degree, float secWait, float spdTurn)
        {
            this.degree = degree;
            this.secWait = secWait;
            this.spdTurn = spdTurn;
        }

        public AIGazeInfo(AIGazeInfo info)
        {
            this.degree = info.degree;
            this.secWait = info.secWait;
            this.spdTurn = info.spdTurn;
        }
    }
}
