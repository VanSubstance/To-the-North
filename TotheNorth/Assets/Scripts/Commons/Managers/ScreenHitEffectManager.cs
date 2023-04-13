using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class ScreenHitEffectManager : MonoBehaviour
    {
        [SerializeField]
        private Transform hitPrefab;
        private ScreenHitEffectController[] hits;

        private void Awake()
        {
            hits = new ScreenHitEffectController[10];
            for (int i = 0; i < 10; i++)
            {
                hits[i] = Instantiate(hitPrefab, transform).GetComponent<ScreenHitEffectController>();
            }
        }

        /// <summary>
        /// 화면 테두리에 피격 이펙트 띄우는 함수
        /// </summary>
        /// <param name="degree">화면 중심 기준 이펙트 띄울 방향의 각도 (right = 0, 반시계 방향)</param>
        public void OnHit(float degree)
        {
            foreach (ScreenHitEffectController hit in hits)
            {
                if (!hit.gameObject.activeSelf)
                {
                    hit.Execute(degree);
                    return;
                }
            }
        }
    }
}
