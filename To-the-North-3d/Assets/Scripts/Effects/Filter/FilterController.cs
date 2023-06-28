using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Effects
{
    public class FilterController : IEffectControl
    {
        private float Power(float damage)
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
        public IEnumerator CoroutineEffect(Transform target, EffectInfo _info)
        {
            if (_info is FilterInfo info)
            {
                info.power = Power(info.power);
                info.color.a = 1;
                SpriteRenderer spr = target.GetComponent<SpriteRenderer>();
                Image img = target.GetComponent<Image>();
                while (info.timeLeft >= 0)
                {
                    info.timeLeft -= Time.deltaTime;
                    if (spr != null) spr.color = info.color * info.power;
                    if (img != null) img.color = info.color * info.power;
                    info.power *= .7f;
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }
        }
    }
}
