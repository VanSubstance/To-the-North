namespace Assets.Scripts.Components.Progress
{
    public class ProgressInfo
    {
        public float maxValue, curValue;
        public ProgressInfo(float _maxValue)
        {
            maxValue = _maxValue;
            curValue = _maxValue;
        }

        public float GetCurrentPercent()
        {
            return curValue / maxValue;
        }
    }
}
