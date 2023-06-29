using System.Collections;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    public class DetectionCompositeController : MonoBehaviour
    {
        [SerializeField]
        private DetectionBaseController[] detections;
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
                    detection.CheckSight();
                }
            }
        }
    }
}
