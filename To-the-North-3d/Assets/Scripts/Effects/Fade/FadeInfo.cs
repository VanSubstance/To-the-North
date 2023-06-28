namespace Assets.Scripts.Effects
{
    public class FadeInfo: EffectInfo
    {
        public float start = 1, end = 0;
        public System.Action actionAfter, actionBefore;
    }
}
