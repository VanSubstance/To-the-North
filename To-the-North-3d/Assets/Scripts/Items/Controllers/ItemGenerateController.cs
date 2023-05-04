using System;
using Assets.Scripts.Components.Windows.Inventory;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemGenerateController : MonoBehaviour
    {
        private const string TAG = "아이템 종류 별 오브젝트 재생산기";

        /// <summary>
        /// 아이템의 정보가 넘어왔을 때, 해당 아이템의 타입에 맞게끔 오브젝트 초기화
        /// </summary>
        /// <param name="itemInfo">아이템 정보</param>
        /// <returns>자동 정렬이 된 경우: 신규 ItemInventorySlot 반환; 자동 정렬이 아닌 경우: null 반환</returns>
        public ItemInventoryInfo InitItem(ItemBaseInfo itemInfo, InventorySlotController slotToAttach = null, ContentType type = ContentType.Undefined)
        {
            switch (itemInfo)
            {
                case ItemMaterialInfo info:
                    return transform.AddComponent<ItemMaterialController>().InitInfo(info, slotToAttach, type);
                case ItemEquipmentInfo info:
                    return transform.AddComponent<ItemEquipmentController>().InitInfo(info, slotToAttach, type);
                case ItemConsumableInfo info:
                    return transform.AddComponent<ItemConsumableController>().InitInfo(info, slotToAttach, type);
            }
            return null;
        }
    }
}
