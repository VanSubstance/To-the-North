using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public interface ISoundable
    {
        /// <summary>
        /// 소리 재생 함수
        /// </summary>
        /// <param name="_clip">재생할 소리:
        /// null이 기본값임:
        /// null 일 경우, 기존 소리 클립 재생</param>
        public void PlaySound(AudioClip _clip = null);
        public void StopSound();
        public void PlaySoundByType(SoundType _type);
        public SoundType IsSoundInPlaying();
    }
}
