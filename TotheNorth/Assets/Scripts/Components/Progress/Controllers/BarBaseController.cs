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

        public void AddCurrent(int value)
        {
            info.curValue += value;
        }
    }
}
