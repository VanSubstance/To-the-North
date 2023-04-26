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
        public string title;
        public string description;
        public int price;

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
    }
}
