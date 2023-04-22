namespace Assets.Scripts.Creatures
{
    public interface ICreatureAction
    {
        /// <summary>
        /// 앉았을 때 실행되는 함수
        /// </summary>
        public void Crouch();

        /// <summary>
        /// 설 때 실행되는 함수
        /// </summary>
        public void Stand();

        /// <summary>
        /// 구를 때 실행되는 함수
        /// </summary>
        /// <param name="dir">구를 방향 (상대 좌표)</param>
        public void Dodge(UnityEngine.Vector3 dir);
    }
}
