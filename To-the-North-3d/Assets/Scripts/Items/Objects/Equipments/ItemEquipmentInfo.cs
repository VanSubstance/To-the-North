
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemEquipmentInfo : ItemBaseInfo
    {
        [HideInInspector]
        public EquipmentType equipmentType;
        //public EquipBodyType equipPartType;
        // 장착가능 아이템 필요 인포
        [HideInInspector]
        public new ItemType itemType
        {
            get
            {
                return ItemType.Equipment;
            }
        }

        [SerializeField]
        [Range(0, 1)]
        private float durability = 1;
        public float Durability
        {
            get
            {
                return durability;
            }

            set
            {
                durability = value;
            }
        }
    }
}