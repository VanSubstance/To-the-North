using System.Collections;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class CameraHitEffectController : MonoBehaviour
    {
        private float timeVib = .5f, timeLeft, powerVib;
        /// <summary>
        /// 데미지에 따라 화면에 진동을 주는 함수
        /// </summary>
        public void OnHit(float damage)
        {
            if (damage <= 10)
            {
                powerVib = 0.2f;
            }
            else if (damage < 20)
            {
                powerVib = 0.5f;
            }
            else
            {
                powerVib = 1f;
            }
            if (timeLeft > 0)
            {
                timeLeft = timeVib;
            }
            else
            {
                StartCoroutine(CoroutineVibrate());
            }
        }

        private IEnumerator CoroutineVibrate()
        {
            timeLeft = timeVib;
            while (timeLeft >= 0)
            {
                timeLeft -= Time.deltaTime;
                transform.localPosition = CalculationFunctions.DirFromAngle(UnityEngine.Random.Range(0, 360)) * powerVib;
                powerVib *= 0.7f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            transform.localPosition = Vector3.zero;
            timeLeft = 0;
            powerVib = 0f;
        }
    }
}
