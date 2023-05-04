using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Events.Abstracts
{
    internal abstract class AbsEventBaseController : MonoBehaviour, IEventInteraction
    {
        public bool isOnTracking = false;
        private bool isInteracted = false;

        private ParticleSystem particle;
        private float timeParticle = 0;

        protected void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            particle.Stop();
        }

        protected void Update()
        {
            CheckParticle();
        }

        private void CheckParticle()
        {
            if (timeParticle > 0)
            {
                timeParticle -= Time.deltaTime;
                return;
            }
        }

        public void StartTrackingInteraction(Transform targetTf)
        {
            if (timeParticle <= 0)
            {
                particle.Play();
            }
            timeParticle = .5f;

            if (isOnTracking) return;
            isOnTracking = true;
            StartCoroutine(StartCoroutineInteraction(targetTf));
        }

        private IEnumerator StartCoroutineInteraction(Transform targetTf)
        {
            while (isOnTracking && Vector2.Distance(targetTf.position, transform.position) <= InGameStatus.User.Detection.distanceInteraction)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                if (Input.GetKey(KeyCode.Space) && !isInteracted)
                {
                    isInteracted = true;
                    OnInteraction();
                }
            }
            isInteracted = false;
            isOnTracking = false;
        }

        public abstract void OnInteraction();
    }
}
