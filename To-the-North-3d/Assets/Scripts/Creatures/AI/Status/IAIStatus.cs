using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.AI.Status
{
    public interface IAIStatus
    {
        /// <summary>
        /// 매 프레임 별로 호출되는 AI 행동 업데이트 함수
        /// </summary>
        /// <param name="target">타겟 좌표 (절대 좌표)</param>
        public void UpdateAction(AIBaseController mover, Vector3? target);
    }
}
