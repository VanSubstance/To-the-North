public interface IControllByKey
{
    /// <summary>
    /// 윈도우 열기/닫기용 함수
    /// </summary>
    /// <param name="purpose">0 <- 그냥 토글, 1 <- 열기, 2 <- 닫기</param>
    public void ControllByKey(int purpose);
}