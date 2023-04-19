namespace Assets.Scripts.Components.Progress
{
    public class ProgressInfo
    {
        public float maxValue;
        private float curValue;
        public float CurValue
        {
            get
            {
                return curValue;
            }
            set
            {
                curValue = value > maxValue ?
                    maxValue :
                    value < 0 ?
                        0 :
                        value
                    ;
            }
        }
        public ProgressInfo(float _maxValue)
        {
            maxValue = _maxValue;
            CurValue = _maxValue;
        }

        public float GetCurrentPercent()
        {
            return CurValue / maxValue;
        }
    }
}
