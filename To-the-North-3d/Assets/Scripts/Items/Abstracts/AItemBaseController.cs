using Assets.Scripts.Components.Windows.Inventory;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 아이템 베이스 컨트롤러
    /// 드래그 앤 드롭: 이동
    ///     !! 드래그 앤 드롭 시, 아래 있는 칸에 얘가 장착이 가능한 지 조건을 판별해야 함
    /// 마우스 위에서 유지: 정보 뜨기
    /// 더블클릭: 추후 추상 구현
    /// </summary>
    public abstract class AItemBaseController<TItemInfo> : AbsItemController
    {
        private InventorySlotController curSlot, prevSlot, nextSlot;

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
        private Vector3 mousePos;
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

        /// <summary>
        /// 마우스 이벤트가 중간에 종료되어야 할 때 후속 이벤트 방지용
        /// </summary>
        /// 
        private bool IsMouseEventDone;
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
            if (destSlot is EquipmentSlotController && info is ItemEquipmentInfo)
            {
                // 슬롯칸이 아닌 장비칸임
                return info.IsEquipment && !((EquipmentSlotController)destSlot).IsEquipped &&
                    (
                        (
                            info is ItemArmorInfo && ((EquipmentSlotController)destSlot).equipType.Equals(((ItemArmorInfo)info).equipPartType)
                        ) ||
                        (
                            info is ItemWeaponInfo && new EquipBodyType[] { EquipBodyType.Left, EquipBodyType.Right }.Contains(((EquipmentSlotController)destSlot).equipType)
                        )
                    )
                    ;
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
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
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
            if (_targetSlot is EquipmentSlotController)
            {
                _targetSlot.IsConsidered = isOn;
            }
            switch (_targetSlot.ContainerType)
            {
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
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
            attachSlot.ItemTf = transform;
            ApplyActionForOnlyContentWithSlots(attachSlot, (_slot) =>
            {
                _slot.ItemTf = transform;
            }, () =>
            {
                // 위치 잡기: 출발점: 0, 0, -1
                Vector3 pos = new Vector3(0, 0, -1);
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
            switch (attachSlot.ContainerType)
            {
                case ContentType.Equipment:
                    if (isRotate)
                    {
                        ItemRotate();
                    }
                    Vector3 pos = new Vector3(0, 0, -1);
                    objTF.localPosition = pos;
                    ((EquipmentSlotController)attachSlot).EquipItemInfo = (ItemEquipmentInfo)info;
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
            }
        }

        /// <summary>
        /// 기존 슬롯에서 Item 분리
        /// </summary>
        public void ItemDetach()
        {
            info.InvenInfo = null;
            ApplyActionForOnlyContentWithSlots(curSlot, (_slot) =>
            {
                _slot.ItemTf = null;
            });
            switch (curSlot.ContainerType)
            {
                case ContentType.Equipment:
                    ((EquipmentSlotController)curSlot).EquipItemInfo = null;
                    curSlot.ItemTf = null;
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
            }
            prevSlot = curSlot;
            curSlot = null;
        }

        /// <summary>
        /// 아이템 회전
        /// </summary>
        public void ItemRotate()
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
                //image.rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                objTF.localRotation = Quaternion.Euler(0, 0, 90);
                //image.rectTransform.anchoredPosition = Vector2.one * 25;
            }
            // itemsize 변경
            int tempSize;
            tempSize = localCol;
            localCol = localRow;
            localRow = tempSize;
            // recttransform.sizeDelta 변경
            //objTF.sizeDelta = new Vector2(objTF.sizeDelta.y, objTF.sizeDelta.x);
            //objTF.transform.position -= new Vector3(objTF.sizeDelta.x / 2, objTF.sizeDelta.y / 2, 0);
            // isRotate 변경
            isRotate = !isRotate;
            OnDraggingSkipRotate();
        }

        protected override void OnDown()
        {
            IsMouseEventDone = false;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                // 왼쪽 ctrl 누른 상태로 클릭 시
                if (curSlot.ContainerType != ContentType.Inventory)
                {
                    // 인벤토리가 아닐 시
                    // -> 바로 인벤토리로 이동
                    IsMouseEventDone = true;
                    // 기존의 슬롯에서 떼기
                    ItemDetach();
                    // 만약 회전 상태다 -> 회전 풀기
                    if (isRotate)
                        ItemRotate();

                    // 자동 정렬로 이동
                    InitInfo(info, null, ContentType.Inventory);
                    return;
                } else
                {
                    // 인벤토리일 시
                    if (info is ItemEquipmentInfo & info is not ItemMagazineInfo)
                    {
                        // 장착 가능 장비일 시
                        switch (((ItemEquipmentInfo)info).equipmentType)
                        {
                            case EquipmentType.Armor:
                                switch (((ItemArmorInfo)info).equipPartType)
                                {
                                    case EquipBodyType.Helmat:
                                        if (!WindowInventoryController.equipmentCtrl.helmetCtrl.IsEquipped)
                                        {
                                            // 장착 가능
                                        }
                                        break;
                                    case EquipBodyType.Mask:
                                        if (!WindowInventoryController.equipmentCtrl.maskCtrl.IsEquipped)
                                        {
                                            // 장착 가능
                                        }
                                        break;
                                    case EquipBodyType.Body:
                                        if (!WindowInventoryController.equipmentCtrl.bodyCtrl.IsEquipped)
                                        {
                                            // 장착 가능
                                        }
                                        break;
                                    case EquipBodyType.BackPack:
                                        if (!WindowInventoryController.equipmentCtrl.backpackCtrl.IsEquipped)
                                        {
                                            // 장착 가능
                                        }
                                        break;
                                    case EquipBodyType.Right:
                                        break;
                                    case EquipBodyType.Left:
                                        break;
                                }
                                break;
                            case EquipmentType.Weapon:
                                if (!WindowInventoryController.equipmentCtrl.handRCtrl.IsEquipped)
                                {
                                    // 양손일 경우 여기서 걸러야 함
                                }
                                if (!WindowInventoryController.equipmentCtrl.handLCtrl.IsEquipped)
                                {

                                }
                                break;
                            case EquipmentType.Magazine:
                                break;
                        }
                    }
                }
            }
            ItemDetach();
            prevRotate = isRotate;
            transform.SetParent(WindowInventoryController.Instance.ItemTf);
            objTF.SetAsLastSibling();
            OnHoverExit();
        }

        protected override void OnDraging()
        {
            if (IsMouseEventDone) return;
            // 드래그 중 R키 누르면 아이템 회전
            if (Input.GetKeyDown(KeyCode.R))
            {
                ItemRotate();
            }
            OnDraggingSkipRotate();
        }

        private void OnDraggingSkipRotate()
        {
            if (IsMouseEventDone) return;
            // 드래그 중 I키 누르면 놓아버림
            if (Input.GetKeyDown(KeyCode.I))
            {
                OnUp();
                isMouseDown = false;
                return;
            }
            // 마우스 드래그 이벤트
            Vector3 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            t.y = 10f;
            objTF.position = t;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            InventorySlotController candidateSlot = null;

            // 장비 체크용
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, out RaycastHit hitEquip, 2f, GlobalStatus.Constant.slotMask))
            {
                candidateSlot = hitEquip.transform.GetComponent<EquipmentSlotController>();
            }
            // 인벤토리 슬롯 체크용
            // 보정값 적용
            t = objTF.TransformVector(VectorCorr);
            if (Physics.Raycast(new Vector3(transform.position.x - t.x, transform.position.y + 1, transform.position.z + (isRotate ? t.z : -t.z)), Vector3.down, out RaycastHit hit, 2f, GlobalStatus.Constant.slotMask))
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
            if (IsMouseEventDone)
            {
                IsMouseEventDone = false;
                return;
            }
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // 탄환의 경우, 아래에 탄창이 있으면 삽탄해야 함
            if (info is ItemBulletInfo)
            {
                // 탄환임
                if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, out RaycastHit hitItem, 2f, GlobalStatus.Constant.itemMask))
                {
                    ItemEquipmentController c;
                    if ((c = hitItem.transform.GetComponent<ItemEquipmentController>()) != null)
                    {
                        if (c.info is ItemMagazineInfo)
                        {
                            // 아래에 탄창 있음
                            // 삽탄
                            info = ((ItemMagazineInfo)c.info).LoadMagazine((ItemBulletInfo)info);
                        }
                    }
                }
            }
            
            // nextSlot이 있는지 확인
            if (nextSlot != null)
            {
                // 있음
                // = 장착
                ItemAttach(nextSlot);
            }
            else
            {
                // 없음
                // = 이전 위치로 롤백
                if (isRotate != prevRotate)
                {
                    ItemRotate();
                }
                ItemAttach(prevSlot);
            }
            IsMouseEventDone = false;
        }

        /// <summary>
        /// 종류에 맞는 데이터 할당 함수
        /// </summary>
        /// <param name="_info">데이터</param>
        public ItemInventoryInfo InitInfo(ItemBaseInfo _info, InventorySlotController slotToAttach = null, ContentType type = ContentType.Undefined)
        {
            Init();
            info = _info;
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
            return ret;
        }

        private void ResizeOnPurpose(InventorySlotController _slot = null)
        {
            if (_slot == null)
            {
                image.rectTransform.sizeDelta = objCollider.size = objTF.sizeDelta = info.size * 50f;
                return;
            }
            if (_slot is EquipmentSlotController)
            {
                float w, h, l;
                w = objTF.sizeDelta.x;
                h = objTF.sizeDelta.y;
                l = Mathf.Max(w, h);
                switch (((EquipmentSlotController)_slot).equipType)
                {
                    // 120 * 120
                    // = 최대 크기: 108 * 108
                    case EquipBodyType.Helmat:
                    case EquipBodyType.Mask:
                        objCollider.size = objTF.sizeDelta = info.size * 110f;
                        break;
                    // 120 * 180
                    // = 최대: 108 * 162
                    case EquipBodyType.Body:
                    case EquipBodyType.BackPack:
                        if (l == w)
                        {
                            // 가로가 더 김
                            // 가로: 108; k = 108 / w;
                            // 세로: h * k = h * 108 / w
                            objCollider.size = objTF.sizeDelta = new Vector2(108, h * 108 / w);
                        }
                        else
                        {
                            // 세로가 더 김
                            // 세로: 162; k = 162 / h
                            // 가로: w * = w * 162 / h
                            objCollider.size = objTF.sizeDelta = new Vector2(w * 162 / h, 162);
                        }
                        break;
                    // 240 * 120
                    // = 최대: 216 * 108
                    case EquipBodyType.Right:
                    case EquipBodyType.Left:
                        if (l == h)
                        {
                            // 세로가 더 김
                            // 세로: 108; k = 108 / h
                            // 가로: w * = w * 108 / h
                            objCollider.size = objTF.sizeDelta = new Vector2(w * 108 / h, 108);
                        }
                        else
                        {
                            // 가로가 더 김
                            // 가로: 216; k = 216 / w;
                            // 세로: h * k = h * 216 / w
                            objCollider.size = objTF.sizeDelta = new Vector2(216, h * 216 / w);
                        }
                        break;
                }
                image.rectTransform.sizeDelta = objCollider.size;
                return;
            }
            ApplyActionForOnlyContentWithSlots(null, null, () =>
            {
                image.rectTransform.sizeDelta = objCollider.size = objTF.sizeDelta = info.size * 50f;
            });
            switch (_slot.ContainerType)
            {
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
            }
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
        }

        /// <summary>
        /// 위치가 정해지지 않은 아이템 설치 가능 위치 및 신규 인벤토리 객체 반환 함수
        /// </summary>
        /// <param name="type">컨텐츠 타입</param>
        /// <param name="_info">아이템 정보</param>
        /// <param name="slotQualified">반환할 설치 가능 슬롯</param>
        /// <param name="newInventoryInfo">반환할 심규 인벤로티 객체</param>
        private void SeekSlotAttachable(ContentType type, ItemBaseInfo _info, out InventorySlotController slotQualified, out ItemInventoryInfo newInventoryInfo)
        {
            InventorySlotController cur = null;
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
                                // 남는칸 있음
                                slotQualified = cur;
                                newInventoryInfo = new()
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
                                newInventoryInfo = new()
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
            newInventoryInfo = null;
            return;
        }

        /// <summary>
        /// 아이템을 뗄 때 그 아래 다른 아이템이 있으면 실행하는 함수
        /// </summary>
        /// <param name="slotController"></param>
        public abstract void GridOnCheckIfItemExist(InventorySlotController slotController);
    }
}
