using UnityEngine;

namespace Assets.Scripts.Items.Abstracts
{
    /// <summary>
    /// 아이템 베이스 컨트롤러
    /// 드래그 앤 드롭: 이동
    ///     !! 드래그 앤 드롭 시, 아래 있는 칸에 얘가 장착이 가능한 지 조건을 판별해야 함
    /// 마우스 위에서 유지: 정보 뜨기
    /// 더블클릭: 추후 추상 구현
    /// </summary>
    public abstract class AItemBaseController : MonoBehaviour
    {
        [SerializeField]
        InventorySlotController curSlot;
        InventorySlotController readySlot;

        public int itemSizeRow;
        public int itemSizeCol;

        private Vector2 rayPos;
        private bool isMouseIn;
        private RectTransform objTF;

        private void Start()
        {
            objTF = GetComponent<RectTransform>();
            // 초기화
            isMouseIn = false;
            // BoxCollider2D에 RectTransform 사이즈 대입
            GetComponent<BoxCollider2D>().size = GetComponent<RectTransform>().sizeDelta;
            // 시작 curSlot 초기화 (ray 사용, rayPos = 게임오브젝트 좌상단 기준 30f, -30f)
            rayPos = transform.TransformPoint(new Vector2(30f, -30f));
            RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.forward, 10f, GlobalStatus.Constant.slotMask);
            if (hit.collider)
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
                        if (InventoryManager.inventorySlots[destSlot.row + j, destSlot.column + i].isAttached == true)
                        {
                            return false;
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
            Vector3 destPos;
            destPos = new Vector3(attachSlot.transform.localPosition.x, attachSlot.transform.localPosition.y, 0f);
            objTF.localPosition = destPos;
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    InventoryManager.inventorySlots[attachSlot.row + j, attachSlot.column + i].isAttached = true;
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
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    InventoryManager.inventorySlots[detachSlot.row + j, detachSlot.column + i].isAttached = false;
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
                    InventoryManager.inventorySlots[readySlot.row + j, readySlot.column + i].isAttachReady = true;
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
                    InventoryManager.inventorySlots[readySlot.row + j, readySlot.column + i].isAttachReady = false;
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

        private void OnMouseDown()
        {
            isMouseIn = true;
            ItemDetach(curSlot);
        }

        private void OnMouseDrag()
        {
            if (isMouseIn)
            {
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
                }
            }
        }

        private void OnMouseUp()
        {
            if (isMouseIn)
            {
                rayPos = transform.TransformPoint(new Vector2(30f, -30f));
                RaycastHit2D hit = Physics2D.Raycast(rayPos, transform.forward, 10f, GlobalStatus.Constant.slotMask);
                if (hit.collider)
                {
                    if (ItemSizeCheck(hit.transform.GetComponent<InventorySlotController>()))
                    {
                        // 칸 조건은 만족
                        if (CheckItemTag())
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
            isMouseIn = false;
            if (false)
            {
                ExecuteDoubleClick();
            }
            // 뭐가 되었던 조건을 만족해서 더블클릭 이벤트가 실행되어야됨
        }

        /// <summary>
        /// 아이템을 사용하는 함수
        /// </summary>
        public abstract void ExecuteDoubleClick();

        /// <summary>
        /// 아이템이 해당 칸에 설치될 수 있는지 체크하는 함수
        /// </summary>
        /// <returns></returns>
        protected abstract bool CheckItemTag();
    }
}
