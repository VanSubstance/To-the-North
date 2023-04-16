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

        private void Awake()
        {
            barFill = currentTf.GetComponent<Image>();
            info = new ProgressInfo(100);
            LiveInfo = +0;
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
            info.CurValue += value;
            if (info.CurValue < 0)
            {
                info.CurValue = 0;
                return;
            }
            if (info.CurValue > info.maxValue)
            {
                info.CurValue = info.maxValue;
                return;
            }
        }

        /// <summary>
        /// 현재 값 반환
        /// </summary>
        /// <returns></returns>
        public float GetCurrent()
        {
            return info.CurValue;
        }
    }
}
