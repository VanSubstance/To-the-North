using System.Collections;
using UnityEngine;
namespace Assets.Scripts.Effects
{
    public interface IEffectControl
    {
        public IEnumerator CoroutineEffect<T>(Transform target, T _info);
    }
}
