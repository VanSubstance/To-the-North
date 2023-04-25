using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class WindowInventoryController : WindowBaseController
    {
        [SerializeField]
        private Transform containerBlank, containerSlots;
        //private Dictionary<ContainerType, ContainerBaseController<ContentBaseController>> containerByType;
        private Dictionary<ContentType, ContentBaseController> contentByType;
        private Dictionary<Side, ContentBaseController> contentsVisual;

        private Transform visualTf, storeTf;

        protected new void Awake()
        {
            base.Awake();
            visualTf = transform.GetChild(0);
            storeTf = transform.GetChild(1);

            contentByType = new Dictionary<ContentType, ContentBaseController>();

            // 최초: 모든 컨테이너는 빈 컨테이너로 연결
            contentByType[ContentType.None_L] = Instantiate(containerBlank, visualTf).GetComponent<ContainerBaseController>().GetContent<ContentBlankController>();
            contentByType[ContentType.None_C] = Instantiate(containerBlank, visualTf).GetComponent<ContainerBaseController>().GetContent<ContentBlankController>();
            contentByType[ContentType.None_R] = Instantiate(containerBlank, visualTf).GetComponent<ContainerBaseController>().GetContent<ContentBlankController>();
            contentsVisual = new Dictionary<Side, ContentBaseController>()
            {
                {
                    Side.L, contentByType[ContentType.None_L]
                },
                {
                    Side.C, contentByType[ContentType.None_C]
                },
                {
                    Side.R, contentByType[ContentType.None_R]
                },
            };

            // 풀링: 각각의 컨테이너 생성 및 보관
            contentByType[ContentType.Inventory] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>("인벤토리");
            contentByType[ContentType.Rooting] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>("루팅");

            /**
             * 테스트
             * I키 -> 인벤토리
             * L : 루팅 | C: 장착 | R: 인벤토리(배낭)
             */
            CallContent(Side.L, ContentType.Rooting);
            CallContent(Side.R, ContentType.Inventory);
        }

        /// <summary>
        /// 해당 사이드에 해당 컨텐츠 컨테이너를 불러오는 함수
        /// </summary>
        /// <param name="side">목표 사이드</param>
        /// <param name="_targetType">목표 타입</param>
        private void CallContent(Side side, ContentType _targetType)
        {
            contentsVisual[side].Container.SetParent(storeTf);
            (contentsVisual[side] = contentByType[_targetType]).Container.SetParent(visualTf);
            contentsVisual[side].Container.SetSiblingIndex((int)side);
        }

        private enum Side
        {
            L, C, R
        }
    }
}
