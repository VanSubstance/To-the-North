using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 들고 있을 때 호출될 함수 인터페이스
    /// </summary>
    public interface IItemHandable
    {
        /// <summary>
        /// 해당 아이템 사거리 반환
        /// </summary>
        /// <returns></returns>
        public float Range();
        /// <summary>
        /// 해당 아이템 사용
        /// </summary>
        /// <param name="targetDir"></param>
        public void Use(Vector3 targetDir);
        /// <summary>
        /// 해당 아이템 조준
        /// </summary>
        /// <param name="targetDir"></param>
        public void Aim(Vector3 targetDir);
    }
}
