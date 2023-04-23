namespace Assets.Scripts.Creatures
{
    public interface IInteractionWithSight
    {
        public void DetectFull();
        public void DetectHalf();
        public void DetectNone();
    }
}
