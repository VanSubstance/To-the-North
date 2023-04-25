using Assets.Scripts.Components.Windows.Inventory;
using System;
using UnityEngine;
using UnityEngine.UI;

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
            set => baseInfo.size.x = value;
            get => (int)baseInfo.size.x;
        }
        public int itemSizeCol
        {
            set => baseInfo.size.y = value;
            get => (int)baseInfo.size.y;
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

        public ItemBaseInfo baseInfo
        {
            get
            {
                return (ItemBaseInfo)(object)info;
            }
        }
        public TItemInfo info;

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
            objTF = GetComponent<RectTransform>();
            objCollider = GetComponent<BoxCollider>();
        }

        /// <summary>
        ///  ItemSize만큼 destSlot 주변의 Slot들 검사
        /// </summary>
        private bool ItemSizeCheck(InventorySlotController destSlot)
        {
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
            ResizeOnPurpose(attachSlot.ContainerType);
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
                } else
                {
                    pos.x -= VectorCorr.y;
                    pos.y -= VectorCorr.x;
                }
                objTF.localPosition = pos;
            });
            // 부착하려고 하는 컨테이너의 타입?
            switch (attachSlot.ContainerType)
            {
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
            ApplyActionForOnlyContentWithSlots(curSlot, (_slot) =>
            {
                _slot.ItemTf = null;
            });
            switch (curSlot.ContainerType)
            {
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
            ItemDetach();
            prevRotate = isRotate;
            transform.SetParent(WindowInventoryController.Instance.ItemTf);
            objTF.SetAsLastSibling();
            OnHoverExit();
        }

        protected override void OnDraging()
        {
            // 드래그 중 R키 누르면 아이템 회전
            if (Input.GetKeyDown(KeyCode.R))
            {
                ItemRotate();
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
            t.y = 10f;
            objTF.position = t;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 마우스 위치 기준으로 아래 그리드인지 아닌지 확인
            // 보정값 적용
            t = objTF.TransformVector(VectorCorr);
            if (Physics.Raycast(new Vector3(transform.position.x - t.x, transform.position.y + 1, transform.position.z + (isRotate ? t.z : -t.z)), Vector3.down, out RaycastHit hit, 2f, GlobalStatus.Constant.slotMask))
            {
                // 슬롯 위에 있음
                // = 후보 존재
                InventorySlotController candidateSlot;
                candidateSlot = hit.transform.GetComponent<InventorySlotController>();
                if (ItemSizeCheck(candidateSlot))
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
            else
            {
                // 아래에 후보 슬롯 없음
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
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
        }

        /// <summary>
        /// 종류에 맞는 데이터 할당 함수
        /// </summary>
        /// <param name="_info">데이터</param>
        public void InitInfo(TItemInfo _info, InventorySlotController slotToAttach = null)
        {
            Init();
            info = _info;
            image.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(baseInfo));
            image.GetComponent<Canvas>().sortingLayerName = "UI Covering Map";
            // 게임오브젝트 이름 변경
            gameObject.name = baseInfo.name;
            localRow = itemSizeRow;
            localCol = itemSizeCol;
            if (slotToAttach != null)
            {
                ItemAttach(slotToAttach);
            }
        }

        private void ResizeOnPurpose(ContentType _type)
        {
            ApplyActionForOnlyContentWithSlots(null, null, () =>
            {
                image.rectTransform.sizeDelta = objCollider.size = objTF.sizeDelta = baseInfo.size * 50f;
            });
            switch (_type)
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
        /// 아이템이 해당 칸에 설치될 수 있는지 체크하는 함수
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckItemTag(InventorySlotController slot, bool isGridOn);

        /// <summary>
        /// 아이템을 뗄 때 그 아래 다른 아이템이 있으면 실행하는 함수
        /// </summary>
        /// <param name="slotController"></param>
        public abstract void GridOnCheckIfItemExist(InventorySlotController slotController);
    }
}
