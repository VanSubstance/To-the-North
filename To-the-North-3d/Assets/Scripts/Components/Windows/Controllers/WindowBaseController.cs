using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Components.Windows
{
    public class WindowBaseController : MonoBehaviourControllByKey
    {
        [SerializeField]
        private Vector2 marginToRatio;

        protected void Awake()
        {
            GetComponent<RectTransform>().sizeDelta = marginToRatio * -120 * 2;
        }

        protected void OnEnable()
        {
            // 이동 잠금
            InGameStatus.User.isPause = true;
        }

        protected void OnDisable()
        {
            // 이동 해금
            InGameStatus.User.isPause = false;
        }
    }
}
