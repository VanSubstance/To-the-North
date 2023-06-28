using System.Collections;
using UnityEngine;
namespace Assets.Scripts.Effects
{
    /// <summary>
    /// 이펙트 효과 관리용 인터페이스
    /// </summary>
    public interface IEffectControl
    {
        /// <summary>
        /// 이펙트 실행용 코루틴 IEnumerator
        /// </summary>
        /// <param name="target"></param>
        /// <param name="_info"></param>
        /// <returns></returns>
        public IEnumerator CoroutineEffect(Transform target, EffectInfo _info);
    }
}
