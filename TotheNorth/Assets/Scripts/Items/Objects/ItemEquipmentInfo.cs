
using System;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class ItemEquipmentInfo : ItemBaseInfo
    {
        public EquipmentType equipmentType;
        public EquipPartType equipPartType;
        // 장착가능 아이템 필요 인포
        public new ItemType itemType
        {
            get
            {
                return ItemType.Equipment;
            }
        }
    }
}