using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Progress
{
    public class BarBaseController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform currentTf;
        [SerializeField]
        [Range(0f, 1f)]
        private float[] colorsFloat;
        [SerializeField]
        private Color[] colors;

        private Color BarColor
        {
            get
            {
                if (colors.Length == 1)
                {
                    return colors[0];
                }
                else
                {
                    int startIdx = -1, endIdx = colorsFloat.Length;
                    float val = info.GetCurrentPercent();
                    for (int i = 0; i < colorsFloat.Length; i++)
                    {
                        if (colorsFloat[i] >= val)
                        {
                            startIdx = i - 1;
                            endIdx = i;
                            break;
                        }
                    }
                    if (startIdx == -1)
                    {
                        // 걍 맨 아래색
                        return colors[0];
                    }
                    else
                    {
                        // 섞기
                        Color startCol = colors[startIdx], endCol = colors[endIdx];
                        Vector3 res = Vector3.Lerp(
                            new Vector3(startCol.r, startCol.g, startCol.b),
                            new Vector3(endCol.r, endCol.g, endCol.b)
                            , (val - colorsFloat[startIdx]) / (colorsFloat[endIdx] - colorsFloat[startIdx])
                            );
                        return new Color(res.x, res.y, res.z, 1);
                    }
                }
            }
        }

        private Image barFill;
        private ProgressInfo info;
        public float LiveInfo
        {
            set
            {
                info.CurValue += value;
                barFill.color = BarColor;
            }
            get
            {
                return (int)info.CurValue;
            }
        }
        public float LivePercent
        {
            get => info.GetCurrentPercent();
        }

        private void Awake()
        {
            if (barFill == null)
            {
                barFill = currentTf.GetComponent<Image>();
                info = new ProgressInfo(100);
                LiveInfo = +0;
            }
        }
        private void Update()
        {
            currentTf.anchorMax = new Vector2(info.GetCurrentPercent(), 1);
            currentTf.offsetMax = Vector2.zero;
        }

        public void SetValue(float maxValue, float curValue)
        {
            if (barFill == null)
            {
                barFill = currentTf.GetComponent<Image>();
            }
            info = new ProgressInfo(maxValue);
            LiveInfo = curValue - maxValue;
        }
    }
}
