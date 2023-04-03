using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Progress
{
    public class BarBaseController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform currentTf;
        [SerializeField]
        private Color barColor;

        private ProgressInfo info;

        private void Awake()
        {
            currentTf.GetComponent<Image>().color = barColor;
            info = new ProgressInfo(100);
        }
        private void Update()
        {
            currentTf.anchorMax = new Vector2(info.GetCurrentPercent(), 1);
            currentTf.offsetMax = Vector2.zero;
        }

        /// <summary>
        /// 수치 변화
        /// </summary>
        /// <param name="value">더할 값</param>
        public void AddCurrent(float value)
        {
            info.curValue += value;
            if (info.curValue < 0)
            {
                info.curValue = 0;
            }
        }

        /// <summary>
        /// 현재 값 반환
        /// </summary>
        /// <returns></returns>
        public float GetCurrent()
        {
            return info.curValue;
        }
    }
}
