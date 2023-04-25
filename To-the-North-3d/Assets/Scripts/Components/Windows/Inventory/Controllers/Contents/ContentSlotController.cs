using UnityEngine;
using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContentSlotController : ContentBaseController
    {
        public InventorySlotController[,] slots;
        [SerializeField]
        private InventorySlotController slotTf;
        private readonly Vector2 sizeSlot = new Vector2(10, 14);

        protected void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (slots != null) return;
            slots = new InventorySlotController[(int)sizeSlot.x, (int)sizeSlot.y];
            for (int i = 0; i < sizeSlot.y; i++)
            {
                for (int j = 0; j < sizeSlot.x; j++)
                {
                    slots[j, i] = Instantiate(slotTf, transform);
                    slots[j, i].SetLocation(i, j);
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
            Debug.Log(slots);
            gen.transform.position = new Vector3
            (slots[_info.x, _info.y].transform.position.x,
            slots[_info.x, _info.y].transform.position.y,
            transform.position.z);
            gen.InitItem(_info.itemInfo, _info);
        }
    }
}
