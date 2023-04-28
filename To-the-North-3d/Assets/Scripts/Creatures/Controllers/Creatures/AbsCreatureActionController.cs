using System;
using Assets.Scripts.Commons;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public abstract class AbsCreatureActionController : MonoBehaviour, ICreatureAction, ISoundable
    {
        [SerializeField]
        private Transform detectionTf, sightTf, handTf;

        protected bool isCrouching = false;

        [HideInInspector]
        private AudioSource Speaker;
        [SerializeField]
        protected AudioClip
            audWalk, audRun;

        protected void Awake()
        {
            Speaker = gameObject.AddComponent<AudioSource>();
            Speaker.loop = true;
            Speaker.playOnAwake = false;
            Speaker.Stop();
        }

        public void Crouch()
        {
            if (isCrouching) return;
            isCrouching = true;
            detectionTf.localPosition = Vector3.up * (-.6f + detectionTf.localPosition.y);
            sightTf.localPosition = Vector3.up * (-.4f + sightTf.localPosition.y);
            handTf.localPosition = Vector3.up * (-.6f + handTf.localPosition.y);
        }

        public void Stand()
        {
            if (!isCrouching) return;
            isCrouching = false;
            detectionTf.localPosition = Vector3.up * (.6f + detectionTf.localPosition.y);
            sightTf.localPosition = Vector3.up * (.4f + sightTf.localPosition.y);
            handTf.localPosition = Vector3.up * (.6f + handTf.localPosition.y);
        }

        public abstract void Dodge(Vector3 dir);

        public void PlaySound(AudioClip _clip = null)
        {
            if (_clip != null)
            {
                Speaker.clip = _clip;
            }
            Speaker.Play();
        }

        public void PlaySoundByType(SoundType _type)
        {
            if (Speaker == null) return;
            if (_type.Equals(IsSoundInPlaying())) return;
            switch (_type)
            {
                case SoundType.Walk:
                    PlaySound(audWalk);
                    break;
                case SoundType.Run:
                    PlaySound(audRun);
                    break;
                case SoundType.None:
                    StopSound();
                    break;
            }
        }

        public SoundType IsSoundInPlaying()
        {
            if (Speaker == null) return SoundType.None;
            if (!Speaker.isPlaying) return SoundType.None;
            AudioClip c = Speaker.clip;
            if (c.Equals(audWalk)) return SoundType.Walk;
            if (c.Equals(audRun)) return SoundType.Run;
            return SoundType.None;
        }

        public void StopSound()
        {
            if (Speaker == null) return;
            Speaker.Stop();
        }
    }
}
