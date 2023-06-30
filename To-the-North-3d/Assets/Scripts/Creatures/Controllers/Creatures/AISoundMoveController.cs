using Assets.Scripts.SoundEffects;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Controllers
{
    public class AISoundMoveController : MonoBehaviour
    {
        private AudioSource Speaker;
        [SerializeField]
        private CreatureType creatureType;
        private SoundType curSoundType;

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
                curSoundType = SoundType.None;
                Speaker.Stop();
                Speaker.clip = null;
                Speaker.maxDistance = 10;
                return;
            }
            if (agent.speed < 3)
            {
                if (Speaker.clip == null || !curSoundType.Equals(SoundType.Walk))
                {
                    curSoundType = SoundType.Walk;
                    Speaker.clip = GlobalDictionary.Sound.Move[creatureType].Walk;
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
                if (Speaker.clip == null || !curSoundType.Equals(SoundType.Run))
                {
                    curSoundType = SoundType.Run;
                    Speaker.clip = GlobalDictionary.Sound.Move[creatureType].Run;
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
