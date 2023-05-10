using System.Collections;
using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Events.Abstracts
{
    public abstract class AbsEventBaseController : MonoBehaviour, IEventInteraction, IInteractionWithSight
    {
        [SerializeField]
        private SpriteRenderer spr;

        private float SpriteOpacity
        {
            set
            {
                spr.color = new Color(1, 1, 1, value);
            }
        }

        [SerializeField]
        private ParticleSystem particle;

        private bool isOnTracking = false;
        private bool isInteracted = false;

        protected void Awake()
        {
            if (particle.isPlaying)
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

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

        public void DetectFull()
        {
            SpriteOpacity = 1;
            if (particle.isPlaying)
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void DetectHalf()
        {
            SpriteOpacity = .5f;
            if (particle.isPlaying)
                particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void DetectNone()
        {
            SpriteOpacity = 0;
            if (!particle.isPlaying)
                particle.Play();
        }

        public void DetectSound(Vector3 _pos)
        {
        }
    }
}
