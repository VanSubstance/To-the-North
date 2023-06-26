
namespace Assets.Scripts.Commons
{
    public interface IControllByKey
    {
        /// <summary>
        /// 윈도우 열기/닫기용 함수
        /// </summary>
        /// <param name="purpose">0 <- 그냥 토글, 1 <- 열기, 2 <- 닫기</param>
        public void ControllByKey(int purpose);

        /// <summary>
        /// 윈도우 열기 직전에 실행될 함수
        /// </summary>
        public void OnOpen();

        /// <summary>
        /// 윈도우 닫은 직후 실행될 함수
        /// </summary>
        public void OnClose();

        /// <summary>
        /// 열려있는지 확인하는 함수
        /// </summary>
        /// <returns></returns>
        public bool IsOpen();

        /// <summary>
        /// 강제 닫기 함수
        /// </summary>
        public void Close();
    }
}