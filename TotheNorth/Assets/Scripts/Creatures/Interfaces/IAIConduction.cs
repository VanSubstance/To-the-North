using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Creatures.Interfaces
{
    internal interface IAIConduction
    {
        /// <summary>
        /// 행동강령 초기화
        /// </summary>
        public void InitConduction();
        /// <summary>
        /// 다음 단순 행동 실행
        /// </summary>
        public void ActNext();
    }
}
