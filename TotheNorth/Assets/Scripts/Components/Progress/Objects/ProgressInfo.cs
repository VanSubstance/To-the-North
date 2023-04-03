namespace Assets.Scripts.Components.Progress
{
    public class ProgressInfo
    {
        public int maxValue, curValue;
        public ProgressInfo(int _maxValue)
        {
            maxValue = _maxValue;
            curValue = _maxValue;
        }

        public float GetCurrentPercent()
        {
            return curValue / (float)maxValue;
        }
    }
}
