using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class WindowInventoryController : WindowBaseController
    {
        [SerializeField]
        private Transform containerBlank, containerSlots, itemPrefab;
        private static Dictionary<ContentType, ContentBaseController> contentByType;
        private static Dictionary<Side, ContentBaseController> contentsVisual;

        private Transform visualTf, storeTf, itemTf;
        public Transform ItemTf
        {
            get
            {
                return itemTf;
            }
        }
        public static InventorySlotController[,] InventorySlots
        {
            get
            {
                return ((ContentSlotController)contentByType[ContentType.Inventory]).slots;
            }
        }
        public static InventorySlotController[,] LootSlots
        {
            get
            {
                return ((ContentSlotController)contentByType[ContentType.Looting]).slots;
            }
        }

        public static List<ItemBaseInfo> items = new List<ItemBaseInfo>();
        public static List<ItemBaseInfo> rootItems = new List<ItemBaseInfo>();

        private static WindowInventoryController _instance;
        public static WindowInventoryController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(WindowInventoryController)) as WindowInventoryController;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        protected new void Awake()
        {
            base.Awake();
            visualTf = transform.GetChild(0);
            storeTf = transform.GetChild(1);
            itemTf = transform.GetChild(2);

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
            contentByType[ContentType.Inventory] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>(ContentType.Inventory);
            contentByType[ContentType.Looting] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>(ContentType.Looting);


            // 테스트용 아이템들 생성
            testItemInit();

            /**
             * 테스트
             * I키 -> 인벤토리
             * L : 루팅 | C: 장착 | R: 인벤토리(배낭)
             */
            CallContent(Side.L, ContentType.Looting);
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

        /// <summary>
        /// 테스트용 아이템 객체들 생성
        /// </summary>
        private void testItemInit()
        {
            foreach (ItemInventoryInfo info in InGameStatus.Item.inventory)
            {
                GenerateItemObject(ContentType.Inventory, info);
            }
        }

        /// <summary>
        /// 아이템 오브젝트 생성 함수
        /// </summary>
        /// <param name="_type">해당 컨테이너: 인벤토리? 루팅? ...</param>
        /// <param name="_info">생성할 아이템 정보</param>
        public void GenerateItemObject(ContentType _type, ItemInventoryInfo _info)
        {
            ItemGenerateController g = Instantiate(itemPrefab, itemTf).GetComponent<ItemGenerateController>();
            switch (_type)
            {
                case ContentType.Inventory:
                    ((ContentSlotController)contentByType[ContentType.Inventory]).GenerateItem(g, _info);
                    break;
                case ContentType.Looting:
                    ((ContentSlotController)contentByType[ContentType.Looting]).GenerateItem(g, _info);
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
            }
        }

        private enum Side
        {
            L, C, R
        }
    }
}
