using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Components.Progress.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Components.Progress.Controllers
{
    internal class ProgressBarController : MonoBehaviour, IProgress
    {
        [SerializeField]
        private RectTransform progressTf;
        public void OnFinish()
        {
            Debug.Log("로딩 완료");
        }

        public void SetProgress(float progress)
        {
            progressTf.anchorMax = new Vector2(progress, 1);
            progressTf.offsetMax = Vector2.zero;
        }
    }
}
