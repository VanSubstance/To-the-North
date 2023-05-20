using Assets.Scripts.Commons;
using UnityEngine;

namespace Assets.Scripts.Components.Windows
{
    public abstract class WindowCenterBaseController : MonoBehaviourControllByKey
    {
        [SerializeField]
        private Vector2 sizeToRatio;

        protected void Awake()
        {
            GetComponent<RectTransform>().sizeDelta = sizeToRatio * 120;
            MouseInteractionController.AttachMouseInteractor<MouseInteractionMoveableController>(transform);
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
