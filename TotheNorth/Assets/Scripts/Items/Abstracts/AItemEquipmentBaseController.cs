using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Items.Abstracts
{
    /// <summary>
    /// 장비 아이템 베이스 컨트롤러
    /// 장비 아이템들의 마우스 이벤트는 아래와 같다:
    /// - 왼쪽 드래그: 옮기기
    /// - 왼쪽 클릭: ??
    /// - 오른쪽 클릭: 정보 보기?
    /// </summary>
    internal abstract class AItemEquipmentBaseController<TEquipmentType> : AItemBaseController
    {
        [SerializeField]
        InventorySlotController curSlot;
        
        public int itemSizeRow;
        public int itemSizeCol;

        private Vector3 rayPos;
        private bool isMouseIn;

        private void Start()
        {
            // 초기화
            isMouseIn = false;
            // BoxCollider에 RectTransform 사이즈 대입
            GetComponent<BoxCollider>().size = GetComponent<RectTransform>().sizeDelta;
            // 시작 curSlot 초기화 (ray 사용, rayPos = 게임오브젝트 좌상단 기준 30f, -30f)
            rayPos = transform.TransformPoint(new Vector3(30f, -30f, 0));
            RaycastHit hit;
            if (Physics.Raycast(rayPos, Vector3.forward, out hit, 40f, GlobalStatus.Constant.slotMask))
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
            Vector3 destPos = new Vector3(attachSlot.transform.position.x, attachSlot.transform.position.y, transform.position.z);
            transform.position = destPos;
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    InventoryManager.inventorySlots[attachSlot.row + j, attachSlot.column + i].isAttached = true;
                }
            }
            curSlot = attachSlot;
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

        public void ReturnToPost()
        {
            Vector3 postPos;
            postPos = new Vector3(curSlot.transform.position.x, curSlot.transform.position.y, transform.position.z);
            transform.position = postPos;
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
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.localPosition = new Vector3(
                    transform.localPosition.x - (transform.GetComponent<BoxCollider>().size.x / 2),
                    transform.localPosition.y + (transform.GetComponent<BoxCollider>().size.y / 2),
                    0
                    );
            }
        }

        private void OnMouseUp()
        {
            if (isMouseIn)
            {
                RaycastHit hit;
                rayPos = transform.TransformPoint(new Vector3(30f, -30f, 0));
                if (Physics.Raycast(rayPos, Vector3.forward, out hit, 40f, GlobalStatus.Constant.slotMask))
                {
                    if (ItemSizeCheck(hit.transform.GetComponent<InventorySlotController>()))
                    {
                        ItemAttach(hit.transform.GetComponent<InventorySlotController>());
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
        }

        /// <summary>
        /// 해당 장비의 정보 확인을 위한 정보 전달 함수
        /// </summary>
        /// <returns>해당 장비의 정보</returns>
        public abstract TEquipmentType GetItemEquipmentInfo();

        public override void OnLeftMouseDown(Vector3 mousePosition)
        {
        }
        public override void OnLeftMouseClick(Vector3 mousePosition)
        {
        }
        public override void OnLeftMouseDrag(Vector3 mousePosition)
        {
        }
        public override void OnRightMouseClick(Vector3 mousePosition)
        {
        }
        public override void OnLeftMouseUp(Vector3 mousePosition)
        {
        }
    }
}
