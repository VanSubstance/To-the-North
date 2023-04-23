using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class DestroyController : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
