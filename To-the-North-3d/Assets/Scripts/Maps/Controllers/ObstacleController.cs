using UnityEngine;

namespace Assets.Scripts.Maps
{
    internal class ObstacleController : MonoBehaviour
    {
        [SerializeField]
        private Transform imageToTilt;
        private float height;
        private static float sq2 = Mathf.Sqrt(2);

        private void Awake()
        {
            height = imageToTilt.position.y;
            imageToTilt.localRotation = Quaternion.Euler(45, 0, 0);
            Vector3 posOrigin = imageToTilt.localPosition;
            imageToTilt.localPosition = new Vector3(
                posOrigin.x,
                height / sq2,
                posOrigin.z + height / sq2
                );
        }
    }
}
