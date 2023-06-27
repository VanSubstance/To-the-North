using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    /// <summary>
    /// 필터 모듈
    /// </summary>
    public class ScreenHitFilterController : MonoBehaviour
    {
        private SpriteRenderer image;
        private float timeVib = .5f, timeLeft;
        private float powerRed
        {
            set
            {
                image.color = new Color(value, 0, 0, .25f);
            }
            get
            {
                return image.color.r;
            }
        }

        private void Awake()
        {
            image = GetComponent<SpriteRenderer>();
        }
        /// <summary>
        /// 데미지에 따라 화면 필터의 색을 변결하는 함수
        /// </summary>
        public void OnHit(float damage)
        {
            if (damage <= 10)
            {
                powerRed = 0.6f;
            }
            else if (damage < 20)
            {
                powerRed = 0.8f;
            }
            else
            {
                powerRed = 1f;
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
                powerRed *= 0.7f;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            timeLeft = 0;
            powerRed = 0;
        }
    }
}
