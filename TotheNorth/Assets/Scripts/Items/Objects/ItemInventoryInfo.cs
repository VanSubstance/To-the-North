using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemInventoryInfo
    {
        public ItemBaseInfo itemInfo;
        public Vector2 pos;
        public int x
        {
            get => (int)pos.x;
        }
        public int y
        {
            get => (int)pos.y;
        }
    }
}
