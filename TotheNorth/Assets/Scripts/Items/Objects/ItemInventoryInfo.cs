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
