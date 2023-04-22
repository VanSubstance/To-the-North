using UnityEngine;
using static Assets.Scripts.Commons.Constants.InGameStatus.User.Detection;

namespace Assets.Scripts.Commons
{
    public class TiltForSightController : MonoBehaviour
    {
        [SerializeField]
        private float height;
        [SerializeField]
        private bool isDig = false;

        private void Awake()
        {
            height /= 2f;
            transform.localRotation = Quaternion.Euler(90, 0, 0);
            Vector3 posOrigin = transform.localPosition;
            transform.localPosition = new Vector3(
                posOrigin.x,
                0,
                !isDig ? (height) : (posOrigin.z)
                );
        }
    }
}
