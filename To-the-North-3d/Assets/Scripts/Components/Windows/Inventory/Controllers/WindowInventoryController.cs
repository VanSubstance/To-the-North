using System.Collections.Generic;
using Assets.Scripts.Events.Controllers;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class WindowInventoryController : WindowBaseController
    {
        [SerializeField]
        private Transform containerBlank, containerSlots, containerEquipment, itemPrefab;
        private static Dictionary<ContentType, ContentBaseController> contentByType;

        public ContentSlotController ContentLoot
        {
            get
            {
                return (ContentSlotController)contentByType[ContentType.Looting];
            }
        }

        public ContentSlotController ContentInventory
        {
            get
            {
                return (ContentSlotController)contentByType[ContentType.Inventory];
            }
        }

        public ContentSlotController ContentCommerce
        {
            get
            {
                return (ContentSlotController)contentByType[ContentType.Commerce];
            }
        }

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
        public static InventorySlotController[,] CommerceSlots
        {
            get
            {
                return ((ContentSlotController)contentByType[ContentType.Commerce]).slots;
            }
        }

        public static ContentEquipmentController equipmentCtrl
        {
            get
            {
                return (ContentEquipmentController)contentByType[ContentType.Equipment];
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
            // 풀링용 아이템 객체 생성해두기:: 100개 ?
            for (int i = 0; i < 100; i++)
            {
                Instantiate(itemPrefab, itemTf).gameObject.SetActive(false);
            }



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
            GlobalComponent.Common.Text.Inventory.inventory = contentByType[ContentType.Inventory].Container.GetComponent<ContainerBaseController>().titleUGUI;
            contentByType[ContentType.Looting] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>(ContentType.Looting);
            GlobalComponent.Common.Text.Inventory.looting = contentByType[ContentType.Looting].Container.GetComponent<ContainerBaseController>().titleUGUI;
            contentByType[ContentType.Commerce] = Instantiate(containerSlots, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentSlotController>(ContentType.Commerce);
            GlobalComponent.Common.Text.Inventory.commerce = contentByType[ContentType.Commerce].Container.GetComponent<ContainerBaseController>().titleUGUI;
            contentByType[ContentType.Equipment] = Instantiate(containerEquipment, storeTf).GetComponent<ContainerBaseController>().GetContent<ContentEquipmentController>(ContentType.Equipment);
            GlobalComponent.Common.Text.Inventory.equipment = contentByType[ContentType.Equipment].Container.GetComponent<ContainerBaseController>().titleUGUI;

            OnClose();
        }

        /// <summary>
        /// 해당 사이드에 해당 컨텐츠 컨테이너를 불러오는 함수
        /// </summary>
        /// <param name="side">목표 사이드</param>
        /// <param name="_targetType">목표 타입</param>
        private void CallContent(Side side, ContentType _targetType)
        {
            if (contentsVisual[side].Container.Equals(contentByType[_targetType])) return;
            contentsVisual[side].Container.SetParent(storeTf);
            (contentsVisual[side] = contentByType[_targetType]).Container.SetParent(visualTf);
            contentsVisual[side].Container.SetSiblingIndex((int)side);
        }

        /// <summary>
        /// 아이템 오브젝트 생성 함수
        /// </summary>
        /// <param name="_type">해당 컨테이너: 인벤토리? 루팅? ...</param>
        /// <param name="_info">생성할 아이템 정보: 위치 정보 O</param>
        public void GenerateItemObject(ContentType _type, ItemInventoryInfo _info)
        {
            ItemGenerateController g = GetNewGenerator();
            switch (_type)
            {
                case ContentType.Inventory:
                    ContentInventory.GenerateItem(g, _info);
                    break;
                case ContentType.Looting:
                    ContentLoot.GenerateItem(g, _info);
                    break;
                case ContentType.Commerce:
                    ContentCommerce.GenerateItem(g, _info);
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
                case ContentType.Equipment:
                    break;
            }
        }

        /// <summary>
        /// 아이템 오브젝트 생성 함수:
        /// 위치 데이터 없음 = 자동 정렬
        /// </summary>
        /// <param name="_type">목표 컨테이너 타입</param>
        /// <param name="_info">생성할 아이템 정보</param>
        public void GenerateItemObjectWithAuto(ContentType _type, ItemBaseInfo _info)
        {
            ItemGenerateController g = GetNewGenerator();
            ItemInventoryInfo s = null;
            switch (_type)
            {
                case ContentType.Inventory:
                    s = ContentInventory.GenerateItemWithAuto(g, _info, ContentType.Inventory);
                    break;
                case ContentType.Looting:
                    s = ContentLoot.GenerateItemWithAuto(g, _info, ContentType.Looting);
                    break;
                case ContentType.Commerce:
                    s = ContentCommerce.GenerateItemWithAuto(g, _info, ContentType.Commerce);
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
                case ContentType.Equipment:
                    break;
            }
        }

        /// <summary>
        /// 풀링에서 신규 제네레이터 하나 가져오기
        /// </summary>
        /// <returns></returns>
        private ItemGenerateController GetNewGenerator()
        {
            for (int i = 0; i < itemTf.childCount; i++)
            {
                Transform t = null;
                if (!(t = itemTf.GetChild(i)).gameObject.activeSelf) return t.GetComponent<ItemGenerateController>();
            }
            return null;
        }

        private EventLootingController CurLoot;
        /// <summary>
        /// 루팅 열기
        /// </summary>
        /// <param name="eventLoot">현재 열 루팅 이벤트</param>
        public void Open(EventLootingController eventLoot)
        {
            if (CurLoot != null) return;
            CurLoot = eventLoot;
            CallContent(Side.L, ContentType.Looting);
            foreach (ItemBaseInfo _itemInfo in eventLoot.itemsLoot)
            {
                if (_itemInfo.Ctrl != null) continue;
                GenerateItemObjectWithAuto(ContentType.Looting, _itemInfo);
            }
            Open();
        }

        public void OpenCommerce()
        {
            CallContent(Side.L, ContentType.Commerce);
            Open();
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
            // 루팅 비우기
            if (CurLoot != null)
            {
                CurLoot.itemsLoot = ContentLoot.Clear();
                CurLoot = null;
            }
            else
            {
                ContentLoot.Clear();
            }
            // 상점 비우기
            ContentCommerce.Clear();
            // 일반 인벤토리로 리셋
            CallContent(Side.L, ContentType.None_L);
            CallContent(Side.C, ContentType.Equipment);
            CallContent(Side.R, ContentType.Inventory);
        }

        private enum Side
        {
            L, C, R
        }
    }
}
