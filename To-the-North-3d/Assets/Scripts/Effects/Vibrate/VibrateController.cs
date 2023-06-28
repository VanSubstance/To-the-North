using System.Collections;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class VibrateController : IEffectControl
    {
        private float PowerVib(float damage)
        {
            if (damage <= 10)
            {
                return 0.2f;
            }
            if (damage < 20)
            {
                return 0.5f;
            }
            return 1f;
        }
        public IEnumerator CoroutineEffect(Transform target, EffectInfo info)
        {
            Vector3 origin = target.localPosition;
            info.power = PowerVib(info.power);
            while (info.timeLeft >= 0)
            {
                info.timeLeft -= Time.deltaTime;
                target.localPosition = origin + CalculationFunctions.DirFromAngle(Random.Range(0, 360)) * info.power;
                info.power *= 0.7f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            target.localPosition = origin;
        }
    }
}
