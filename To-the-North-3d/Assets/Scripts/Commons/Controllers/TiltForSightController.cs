using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class TiltForSightController : MonoBehaviour
    {
        [SerializeField]
        private float height;
        private static float sq2 = Mathf.Sqrt(2);

        private void Awake()
        {
            height = transform.position.y;
            transform.localRotation = Quaternion.Euler(45, 0, 0);
            Vector3 posOrigin = transform.localPosition;
            transform.localPosition = new Vector3(
                posOrigin.x,
                height / sq2,
                posOrigin.z + height / sq2
                );
        }
    }
}
