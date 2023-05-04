using Assets.Scripts.SoundEffects;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Controllers
{
    public class AISoundMoveController : MonoBehaviour
    {
        private AudioSource Speaker;
        [SerializeField]
        protected AudioClip
            audWalk, audRun;

        private NavMeshAgent agent;

        protected void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            SoundEffectManager.AddAudioSource(transform, true, out Speaker);
        }

        private void Update()
        {
            CheckSound();
        }

        private void CheckSound()
        {
            if (agent.remainingDistance < 0.01f)
            {
                Speaker.Stop();
                Speaker.clip = null;
                Speaker.maxDistance = 10;
                return;
            }
            if (agent.speed < 3)
            {
                if (Speaker.clip == null || !Speaker.clip.Equals(audWalk))
                {
                    Speaker.clip = audWalk;
                    Speaker.maxDistance = 15;
                }
                if (!Speaker.isPlaying)
                {
                    Speaker.Play();
                }
                return;
            }
            if (agent.speed >= 3)
            {
                if (Speaker.clip == null || !Speaker.clip.Equals(audRun))
                {
                    Speaker.clip = audRun;
                    Speaker.maxDistance = 20;
                }
                if (!Speaker.isPlaying)
                {
                    Speaker.Play();
                }
                return;
            }
        }
    }
}
