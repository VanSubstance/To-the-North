using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Objects;

namespace Assets.Scripts.Creatures.Interfaces
{
    internal interface IAIAct
    {
        public void Move(AIMoveInfo info);
        public void Gaze(AIGazeInfo info);
    }
}
