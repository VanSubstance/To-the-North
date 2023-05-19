using UnityEngine;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Components.Windows
{
    public abstract class WindowFlexBaseController : MonoBehaviourControllByKey
    {
        [SerializeField]
        private Vector2 sizeToRatio, initPositionToRatio;

        protected void Awake()
        {
            GetComponent<RectTransform>().sizeDelta = sizeToRatio * GlobalSetting.UnitSize;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(initPositionToRatio.x, -initPositionToRatio.y) * GlobalSetting.UnitSize;
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
