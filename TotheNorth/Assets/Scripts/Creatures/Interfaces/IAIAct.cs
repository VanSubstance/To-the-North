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
        public bool IsExecutable();
        /// <summary>
        /// 단순 행동 실행
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        public void ExecuteAct(AIActInfo info);
        /// <summary>
        /// 현재 행동을 일시정지/재개
        /// </summary>
        /// <param name="isPause"></param>
        public void PauseOrResumeAct(bool isPause);
        public void Move(AIMoveInfo info);
        public void Gaze(AIGazeInfo info);
    }
}
