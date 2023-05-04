using UnityEngine;
using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentSlotController : ContentBaseController
    {
        public InventorySlotController[,] slots;
        [SerializeField]
        private InventorySlotController slotTf;
        private readonly Vector2 sizeSlot = new Vector2(14, 10);

        protected void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (slots != null) return;
            slots = new InventorySlotController[(int)sizeSlot.x, (int)sizeSlot.y];
            // i = x = row
            for (int i = 0; i < sizeSlot.x; i++)
            {
                // j = y = column
                for (int j = 0; j < sizeSlot.y; j++)
                {
                    slots[i, j] = Instantiate(slotTf, transform);
                    slots[i, j].SetLocation(i, j);
                }
            }
        }

        /// <summary>
        /// 아이템 오브젝트 생성 함수
        /// </summary>
        /// <param name="_info">아이템 정보</param>
        public void GenerateItem(ItemGenerateController gen, ItemInventoryInfo _info)
        {
            Init();
            gen.InitItem(_info.itemInfo, slots[_info.row, _info.col]);
        }

        /// <summary>
        /// 아이템 오브젝트 생성 함수:
        /// 슬롯 없음
        /// </summary>
        /// <param name="_info">아이템 정보</param>
        public ItemInventoryInfo GenerateItemWithAuto(ItemGenerateController gen, ItemBaseInfo _info = null, ContentType _type = ContentType.Undefined)
        {
            Init();
            // 슬롯 지정이 없을 경우 = 자동 정렬 필요
            return gen.InitItem(_info, type: _type);
        }
    }
}
