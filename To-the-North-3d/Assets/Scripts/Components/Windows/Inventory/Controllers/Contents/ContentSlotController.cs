using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Items;
using Assets.Scripts.Commons.Objects;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentSlotController : ContentBaseController
    {
        public InventorySlotController[,] slots;
        [SerializeField]
        private InventorySlotController slotTf;
        private readonly Vector2 sizeSlot = new Vector2(14, 10);
        public ListWithListener<ItemInventoryInfo> itemsAttached = new ListWithListener<ItemInventoryInfo>();

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
            return gen.InitItem(_info, type: _type);
        }

        /// <summary>
        /// 현재 슬롯을 비우고 적재되어있던 아이템 어레이 반환
        /// </summary>
        /// <returns></returns>
        public ItemBaseInfo[] Clear()
        {
            ItemBaseInfo[] res = itemsAttached.ConvertAll((_info) => _info.itemInfo).ToArray();
            foreach (ItemInventoryInfo _info in new List<ItemInventoryInfo>(itemsAttached))
            {
                if (_info.itemInfo == null) continue;
                _info.itemInfo.Ctrl.ItemTruncate();
            }
            itemsAttached.Clear();
            return res;
        }
    }
}
