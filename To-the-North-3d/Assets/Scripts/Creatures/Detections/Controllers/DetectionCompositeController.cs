using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections.Controllers
{
    internal class DetectionCompositeController : MonoBehaviour
    {
        [SerializeField]
        private DetectionBaseController[] detections;
        public Transform targetTf;
        private bool isCheckingActive;
        private void Awake()
        {
            isCheckingActive = false;
        }

        private void Update()
        {
            if (isCheckingActive) return;
            StartCoroutine(CheckSightWithDelay(0.01f));
        }

        private IEnumerator CheckSightWithDelay(float delay)
        {
            isCheckingActive = true;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                foreach (DetectionBaseController detection in detections)
                {
                    targetTf = detection.CheckSight();
                }
            }
        }
    }
}
