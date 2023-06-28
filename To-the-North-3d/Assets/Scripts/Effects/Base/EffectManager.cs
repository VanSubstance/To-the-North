using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Effects.Vibrate;

namespace Assets.Scripts.Effects
{
    public class EffectManager : SingletonObject<EffectManager>
    {
        private Dictionary<EffectType, IEffectControl> effectControllers;

        private new void Awake()
        {
            base.Awake();
            effectControllers = new Dictionary<EffectType, IEffectControl>() {
                { EffectType.Vibrate, new VibrateController()}
            };
        }

        public void ExecuteEffect<T>(EffectType effectType, Transform target, T info)
        {
            StartCoroutine(effectControllers[effectType].CoroutineEffect(target, info));
        }
    }
}
