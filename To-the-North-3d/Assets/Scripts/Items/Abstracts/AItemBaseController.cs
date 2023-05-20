using System;
using System.Linq;
using Assets.Scripts.Components.Windows.Inventory;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Items
{
    public abstract class AItemBaseController : AbsItemController
    {
        private InventorySlotController curSlot, prevSlot, nextSlot;
        public InventorySlotController CurSlot
        {
            get
            {
                return curSlot;
            }
        }

        public int itemSizeRow
        {
            set => info.size.x = value;
            get => (int)info.size.x;
        }
        public int itemSizeCol
        {
            set => info.size.y = value;
            get => (int)info.size.y;
        }
        public bool isRotate, prevRotate;

        private int localRow;
        private int localCol;
        private Vector3 VectorCorr
        {
            get
            {
                return new Vector3(objTF.sizeDelta.x / 2 - 25, -objTF.sizeDelta.y / 2 + 25, 0);
            }
        }
        private RectTransform objTF;
        private BoxCollider objCollider;
        private Image image;
        private TextMeshProUGUI amountUGUI;

        public ItemBaseInfo info;

        private new void Update()
        {
            base.Update();
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (image != null) return;
            image = GetComponentInChildren<Image>();
            amountUGUI = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            amountUGUI.text = string.Empty;
            objTF = GetComponent<RectTransform>();
            objCollider = GetComponent<BoxCollider>();
        }

        /// <summary>
        ///  현재 슬롯을 시작으로 해당 아이템을 놓을 수 있는지 조건 확인하는 함수
        /// </summary>
        private bool CheckItemAttachable(InventorySlotController destSlot)
        {
            if (destSlot == null) return false;
            if (destSlot is EquipmentSlotController eSlot && info is ItemEquipmentInfo eInfo)
            {
                // 슬롯칸이 아닌 장비칸임
                return info.IsEquipment && !eSlot.IsEquipped &&
                    (
                        (
                            eInfo is ItemArmorInfo aInfo && eSlot.equipType.Equals(aInfo.equipPartType)
                        ) ||
                        (
                            eInfo is ItemWeaponInfo wInfo && new EquipBodyType[] { EquipBodyType.Primary, EquipBodyType.Secondary }.Contains(eSlot.equipType)
                        )
                    )
                    ;
            }
            if (destSlot is QuickSlotController qSlot && info.IsQuickable && !qSlot.IsEquipped)
            {
                // 퀵슬롯 등록 가능한 아이템 + 퀵슬롯 + 퀵슬롯 비었음
                // = 등록 가능
                return true;
            }
            switch (destSlot.ContainerType)
            {
                case ContentType.Inventory:
                    return ApplyActionForAllSlots(destSlot, (row, col) =>
                    {
                        return WindowInventoryController.InventorySlots[row, col].ItemTf == null;
                    });
                case ContentType.Looting:
                    return ApplyActionForAllSlots(destSlot, (row, col) =>
                    {
                        return WindowInventoryController.LootSlots[row, col].ItemTf == null;
                    });
                case ContentType.Commerce:
                    return ApplyActionForAllSlots(destSlot, (row, col) =>
                    {
                        return WindowInventoryController.CommerceSlots[row, col].ItemTf == null;
                    });
                default:
                    break;
            }
            return false;
        }

        /// <summary>
        /// 임시로 가능성 있는 타일 불 켜기/끄기 함수
        /// </summary>
        /// <param name="isOn"></param>
        private void ConsiderTargetSlot(InventorySlotController _targetSlot, bool isOn)
        {
            if (_targetSlot == null) return;
            ApplyActionForOnlyContentWithSlots(_targetSlot, (_slot) =>
            {
                _slot.IsConsidered = isOn;
            });
            if (_targetSlot is EquipmentSlotController ||
                _targetSlot is QuickSlotController)
            {
                _targetSlot.IsConsidered = isOn;
            }
        }

        /// <summary>
        /// attachSlot에 Item 부착
        /// </summary>
        public void ItemAttach(InventorySlotController attachSlot)
        {
            transform.SetParent(attachSlot.transform);
            // 리사이징
            ResizeOnPurpose(attachSlot);
            curSlot = attachSlot;
            nextSlot = prevSlot = null;
            if (info.InvenInfo == null)
            {
                info.InvenInfo = new()
                {
                    itemInfo = info,
                    pos = new Vector2(attachSlot.row, attachSlot.column),
                };
            }
            attachSlot.ItemTf = transform;
            attachSlot.EnrollItemToSlot();
            ApplyActionForOnlyContentWithSlots(attachSlot, (_slot) =>
            {
                _slot.ItemTf = transform;
            }, () =>
            {
                // 위치 잡기: 출발점: 0, 0, -1
                Vector3 pos = new(0, 0, -1);
                if (!isRotate)
                {
                    pos += VectorCorr;
                }
                else
                {
                    pos.x -= VectorCorr.y;
                    pos.y -= VectorCorr.x;
                }
                objTF.localPosition = pos;
            });
            // 부착하려고 하는 컨테이너의 타입?
            if (attachSlot is EquipmentSlotController equipSlot ||
                attachSlot is QuickSlotController)
            {
                if (isRotate)
                {
                    ItemRotate(false);
                }
                Vector3 pos = new(0, 0, -1);
                objTF.localPosition = pos;
            }
        }

        /// <summary>
        /// 기존 슬롯에서 Item 분리
        /// </summary>
        public void ItemDetach()
        {
            if (curSlot == null) return;
            curSlot.DetachItemFromSlot();
            info.InvenInfo = null;
            ApplyActionForOnlyContentWithSlots(curSlot, (_slot) =>
            {
                _slot.ItemTf = null;
            });
            switch (curSlot.ContainerType)
            {
                case ContentType.Equipment:
                    ((EquipmentSlotController)curSlot).EquipItemInfo = null;
                    break;
                default:
                    break;
            }
            curSlot.ItemTf = null;
            prevSlot = curSlot;
            curSlot = null;
        }

        /// <summary>
        /// 아이템 회전
        /// </summary>
        /// <param name="isSingle">단일 행동인지: false -> 드래그 로직 실행 = 부착까지 이어서 진행</param>
        public void ItemRotate(bool isSingle)
        {
            // 돌리는 의미가 없는 아이템이면 return
            if (localRow == localCol)
                return;
            // 아이템 하단의 흰색 칸 헤제
            ConsiderTargetSlot(nextSlot, false);
            nextSlot = null;
            // 현재 돌아가 있는지 확인해서 방향 결정
            if (isRotate)
            {
                objTF.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                objTF.localRotation = Quaternion.Euler(0, 0, 90);
            }
            // itemsize 변경
            int tempSize;
            tempSize = localCol;
            localCol = localRow;
            localRow = tempSize;
            // isRotate 변경
            isRotate = !isRotate;
            if (!isSingle)
            {
                OnDraggingSkipRotate();
            }
        }

        /// <summary>
        /// 아이템 인벤토리에서 제거:
        /// 오브젝트는 제너레이터만 남고 비활성화된 뒤 Items로 간다 (풀링)
        /// </summary>
        public void ItemTruncate()
        {
            ItemDetach();
            info.Ctrl = null;
            image.sprite = null;
            amountUGUI.text = string.Empty;
            transform.SetParent(WindowInventoryController.Instance.ItemTf);
            gameObject.SetActive(false);
            Destroy(this);
        }

        protected override void OnDown()
        {
            ItemDetach();
            prevRotate = isRotate;
            transform.SetParent(WindowInventoryController.Instance.ItemTf);
            objTF.SetAsLastSibling();
            OnHoverExit();
        }

        protected override void OnDownWithKeyPress()
        {
            if (curSlot.ContainerType != ContentType.Inventory)
            {
                // 인벤토리가 아닐 시 = 공통
                // -> 바로 인벤토리로 이동
                // 기존의 슬롯에서 떼기
                ItemDetach();
                // 만약 회전 상태다 -> 회전 풀기
                if (isRotate)
                    ItemRotate(true);

                // 자동 정렬로 이동
                InitInfo(info, null, ContentType.Inventory);
                return;
            }
            // 공통이 아닌 부분
            OnItemDownWithKeyPress();
        }

        protected override void OnDraging()
        {
            // 드래그 중 R키 누르면 아이템 회전
            if (Input.GetKeyDown(KeyCode.R))
            {
                ItemRotate(true);
            }
            OnDraggingSkipRotate();
        }

        private void OnDraggingSkipRotate()
        {
            // 드래그 중 I키 누르면 놓아버림
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnUp();
                isMouseDown = false;
                return;
            }
            // 마우스 드래그 이벤트
            Vector3 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            t.y = 12f;
            objTF.position = t;

            InventorySlotController candidateSlot = null;
            // 중앙 레이 = 장비 체크용
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Vector3.down, out RaycastHit hitEquip, 2f, GlobalStatus.Constant.slotMask))
            {
                candidateSlot = hitEquip.transform.GetComponent<EquipmentSlotController>();
            }
            // 인벤토리 슬롯 체크용
            // 보정값 적용
            t = objTF.TransformVector(VectorCorr);
            if (Physics.Raycast(new Vector3(transform.position.x - t.x, transform.position.y - 1, transform.position.z + (isRotate ? t.z : -t.z)), Vector3.down, out RaycastHit hit, 2f, GlobalStatus.Constant.slotMask))
            {
                // 슬롯 위에 있음
                // = 후보 존재
                candidateSlot = hit.transform.GetComponent<InventorySlotController>();
            }
            if (CheckItemAttachable(candidateSlot))
            {
                // 배치 가능
                if (nextSlot != null && nextSlot.Equals(candidateSlot))
                {
                    // 이전 배치 가능 슬롯하고 동일
                    // = 별거 안함
                }
                else
                {
                    if (nextSlot != null)
                    {
                        ConsiderTargetSlot(nextSlot, false);
                    }
                    // 신규 배치 가능 슬롯임
                    // = 후보 등록 + 활성화
                    nextSlot = candidateSlot;
                    ConsiderTargetSlot(nextSlot, true);
                }
            }
            else
            {
                // 배치 불가
                if (nextSlot != null)
                {
                    ConsiderTargetSlot(nextSlot, false);
                }
                nextSlot = null;
            }
        }

        protected override void OnUp()
        {
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Vector3.down, out RaycastHit hitItem, 2f, GlobalStatus.Constant.itemMask))
            {
                // 아래에 아이템 있음
                if (!OnItemOnOtherItem(hitItem.transform.GetComponent<AItemBaseController>().info)) return;
            }

            void Rollback()
            {
                if (nextSlot != null)
                {
                    ConsiderTargetSlot(nextSlot, false);
                }
                if (isRotate != prevRotate)
                {
                    ItemRotate(true);
                }
                ItemAttach(prevSlot);
            }

            // nextSlot이 있는지 확인
            if (nextSlot != null)
            {
                // 있음
                // 만약 구매 시도라면 ? 돈 확인 필요
                if (prevSlot.ContainerType.Equals(ContentType.Commerce))
                {
                    // 돈이 없는가 ?
                    if (InGameStatus.Currency < info.price)
                    {
                        // 롤백
                        Rollback();
                    }
                    else
                    {
                        // 구매
                        ItemAttach(nextSlot);
                        InGameStatus.Currency = -info.price;
                    }
                    return;
                }
                // 만약 판매 시도라면 ?
                if (nextSlot.ContainerType.Equals(ContentType.Commerce))
                {
                    // 판매
                    ItemAttach(nextSlot);
                    InGameStatus.Currency = +info.price;
                    return;
                }
                // 만약 장착 시도라면 ?
                if (nextSlot.ContainerType.Equals(ContentType.Equipment))
                {
                    OnItemDownWithKeyPress();
                    return;
                }
                // = 장착
                ItemAttach(nextSlot);
            }
            else
            {
                // 없음
                // = 이전 위치로 롤백
                Rollback();
            }
        }

        /// <summary>
        /// 종류에 맞는 데이터 할당 함수
        /// </summary>
        /// <param name="_info">데이터</param>
        public ItemInventoryInfo InitInfo(ItemBaseInfo _info, InventorySlotController slotToAttach = null, ContentType type = ContentType.Undefined)
        {
            Init();
            info = _info;
            info.Ctrl = this;
            image.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
            switch (info)
            {
                case ItemConsumableInfo amountInfo:
                    amountInfo.AmountDisplay = amountUGUI;
                    break;
                case ItemMagazineInfo amountInfo:
                    amountInfo.AmountDisplay = amountUGUI;
                    break;
                case ItemWeaponInfo amountInfo:
                    amountInfo.AmountDisplay = amountUGUI;
                    break;
            }
            image.GetComponent<Canvas>().sortingLayerName = "UI Covering Map";
            // 게임오브젝트 이름 변경
            gameObject.name = info.name;
            localRow = itemSizeRow;
            localCol = itemSizeCol;
            if (slotToAttach != null)
            {
                // 자동 정렬이 아닌 경우
                ItemAttach(slotToAttach);
                return null;
            }
            // 자동 정렬인 경우
            SeekSlotAttachable(type, info, out InventorySlotController slotQualified, out ItemInventoryInfo ret);
            ItemAttach(slotQualified);
            gameObject.SetActive(true);
            return ret;
        }

        /// <summary>
        /// 아이템 오브젝트를 인벤토리에 넣는 함수:
        /// 자동 정렬
        /// </summary>
        public void SendToInventory()
        {
            SeekSlotAttachable(ContentType.Inventory, info, out InventorySlotController slotQualified, out ItemInventoryInfo ret);
            ItemAttach(slotQualified);
        }

        private void ResizeOnPurpose(InventorySlotController _slot = null)
        {
            float w, h, l;
            w = objTF.sizeDelta.x;
            h = objTF.sizeDelta.y;
            l = Mathf.Max(w, h);
            void ApplyResize(int maxW, int maxH)
            {
                if (l == w)
                {
                    // 가로가 더 김
                    // 가로: 108; k = 108 / w;
                    // 세로: h * k = h * 108 / w
                    objCollider.size = objTF.sizeDelta = new Vector2(maxW, h * maxW / w);
                }
                else
                {
                    // 세로가 더 김
                    // 세로: 162; k = 162 / h
                    // 가로: w * = w * 162 / h
                    objCollider.size = objTF.sizeDelta = new Vector2(w * maxH / h, maxH);
                }
                // 이미지 사이즈 비율 유지하면서 확대하기
                // 이미지 가로가 긴가 세로가 긴가 확인
                if (image.sprite == null) return;
                Vector2 s = image.sprite.bounds.size;
                float sw = s.x, sh = s.y;
                if (sw >= sh)
                {
                    // 가로가 김 = 가로에 맞춰서 확대
                    image.rectTransform.sizeDelta = new Vector2(objCollider.size.x, objCollider.size.x * sh / sw);
                }
                else
                {
                    // 세로가 김 = 세로에 맞춰서 확대
                    image.rectTransform.sizeDelta = new Vector2(objCollider.size.y * sw / sh, objCollider.size.y);
                }
            }
            if (_slot == null)
            {
                // = 최대 크기: 50 * 50
                ApplyResize(50, 50);
                return;
            }
            if (_slot is QuickSlotController hSlot)
            {
                // = 최대 크기: 70 * 70
                ApplyResize(70, 70);
                return;
            }
            if (_slot is EquipmentSlotController eSlot)
            {
                switch (eSlot.equipType)
                {
                    // 120 * 120
                    // = 최대 크기: 108 * 108
                    case EquipBodyType.Helmat:
                    case EquipBodyType.Mask:
                        ApplyResize(108, 108);
                        break;
                    // 120 * 180
                    // = 최대: 108 * 162
                    case EquipBodyType.Body:
                    case EquipBodyType.BackPack:
                        ApplyResize(108, 162);
                        break;
                    // 240 * 120
                    // = 최대: 216 * 108
                    case EquipBodyType.Primary:
                    case EquipBodyType.Secondary:
                        ApplyResize(216, 108);
                        break;
                }
                return;
            }
            // 일반 슬롯 컨테이너일 경우
            w = 50 * (int)info.size.x;
            h = 50 * (int)info.size.y;
            l = Mathf.Max(w, h);
            ApplyResize(50 * (int)info.size.x, 50 * (int)info.size.y);
        }

        /// <summary>
        /// 아래 모든 슬롯에 액션 반복 함수: 반환 없음
        /// </summary>
        /// <param name="actionToApply"></param>
        private void ApplyActionForAllSlots(InventorySlotController startSlot, Action<int, int> actionToApply)
        {
            for (int i = 0; i < localCol; i++)
            {
                for (int j = 0; j < localRow; j++)
                {
                    try
                    {
                        actionToApply(startSlot.row + i, startSlot.column + j);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // 넘어감
                    }
                }
            }
        }


        /// <summary>
        /// 아래 모든 슬롯에 액션 반복 검사 함수:
        /// 모든 슬롯에 대해서 true일 때 true 반환
        /// </summary>
        /// <param name="actionToApply"></param>
        private bool ApplyActionForAllSlots(InventorySlotController startSlot, Func<int, int, bool> actionToApply)
        {
            for (int i = 0; i < localCol; i++)
            {
                for (int j = 0; j < localRow; j++)
                {
                    try
                    {
                        if (!actionToApply(startSlot.row + i, startSlot.column + j))
                        {
                            return false;
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// ContentSlotController인 경우에만 적용하는 함수
        /// </summary>
        /// <param name="_targetSlot"></param>
        /// <param name="actionToLoop"></param>
        private void ApplyActionForOnlyContentWithSlots(InventorySlotController _targetSlot, Action<InventorySlotController> actionToLoop = null, Action actionBeforeLoop = null)
        {
            if (actionBeforeLoop != null)
                actionBeforeLoop();
            if (_targetSlot == null) return;
            if (_targetSlot.ContainerType == ContentType.Inventory)
            {
                if (actionToLoop != null)
                    ApplyActionForAllSlots(_targetSlot, (r, c) =>
                    {
                        actionToLoop(WindowInventoryController.InventorySlots[r, c]);
                    });
                return;
            }
            if (_targetSlot.ContainerType == ContentType.Looting)
            {
                if (actionToLoop != null)
                    ApplyActionForAllSlots(_targetSlot, (r, c) =>
                    {
                        actionToLoop(WindowInventoryController.LootSlots[r, c]);
                    });
                return;
            }
            if (_targetSlot.ContainerType == ContentType.Commerce)
            {
                if (actionToLoop != null)
                    ApplyActionForAllSlots(_targetSlot, (r, c) =>
                    {
                        actionToLoop(WindowInventoryController.CommerceSlots[r, c]);
                    });
                return;
            }
        }

        /// <summary>
        /// 위치가 정해지지 않은 아이템 설치 가능 위치 및 신규 인벤토리 객체 반환 함수
        /// </summary>
        /// <param name="type">컨텐츠 타입</param>
        /// <param name="_info">아이템 정보</param>
        /// <param name="slotQualified">반환할 설치 가능 슬롯</param>
        /// <param name="newInventoryInfo">반환할 신규 인벤로티 객체</param>
        private void SeekSlotAttachable(ContentType type, ItemBaseInfo _info, out InventorySlotController slotQualified, out ItemInventoryInfo newInventoryInfo)
        {
            InventorySlotController cur;
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    switch (type)
                    {
                        case ContentType.Inventory:
                            cur = WindowInventoryController.InventorySlots[i, j];
                            if (CheckItemAttachable(cur))
                            {
                                slotQualified = cur;
                                _info.InvenInfo = newInventoryInfo = new()
                                {
                                    itemInfo = _info,
                                    pos = new Vector2(cur.row, cur.column)
                                };
                                return;
                            }
                            continue;
                        case ContentType.Looting:
                            cur = WindowInventoryController.LootSlots[i, j];
                            if (CheckItemAttachable(cur))
                            {
                                slotQualified = cur;
                                _info.InvenInfo = newInventoryInfo = new()
                                {
                                    itemInfo = _info,
                                    pos = new Vector2(cur.row, cur.column)
                                };
                                return;
                            }
                            continue;
                        case ContentType.Commerce:
                            cur = WindowInventoryController.CommerceSlots[i, j];
                            if (CheckItemAttachable(cur))
                            {
                                slotQualified = cur;
                                _info.InvenInfo = newInventoryInfo = new()
                                {
                                    itemInfo = _info,
                                    pos = new Vector2(cur.row, cur.column)
                                };
                                return;
                            }
                            continue;
                        default:
                            break;
                    }
                }
            }
            // 남는칸 없음
            slotQualified = null;
            _info.InvenInfo = newInventoryInfo = null;
            return;
        }

        /// <summary>
        /// 아이템을 뗄 때 그 아래 다른 아이템이 있으면 실행하는 함수
        /// </summary>
        /// <returns>이후 로직을 실행하지 않는다 = false | 이후 로직을 이어서 실행한다 = true</returns>
        public abstract bool OnItemOnOtherItem(ItemBaseInfo _targetItemInfo);

        /// <summary>
        /// 아이템 타입 별 키 누른 상태로 마우스 다운일 때 실행되어야 하는 함수
        /// </summary>
        public abstract void OnItemDownWithKeyPress();
    }
}
