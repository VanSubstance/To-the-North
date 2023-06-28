using System.Collections;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Effects.Vibrate
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
        public IEnumerator CoroutineEffect<T>(Transform target, T _info)
        {
            if (_info is VibrateInfo info)
            {
                Vector3 origin = target.localPosition;
                info.powerVib = PowerVib(info.powerVib);
                while (info.timeVib >= 0)
                {
                    info.timeVib -= Time.deltaTime;
                    target.localPosition = origin + CalculationFunctions.DirFromAngle(Random.Range(0, 360)) * info.powerVib;
                    info.powerVib *= 0.7f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                target.localPosition = origin;
            }
        }
    }
}
