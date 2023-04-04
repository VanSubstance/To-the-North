using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 들고 있을 때 호출될 함수 인터페이스
    /// </summary>
    public interface IItemHandable
    {
        public void Use(Vector3 targetDir);
        public void Aim(Vector3 targetDir);
    }
}
