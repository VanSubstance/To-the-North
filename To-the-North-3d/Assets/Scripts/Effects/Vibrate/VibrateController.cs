using System.Collections;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Effects.Vibrate
{
    public class VibrateController : IEffectControl
    {
        public IEnumerator CoroutineEffect<T>(Transform target, T _info)
        {
            if (_info is VibrateInfo info)
            {
                while (info.timeVib >= 0)
                {
                    info.timeVib -= Time.deltaTime;
                    target.localPosition = CalculationFunctions.DirFromAngle(Random.Range(0, 360)) * info.powerVib;
                    info.powerVib *= 0.7f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                target.localPosition = Vector3.zero;
            }
        }
    }
}
