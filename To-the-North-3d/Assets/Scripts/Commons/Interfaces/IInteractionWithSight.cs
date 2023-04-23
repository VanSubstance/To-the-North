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
        public void DetectNone();
    }
}
