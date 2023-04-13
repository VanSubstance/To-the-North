
using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Material Info", menuName = "Data Objects/Items/Material", order = int.MaxValue)]
    [Serializable]
    public class ItemMaterialInfo : ItemBaseInfo
    {
        // 재료 아이템 필요 인포
        [HideInInspector]
        public new ItemType itemType
        {
            get => ItemType.Material;
        }
    }
}
