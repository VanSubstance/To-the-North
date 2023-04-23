namespace Assets.Scripts.Creatures
{
    public interface ICreatureInteractionWithSight
    {
        public void DetectFull();
        public void DetectHalf();
        public void DetectNone();
    }
}
