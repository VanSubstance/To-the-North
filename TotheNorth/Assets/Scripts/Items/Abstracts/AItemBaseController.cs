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
    public abstract class AItemBaseController : AbsItemController
    {
        [SerializeField]
        InventorySlotController curSlot;
        InventorySlotController readySlot;

        public int itemSizeRow;
        public int itemSizeCol;
        public bool isRotate;

        private Vector2 rayPos;
        private RectTransform objTF;
        private BoxCollider2D objCollider;
        private Image image;

        private ItemBaseInfo baseInfo;
        private void Awake()
        {
            image = GetComponentInChildren<Image>();
            objTF = GetComponent<RectTransform>();
            objCollider = GetComponent<BoxCollider2D>();
            baseInfo = GetBaseInfo();
            SetImage(baseInfo.imagePath);
        }

        private void Start()
        {
            // BoxCollider2D에 RectTransform 사이즈 대입
            objCollider.size = objTF.sizeDelta;
            objCollider.offset = new Vector2(objTF.sizeDelta.x / 2f, objTF.sizeDelta.y / -2f);
            // 시작 curSlot 초기화 (ray 사용, rayPos = 게임오브젝트 좌상단 기준 30f, -30f)
            rayPos = transform.TransformPoint(new Vector2(30f, -30f));
            RaycastHit2D hit;
            if ((hit = Physics2D.Raycast(rayPos, transform.forward, 10f, GlobalStatus.Constant.slotMask)) && hit.collider)
            {
                curSlot = hit.transform.GetComponent<InventorySlotController>();
                ItemAttach(curSlot);
            }
        }

        /// <summary>
        ///  ItemSize만큼 destSlot 주변의 Slot들 검사
        /// </summary>
        public bool ItemSizeCheck(InventorySlotController destSlot)
        {
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    try
                    {
                        if (destSlot.slotType == SlotType.Inventory)
                        {
                            if (InventoryManager.inventorySlots[destSlot.row + j, destSlot.column + i].isAttached == true)
                            {
                                return false;
                            }
                        }
                        else if (destSlot.slotType == SlotType.Rooting)
                        {
                            if (InventoryManager.rootSlots[destSlot.row + j, destSlot.column + i].isAttached == true)
                            {
                                return false;
                            }
                        }
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// attachSlot에 Item 부착
        /// </summary>
        public void ItemAttach(InventorySlotController attachSlot)
        {
            if (attachSlot.slotType == SlotType.Inventory)
                transform.SetParent(InventoryManager.rightInventoryTF);
            if (attachSlot.slotType == SlotType.Shop || attachSlot.slotType == SlotType.Rooting)
                transform.SetParent(InventoryManager.leftInventoryTF);
            Vector3 destPos;
            destPos = new Vector3(attachSlot.transform.localPosition.x, attachSlot.transform.localPosition.y, 0f);
            objTF.localPosition = destPos;
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    if (attachSlot.slotType == SlotType.Inventory)
                    {
                        InventoryManager.inventorySlots[attachSlot.row + j, attachSlot.column + i].isAttached = true;
                    }
                    else if (attachSlot.slotType == SlotType.Rooting)
                    {
                        InventoryManager.rootSlots[attachSlot.row + j, attachSlot.column + i].isAttached = true;
                    }
                }
            }
            curSlot = attachSlot;
            UnCheckReady(attachSlot);
        }

        /// <summary>
        /// detachSlot에서 Item 분리
        /// </summary>
        /// <param name="detachSlot"></param>
        public void ItemDetach(InventorySlotController detachSlot)
        {
            transform.SetParent(InventoryManager.movingSpaeceTF);
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    if (detachSlot.slotType == SlotType.Inventory)
                    {
                        InventoryManager.inventorySlots[detachSlot.row + j, detachSlot.column + i].isAttached = false;
                    }
                    else if (detachSlot.slotType == SlotType.Rooting)
                    {
                        InventoryManager.rootSlots[detachSlot.row + j, detachSlot.column + i].isAttached = false;
                    }
                }
            }
        }

        /// <summary>
        /// itemSize만큼 slot들의 isAttachReady True로
        /// </summary>
        /// <param name="detachSlot"></param>
        public void CheckReady(InventorySlotController readySlot)
        {
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    if (readySlot.slotType == SlotType.Inventory)
                    {
                        InventoryManager.inventorySlots[readySlot.row + j, readySlot.column + i].isAttachReady = true;
                    }
                    if (readySlot.slotType == SlotType.Rooting)
                    {
                        InventoryManager.rootSlots[readySlot.row + j, readySlot.column + i].isAttachReady = true;
                    }
                }
            }
        }

        /// <summary>
        /// itemSize만큼 slot들의 isAttachReady False로
        /// </summary>
        /// <param name="detachSlot"></param>
        public void UnCheckReady(InventorySlotController readySlot)
        {
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    try
                    {
                        if (readySlot.slotType == SlotType.Inventory)
                        {
                            InventoryManager.inventorySlots[readySlot.row + j, readySlot.column + i].isAttachReady = false;

                        }
                        else if (readySlot.slotType == SlotType.Rooting)
                        {
                            InventoryManager.rootSlots[readySlot.row + j, readySlot.column + i].isAttachReady = false;
                        }
                    }
                    catch (System.NullReferenceException)
                    {
                        return;
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// curSlot으로 복귀
        /// </summary>
        public void ReturnToPost()
        {
            Vector3 postPos;
            postPos = new Vector3(curSlot.transform.localPosition.x, curSlot.transform.localPosition.y, 0f);
            objTF.localPosition = postPos;
            ItemAttach(curSlot);
        }

        /// <summary>
        /// 아이템 회전
        /// </summary>
        public void ItemRotate()
        {
            // 돌리는 의미가 없는 아이템이면 return
            if (itemSizeRow == itemSizeCol)
                return;
            // 아이템 하단의 흰색 칸 헤제
            UnCheckReady(readySlot);
            // 현재 돌아가 있는지 확인해서 방향 결정
            if (isRotate)
                image.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            else
                image.rectTransform.rotation = Quaternion.Euler(0, 0, 90f);
            // itemsize 변경
            int tempSize;
            tempSize = itemSizeCol;
            itemSizeCol = itemSizeRow;
            itemSizeRow = tempSize;
            // recttransform.sizeDelta 변경
            objTF.sizeDelta = new Vector2(objTF.sizeDelta.y, objTF.sizeDelta.x);
            // BoxCollider2D.size 변경
            objCollider.size = objTF.sizeDelta;
            // BoxCollider2D.offset 변경
            objCollider.offset = new Vector2(objTF.sizeDelta.x / 2f, objTF.sizeDelta.y / -2f);
            // isRotate 변경
            isRotate = !isRotate;
        }

        protected override void OnDown()
        {
            ItemDetach(curSlot);
            objTF.SetAsLastSibling();
        }

        protected override void OnDraging()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ItemRotate();
            }
            objTF.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objTF.localPosition = new Vector3(
                objTF.localPosition.x - (transform.GetComponent<BoxCollider2D>().size.x / 2),
                objTF.localPosition.y + (transform.GetComponent<BoxCollider2D>().size.y / 2),
                0f
                );
            rayPos = transform.TransformPoint(new Vector2(30f, -30f));
            RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.forward, 10f, GlobalStatus.Constant.slotMask);
            if (hit.collider)
            {
                InventorySlotController tempSlot;
                tempSlot = hit.transform.GetComponent<InventorySlotController>();
                if (readySlot == tempSlot && readySlot.isAttachReady == true)
                {
                    return;
                }
                else
                {
                    if (ItemSizeCheck(tempSlot))
                    {
                        if (readySlot != null)
                        {
                            UnCheckReady(readySlot);
                        }
                        readySlot = tempSlot;
                        CheckReady(readySlot);
                    }
                    else
                    {
                        UnCheckReady(readySlot);
                    }
                }
            }
            else
            {
                UnCheckReady(readySlot);
                readySlot = null;
            }
        }

        protected override void OnUp()
        {
            rayPos = transform.TransformPoint(new Vector2(30f, -30f));
            RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.forward, 10f, GlobalStatus.Constant.slotMask);
            if (hit.collider)
            {
                // 아이템 사이즈 체크
                if (ItemSizeCheck(hit.transform.GetComponent<InventorySlotController>()))
                {
                    // 슬롯 타입 체크
                    if (CheckItemTag(hit.transform.GetComponent<InventorySlotController>()))
                    {
                        ItemAttach(hit.transform.GetComponent<InventorySlotController>());
                    }
                }
                else
                {
                    ReturnToPost();
                }
            }
            else
            {
                ReturnToPost();
            }
        }

        private void SetImage(string imagePath)
        {
            image.sprite = Resources.Load<Sprite>(imagePath);
        }

        /// <summary>
        /// 아이템이 해당 칸에 설치될 수 있는지 체크하는 함수
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckItemTag(InventorySlotController slot);

        /// <summary>
        /// 자식의 데이터에서 공통된 아이템 처리에 필요한 데이터 추출하는 함수
        /// </summary>
        /// <returns></returns>
        protected abstract ItemBaseInfo GetBaseInfo();
    }
}
