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
        /// <summary>
        /// 단순 행동 실행
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public void ExecuteAct(AIActInfo info);
        public void Move(AIMoveInfo info);
        public void Gaze(AIGazeInfo info);
    }
}
