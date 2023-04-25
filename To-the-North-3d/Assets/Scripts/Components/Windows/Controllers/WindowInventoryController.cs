using Assets.Scripts.Commons.Constants;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Components.Windows
{
    public class WindowInventoryController : WindowBaseController
    {
        [SerializeField]
        private Transform left, center, right;
        private TextMeshProUGUI titleL, titleC, titleR;
        private Transform contL, contC, contR;

        protected new void Awake()
        {
            base.Awake();
            ConnectContainer(left, out titleL, out contL);
            ConnectContainer(right, out titleR, out contR);
            ConnectContainer(center, out titleC, out contC);
        }

        /// <summary>
        /// 각 구역의 제목, 컨텐츠를 변수와 연결하는 함수 (최초 연결용)
        /// </summary>
        /// <param name="side">실제 구역 Transform</param>
        /// <param name="title">제목 UGUI</param>
        /// <param name="cont">컨텐츠 Transform</param>
        private void ConnectContainer(Transform side, out TextMeshProUGUI title, out Transform cont)
        {
            title = side.GetChild(0).GetComponent<TextMeshProUGUI>();
            cont = side.GetChild(1);
        }
    }
}
