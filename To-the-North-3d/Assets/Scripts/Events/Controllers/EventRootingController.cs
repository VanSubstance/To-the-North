using Assets.Scripts.Events.Abstracts;
using Assets.Scripts.Items;
using Assets.Scripts.Components.Windows.Inventory;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    public class EventRootingController : AbsEventBaseController
    {
        [SerializeField]
        private bool isItemFixed;
        [SerializeField]
        private ItemBaseInfo[] itemsFixed;
        [SerializeField]
        private int amountIfRandom;
        [SerializeField]
        private ItemInfoWithWeight[] itemsRandom;

        private ItemBaseInfo[] itemsLoot;
        /// <summary>
        /// 실제 루팅 아이템 생성하기
        /// </summary>
        protected new void Awake()
        {
            base.Awake();
            if (isItemFixed)
            {
                itemsLoot = new ItemBaseInfo[itemsFixed.Length];
                for (int i = 0; i < itemsFixed.Length; i++)
                {
                    itemsLoot[i] = Instantiate(itemsFixed[i]);
                }
            } else
            {

            }
        }

        /// <summary>
        /// SpaceBar 눌렀을 때 작동하는 함수
        /// </summary>
        public override void OnInteraction()
        {
            // 띄워줄 때, 이전에 이미 습득한 아이템은 건너뛴다
            foreach (ItemBaseInfo _itemInfo in itemsLoot)
            {
                if (_itemInfo.Ctrl != null) continue;
                WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Looting, _itemInfo);
            }
            WindowInventoryController.Instance.Open();
        }

        [System.Serializable]
        private class ItemInfoWithWeight
        {
            public ItemBaseInfo info;
            public int weight;
        }
    }
}
