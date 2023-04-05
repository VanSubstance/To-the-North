using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections.Controllers
{
    internal class DetectionCompositeController : MonoBehaviour
    {
        [SerializeField]
        private DetectionBaseController[] detections;

        private void Update()
        {
            StartCoroutine(CheckSightWithDelay(0.01f));
        }

        private IEnumerator CheckSightWithDelay(float delay)
        {
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
