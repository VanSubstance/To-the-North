using System;
using Assets.Scripts.Commons;
using Unity.VisualScripting;
using Assets.Scripts.Components.Windows.Inventory;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        public bool isRotate;
        public bool isGridOn;

        private int localRow;
        private int localCol;
        private Vector3 rayPos;
        private Vector3 mousePos;
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
                    if (ApplyActionForAllSlots(destSlot, (row, col) =>
                    {
                        return WindowInventoryController.InventorySlots[row, col].ItemTf != null;
                    }))
                    {
                        return false;
                    }
                    return true;
                case ContentType.Looting:
                    if (ApplyActionForAllSlots(destSlot, (row, col) =>
                    {
                        return WindowInventoryController.LootSlots[row, col].ItemTf != null;
                    }))
                    {
                        return false;
                    }
                    return true;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
            }
            return true;
        }

        /// <summary>
        /// 임시로 가능성 있는 타일 불 켜기/끄기 함수
        /// </summary>
        /// <param name="isOn"></param>
        private void ConsiderNextSlot(bool isOn)
        {
            switch (nextSlot.ContainerType)
            {
                case ContentType.Inventory:
                    ApplyActionForAllSlots(nextSlot, (r, c) =>
                    {
                        WindowInventoryController.InventorySlots[r, c].IsConsidered = isOn;
                    });
                    return;
                case ContentType.Looting:
                    ApplyActionForAllSlots(nextSlot, (r, c) =>
                    {
                        WindowInventoryController.LootSlots[r, c].IsConsidered = isOn;
                    });
                    return;
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
            // 부착하려고 하는 컨테이너의 타입?
            switch (attachSlot.ContainerType)
            {
                case ContentType.Inventory:
                    // 위치 잡기
                    objTF.localPosition = new Vector3(-25, 25, -1);
                    // 부착
                    ApplyActionForAllSlots(attachSlot, (row, col) =>
                    {
                        WindowInventoryController.InventorySlots[row, col].ItemTf = transform;
                    });
                    break;
                case ContentType.Looting:
                    // 위치 잡기
                    objTF.localPosition = new Vector3(-25, 25, -1);
                    // 부착
                    ApplyActionForAllSlots(attachSlot, (row, col) =>
                    {
                        WindowInventoryController.LootSlots[row, col].ItemTf = transform;
                    });
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
            switch (curSlot.ContainerType)
            {
                case ContentType.Inventory:
                    ApplyActionForAllSlots(curSlot, (row, col) =>
                    {
                        WindowInventoryController.InventorySlots[row, col].ItemTf = null;
                    });
                    break;
                case ContentType.Looting:
                    ApplyActionForAllSlots(curSlot, (row, col) =>
                    {
                        WindowInventoryController.LootSlots[row, col].ItemTf = null;
                    });
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
            //// 돌리는 의미가 없는 아이템이면 return
            //if (localRow == localCol)
            //    return;
            //// 아이템 하단의 흰색 칸 헤제
            //UnCheckReady(readySlot);
            //// 현재 돌아가 있는지 확인해서 방향 결정
            //if (isRotate)
            //    image.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            //else
            //    image.rectTransform.rotation = Quaternion.Euler(0, 0, 90f);
            //// itemsize 변경
            //int tempSize;
            //tempSize = localCol;
            //localCol = localRow;
            //localRow = tempSize;
            //// recttransform.sizeDelta 변경
            //objTF.sizeDelta = new Vector2(objTF.sizeDelta.y, objTF.sizeDelta.x);
            //// isRotate 변경
            //isRotate = !isRotate;
        }

        /// <summary>
        /// 아이템이 그리드 위에 있는지 확인
        /// </summary>
        public void GridOnCheck(InventorySlotController checkSlot)
        {
            switch (checkSlot.ContainerType)
            {
                case ContentType.Inventory:
                case ContentType.Looting:
                    isGridOn = true;
                    break;
                case ContentType.None_L:
                    break;
                case ContentType.None_C:
                    break;
                case ContentType.None_R:
                    break;
                case ContentType.Undefined:
                    break;
            }
        }

        protected override void OnDown()
        {
            ItemDetach();
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
            //// 드래그 중 I키 누르면 놓아버림
            //if (Input.GetKeyDown(KeyCode.I))
            //{
            //    Debug.Log("작동 됨");
            //    OnUp();
            //    isMouseDown = false;
            //    return;
            //}
            //// 마우스 드래그 이벤트
            objTF.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objTF.localPosition = new Vector3(
                objTF.localPosition.x - (transform.GetComponent<BoxCollider>().size.x / 2),
                objTF.localPosition.y + (transform.GetComponent<BoxCollider>().size.y / 2),
                0f
                );
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 마우스 위치 기준으로 아래 그리드인지 아닌지 확인
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down, out RaycastHit hit, 2f, GlobalStatus.Constant.slotMask))
            {
                InventorySlotController tempSlot;
                tempSlot = hit.transform.GetComponent<InventorySlotController>();
                GridOnCheck(tempSlot);
            }
            else
            {
                isGridOn = false;
            }
            if (isGridOn)
            {
                rayPos = transform.TransformPoint(new Vector3(25, -25, -1));
            }
            else
            {
                rayPos = mousePos;
            }
            if (Physics.Raycast(rayPos, Vector3.down, out RaycastHit hitValid, 2f, GlobalStatus.Constant.slotMask))
            {
                // 아래에 후보 슬롯 존재
                InventorySlotController candidateSlot = hitValid.transform.GetComponent<InventorySlotController>();

                // 배치 가능 여부 판단
                if (ItemSizeCheck(candidateSlot))
                {
                    // 배치 가능
                    if (nextSlot != null && nextSlot.Equals(candidateSlot))
                    {
                        // 이전 배치 가능 슬롯하고 동일
                        // = 별거 안함
                    } else
                    {
                        if (nextSlot != null)
                        {
                            nextSlot.IsConsidered = false;
                            ConsiderNextSlot(false);
                        }
                        // 신규 배치 가능 슬롯임
                        // = 후보 등록 + 활성화
                        nextSlot = candidateSlot;
                        nextSlot.IsConsidered = true;
                        ConsiderNextSlot(true);
                    }
                } else
                {
                    // 배치 불가
                    if (nextSlot != null)
                    {
                        nextSlot.IsConsidered = false;
                        ConsiderNextSlot(false);
                    }
                    nextSlot = null;
                }
            } else
            {
                // 아래에 후보 슬롯 없음
                // 배치 불가
                if (nextSlot != null)
                {
                    nextSlot.IsConsidered = false;
                    ConsiderNextSlot(false);
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
            } else
            {
                // 없음
                // = 이전 위치로 롤백
                ItemAttach(prevSlot);
            }
            //Collider2D hit;
            //// 마우스 위치 기준으로 아래 그리드인지 아닌지 확인
            //if (hit = Physics2D.OverlapPoint(mousePos, GlobalStatus.Constant.slotMask))
            //{
            //    InventorySlotController tempSlot;
            //    tempSlot = hit.transform.GetComponent<InventorySlotController>();
            //    GridOnCheck(tempSlot);
            //    if (tempSlot.isAttached)
            //    {
            //        GridOnCheckIfItemExist(tempSlot);
            //    }
            //}
            //// 슬롯 미감지시 그리드 위는 어차피 아님
            //else
            //{
            //    isGridOn = false;
            //}
            //// 앞서 체크 결과, Grid 위에 있으면
            //if (isGridOn)
            //{
            //    rayPos = transform.TransformPoint(new Vector2(30f, -30f));
            //}
            //else
            //{
            //    rayPos = mousePos;
            //}
            //if (hit = Physics2D.OverlapPoint(rayPos, GlobalStatus.Constant.slotMask))
            //{
            //    // 아이템 사이즈 체크
            //    if (ItemSizeCheck(hit.transform.GetComponent<InventorySlotController>()))
            //    {
            //        // 슬롯 타입 체크
            //        if (CheckItemTag(hit.transform.GetComponent<InventorySlotController>(), isGridOn))
            //        {
            //            ItemAttach(hit.transform.GetComponent<InventorySlotController>());
            //        }
            //        else
            //        {
            //            ReturnToPost();
            //            UnCheckReady(readySlot);
            //        }
            //    }
            //    else
            //    {
            //        ReturnToPost();
            //    }
            //}
            //else
            //{
            //    ReturnToPost();
            //}
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
            switch (_type)
            {
                case ContentType.Inventory:
                case ContentType.Looting:
                    // 인벤토리 또는 루팅
                    // = 사이즈: 50 * 50
                    image.rectTransform.sizeDelta = objCollider.size = objTF.sizeDelta = baseInfo.size * 50f;
                    objCollider.center = new Vector3(objCollider.size.x / 2, -objCollider.size.y / 2, 0);
                    break;
                case ContentType.None_L:
                    break;
                case ContentType.None_C:
                    break;
                case ContentType.None_R:
                    break;
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
        /// 아래 모든 슬롯에 액션 반복 함수: 반환 있음
        /// </summary>
        /// <param name="actionToApply"></param>
        private TReturn ApplyActionForAllSlots<TReturn>(InventorySlotController startSlot, Func<int, int, TReturn> actionToApply)
        {
            for (int i = 0; i < localCol; i++)
            {
                for (int j = 0; j < localRow; j++)
                {
                    try
                    {
                        return actionToApply(startSlot.row + i, startSlot.column + j);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return (TReturn)(object)false;
                    }
                }
            }
            return (TReturn)(object)false;
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
