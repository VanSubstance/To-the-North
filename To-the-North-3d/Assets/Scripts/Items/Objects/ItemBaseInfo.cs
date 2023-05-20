using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemBaseInfo : ScriptableObject
    {
        [HideInInspector]
        public ItemType itemType;
        /// <summary>
        /// x: row, y: col
        /// </summary>
        public Vector2 size;
        public string imagePath;
        [SerializeField]
        private string titleKor, titleEng;
        public string Title
        {
            get
            {
                return GlobalSetting.Language == "Kor" ? titleKor : titleEng;
            }
        }
        public string description;
        public int price, weight, weightExtension;

        /// <summary>
        /// 장비로 착용이 가능한지 여부 반환
        /// </summary>
        public bool IsEquipment
        {
            get
            {
                return this is ItemArmorInfo || this is ItemWeaponInfo;
            }
        }


        /// <summary>
        /// 퀵슬롯에 착용이 가능한지 여부 반환
        /// </summary>
        public bool IsQuickable
        {
            get
            {
                return this is ItemFoodInfo || this is ItemMedicineInfo;
            }
        }

        /// <summary>
        /// 아이템이 현재 장착된 인벤토리 정보:
        /// 역추적이 필요함
        /// </summary>
        private ItemInventoryInfo invenInfo;
        public ItemInventoryInfo InvenInfo
        {
            get
            {
                return invenInfo;
            }
            set
            {
                if (invenInfo != null && invenInfo.itemInfo != null)
                {
                    invenInfo.itemInfo = null;
                }
                invenInfo = value;
            }
        }

        private AItemBaseController ctrl;
        public AItemBaseController Ctrl
        {
            get
            {
                return ctrl;
            }
            set
            {
                ctrl = value;
            }
        }
    }
}
