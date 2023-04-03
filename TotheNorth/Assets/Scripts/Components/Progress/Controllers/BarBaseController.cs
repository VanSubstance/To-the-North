using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Components
{
    class BarBaseController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform currentTf;
        [SerializeField]
        [Range(0, 1)]
        private float test;
        [SerializeField]
        private Color barColor;

        private void Awake()
        {
            currentTf.GetComponent<Image>().color = barColor;
        }

        private void Update()
        {
            SetCurrent(test);
        }

        public void SetCurrent(float current)
        {
            currentTf.anchorMax = new Vector2(current, 1);
            currentTf.offsetMax = Vector2.zero;
        }
    }
}
