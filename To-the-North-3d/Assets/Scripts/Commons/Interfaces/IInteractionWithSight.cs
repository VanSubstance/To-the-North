using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public interface IInteractionWithSight
    {
        /// <summary>
        /// 유저의 시야에 완전 식별되었을 때 = 윗 시야에 걸렸을 때 실행하는 함수
        /// </summary>
        public void DetectFull();
        /// <summary>
        /// 유저의 시야에 절반 식별되었을 때 = 아랫시야에만 걸렸을 때 실행하는 함수
        /// </summary>
        public void DetectHalf();
        /// <summary>
        /// 유저의 시야에서 벗어났을 때 실행하는 함수
        /// </summary>
        /// 
        public void DetectNone();
        /// <summary>
        /// 소리를 감지하였을 때 실행되는 함수
        /// </summary>
        /// <param name="_pos">감지된 소리의 근원지</param>
        public void DetectSound(Vector3 _pos);
    }
}
