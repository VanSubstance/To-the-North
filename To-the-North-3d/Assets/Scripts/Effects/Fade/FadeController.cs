using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Effects
{
    public class FadeController : IEffectControl
    {
        public IEnumerator CoroutineEffect(Transform target, EffectInfo _info)
        {
            if (_info is FadeInfo info)
            {
                info.actionBefore?.Invoke();
                float timePass = 0, curOp;
                SpriteRenderer spr = target.GetComponent<SpriteRenderer>();
                Image img = target.GetComponent<Image>();
                while (info.timeLeft >= timePass)
                {
                    curOp = info.start + (info.end - info.start) * timePass / info.timeLeft;
                    timePass += Time.deltaTime;
                    if (spr != null) spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, curOp);
                    if (img != null) img.color = new Color(img.color.r, img.color.g, img.color.b, curOp);
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                info.actionAfter?.Invoke();
            }
        }
    }
}
