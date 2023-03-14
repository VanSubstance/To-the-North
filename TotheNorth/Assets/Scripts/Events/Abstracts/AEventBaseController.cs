using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Events.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Events.Abstracts
{
    internal abstract class AEventBaseController : MonoBehaviour, IEventInteraction
    {
        public bool isOnTracking = false;

        public void StartTrackingInteraction(Transform targetTf)
        {
            if (isOnTracking) return;
            isOnTracking = true;
            StartCoroutine(StartCoroutineInteraction(targetTf));
        }

        private IEnumerator StartCoroutineInteraction(Transform targetTf)
        {
            while (isOnTracking && Vector2.Distance(targetTf.position, transform.position) <= InGameStatus.User.Detection.distanceInteraction)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                if (Input.GetKey(KeyCode.Space))
                {
                    isOnTracking = false;
                    OnInteraction();
                }
            }
            isOnTracking = false;
        }

        public abstract void OnInteraction();
    }
}
