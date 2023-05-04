using UnityEngine;

namespace Assets.Scripts.SoundEffects
{
    public class SoundEffectController : MonoBehaviour
    {
        private AudioSource Speaker;
        public float impactDistance;

        public bool isAsleep
        {
            get
            {
                return !gameObject.activeSelf;
            }
        }

        public void Init()
        {
            if (Speaker != null) return;
            Speaker = GetComponent<AudioSource>();
            Speaker.loop = false;
            Speaker.playOnAwake = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 효과음 재생 함수
        /// </summary>
        /// <param name="_clip"></param>
        public void PlaySound(Vector3 posWorld, AudioClip _clip, float _impactDistance)
        {
            Init();
            transform.position = posWorld;
            Speaker.clip = _clip;
            Speaker.maxDistance = _impactDistance;
            gameObject.SetActive(true);
            impactDistance = _impactDistance;
        }

        private void Update()
        {
            if (!Speaker.isPlaying)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
