using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 근거리 무기용 컨트롤러
    /// </summary>
    internal class ItemMeleeController : MonoBehaviour, IItemHandable
    {
        public void Aim(Vector3 targetDir)
        {
            Debug.Log("근거리 조준중 ...");
        }

        public void Use(Vector3 targetDir)
        {
            Debug.Log("근거리 사용중 ...");
        }

        public float Range()
        {
            return 0f;
        }
    }
}
