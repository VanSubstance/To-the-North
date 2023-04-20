using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemBaseInfo : ScriptableObject
    {
        [HideInInspector]
        public ItemType itemType;
        public Vector2 size;
        public string imagePath;
        public string title;
        public string description;
        public int price;
    }
}
