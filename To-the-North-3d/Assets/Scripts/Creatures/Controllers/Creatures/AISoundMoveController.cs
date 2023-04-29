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
            Speaker = gameObject.AddComponent<AudioSource>();
            Speaker.loop = true;
            Speaker.playOnAwake = false;
            Speaker.Stop();
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
                return;
            }
            if (agent.speed < 3)
            {
                if (Speaker.clip == null || !Speaker.clip.Equals(audWalk))
                {
                    Speaker.clip = audWalk;
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
