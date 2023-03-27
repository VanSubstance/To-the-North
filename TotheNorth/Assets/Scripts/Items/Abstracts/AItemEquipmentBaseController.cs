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
        InventorySlotController currentSlot;
        Transform destSlot;
        Transform postSlot;
        public Vector2[] itemSizes;
        private Vector3 rayPos;
        private bool isMouseIn = false;

        private void Start()
        {
            GetComponent<BoxCollider>().size = GetComponent<RectTransform>().sizeDelta;
            RaycastHit hit;
            rayPos = transform.TransformPoint(new Vector3(30f, -30f, 0));
            if (Physics.Raycast(rayPos, Vector3.forward, out hit, 40f, GlobalStatus.Constant.slotMask))
            {
                postSlot = hit.transform;
                hit.transform.GetComponent<InventorySlotController>().isAttached = true;
                currentSlot = hit.transform.GetComponent<InventorySlotController>();
            }
        }
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

        public bool SlotSizeCheck(Transform destSlot)
        {
            InventorySlotController temp = destSlot.GetComponent<InventorySlotController>();
            int row = temp.row;
            int col = temp.column;
            // Debug.Log("index = " + row + "," + col);
            // Debug.Log("itemSize = " + itemSizeRow + "," + itemSizeCol);
            // Debug.Log("destSlot.isAttached = " + temp.isAttached);
            /*
            for (int i = 0; i < itemSizeCol; i++)
            {
                for (int j = 0; j < itemSizeRow; j++)
                {
                    // Debug.Log("checkIndex = " + (row + j) + "," + (col + i));
                    // Debug.Log("checkSlot.isAttached = " +
                    //     InventoryManager.inventorySlots[row + j, col + i].isAttached);
                    try
                    {
                        if (InventoryManager.inventorySlots[row + j, col + i].isAttached == true)
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
            */
            foreach (Vector2 curSlot in itemSizes)
            {
                try
                {
                    if (InventoryManager.inventorySlots[row + (int)curSlot.x, col + (int)curSlot.y].isAttached == true)
                    {
                        return false;
                    }
                }
                catch (System.IndexOutOfRangeException)
                {
                    return false;
                }
            }
            return true;
        }

        public void ReturnToPost()
        {
            Vector3 postPos;
            postPos = new Vector3(postSlot.position.x, postSlot.position.y, transform.position.z);
            transform.position = postPos;
        }

        private void OnMouseDown()
        {
            isMouseIn = true;
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
                Debug.Log(transform.name);
                Debug.Log("OnMouseUp");
                Debug.Log(postSlot.GetComponent<InventorySlotController>().row);
                Debug.Log(postSlot.GetComponent<InventorySlotController>().column);
                RaycastHit hit;
                rayPos = transform.TransformPoint(new Vector3(30f, -30f, 0));
                if (Physics.Raycast(rayPos, Vector3.forward, out hit, 40f, GlobalStatus.Constant.slotMask))
                {
                    Debug.Log("ray hit");
                    destSlot = hit.transform;
                    if (SlotSizeCheck(destSlot))
                    {
                        Debug.Log("true");
                        Vector3 destPos;
                        destPos = new Vector3(destSlot.position.x, destSlot.position.y, transform.position.z);
                        transform.position = destPos;
                        destSlot.GetComponent<InventorySlotController>().isAttached = true;
                        currentSlot = destSlot.transform.GetComponent<InventorySlotController>();
                        postSlot.GetComponent<InventorySlotController>().isAttached = false;
                        postSlot = destSlot;
                    }
                    else
                    {
                        Debug.Log("false");
                        ReturnToPost();
                    }
                }
                else
                {
                    Debug.Log("false");
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
    }
}
