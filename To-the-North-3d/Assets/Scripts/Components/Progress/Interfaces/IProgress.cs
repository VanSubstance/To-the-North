namespace Assets.Scripts.Components.Progress
{
    internal interface IProgress
    {
        /// <summary>
        /// 현재 progress 설정
        /// </summary>
        /// <param name="progress">
        /// 0 ~ 1 사이
        /// </param>
        public void SetProgress(float progress);
        /// <summary>
        /// 종료하엿을 때 함수
        /// </summary>
        public void OnFinish();
    }
}
