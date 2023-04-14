
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableInfo : ItemBaseInfo
    {
        [HideInInspector]
        public new ItemType itemType
        {
            get
            {
                return ItemType.Consumable;
            }
        }
        [HideInInspector]
        public ConsumbableType consumbableType;
    }
}
