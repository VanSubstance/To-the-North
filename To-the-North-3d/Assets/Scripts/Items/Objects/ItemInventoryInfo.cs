using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemInventoryInfo
    {
        public ItemBaseInfo itemInfo;
        // {row, col}
        public Vector2 pos;
        public int row
        {
            get => (int)pos.x;
        }
        public int col
        {
            get => (int)pos.y;
        }

        public ItemInventoryInfo()
        {
            itemInfo = null;
            pos = Vector2.zero;
        }

        public ItemInventoryInfo(ItemBaseInfo _itemInfo)
        {
            itemInfo = _itemInfo;
            pos = Vector2.zero;
        }
    }
}
