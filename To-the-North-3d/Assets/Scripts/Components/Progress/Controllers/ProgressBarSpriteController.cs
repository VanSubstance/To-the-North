using UnityEngine;

namespace Assets.Scripts.Components.Progress
{
    public class ProgressBarSpriteController : MonoBehaviour
    {
        private float curProgress;
        private Transform barToControll;
        public float CurProgress
        {
            set
            {
                curProgress = value > 1 ? 1 : value < 0 ? 0 : value;
                if (curProgress >= 1 || curProgress <= 0)
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (!gameObject.activeSelf && 
                    (curProgress > 0 || curProgress < 1))
                {
                    gameObject.SetActive(true);
                }
                Init();
                barToControll.localPosition = Vector3.left * (1 - curProgress) / 2;
                barToControll.localScale = new Vector3(curProgress, .1f, 1f);
            }
            get
            {
                return curProgress;
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (barToControll != null) return;
            barToControll = transform.GetChild(1);
            CurProgress = 0;
        }
    }
}
