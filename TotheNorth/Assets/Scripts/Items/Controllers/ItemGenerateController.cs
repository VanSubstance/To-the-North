using System;
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
        public void InitItem(ItemBaseInfo itemInfo)
        {
            try
            {
                // 재료 아이템
                ItemMaterialInfo materialInfo = (ItemMaterialInfo)itemInfo;
            }
            catch (InvalidCastException)
            {
                try
                {
                    // 장비 아이템
                    ItemEquipmentInfo equipmentInfo = (ItemEquipmentInfo)itemInfo;
                }
                catch (InvalidCastException)
                {
                    try
                    {
                        // 소모성 아이템
                        ItemConsumableInfo consumableInfo = (ItemConsumableInfo)itemInfo;
                    }
                    catch (InvalidCastException)
                    {
                        // 아이템 데이터 타입이 뭔가 단단히 잘못된게 틀림이 없음
                    }
                }
            }
        }
    }
}
