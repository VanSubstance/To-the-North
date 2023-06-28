using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class EffectManager : SingletonObject<EffectManager>
    {
        private Dictionary<EffectType, IEffectControl> effectControllers;

        private new void Awake()
        {
            base.Awake();
            effectControllers = new Dictionary<EffectType, IEffectControl>() {
                { EffectType.Vibrate, new VibrateController()},
                { EffectType.Filter, new FilterController()},
                { EffectType.Fade, new FadeController()},
            };
        }

        public void ExecuteEffect(EffectType effectType, Transform target, EffectInfo info)
        {
            StartCoroutine(effectControllers[effectType].CoroutineEffect(target, info));
        }
    }
}
