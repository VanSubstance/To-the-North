using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Events.Abstracts;
using Assets.Scripts.Items;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Events.Controllers
{
    public class EventLootingController : AbsEventBaseController
    {
        [SerializeField]
        private int currency;
        [SerializeField]
        private ItemBaseInfo[] itemsFixed;
        [SerializeField]
        private int amountIfRandom;
        [SerializeField]
        private ItemInfoWithWeight[] itemsRandom;

        [HideInInspector]
        public ItemBaseInfo[] itemsLoot;
        /// <summary>
        /// 실제 루팅 아이템 생성하기
        /// </summary>
        protected new void Awake()
        {
            base.Awake();
            if (itemsFixed.Length > 0)
            {
                itemsLoot = new ItemBaseInfo[itemsFixed.Length];
                for (int i = 0; i < itemsFixed.Length; i++)
                {
                    itemsLoot[i] = Instantiate(itemsFixed[i]);
                }
                itemsFixed = null;
                return;
            }

            itemsLoot = new ItemBaseInfo[amountIfRandom];
            int t = 0;
            foreach (ItemInfoWithWeight _info in itemsRandom)
            {
                _info.weight = t += _info.weight;
            }
            int r, idx = 0;
            while (idx < amountIfRandom)
            {
                r = Random.Range(0, t);
                foreach (ItemInfoWithWeight _info in itemsRandom)
                {
                    if (r < _info.weight)
                    {
                        itemsLoot[idx] = Instantiate(_info.info);
                        idx++;
                        break;
                    }
                }
            }
            itemsRandom = null;
            return;
        }

        /// <summary>
        /// SpaceBar 눌렀을 때 작동하는 함수
        /// </summary>
        public override void OnInteraction()
        {
            InGameStatus.Currency = currency;
            currency = 0;
            // 띄워줄 때, 이전에 이미 습득한 아이템은 건너뛴다
            foreach (ItemBaseInfo _itemInfo in itemsLoot)
            {
                if (_itemInfo.Ctrl != null) continue;
                WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Looting, _itemInfo);
            }
            WindowInventoryController.Instance.Open(this);
        }

        [System.Serializable]
        private class ItemInfoWithWeight
        {
            public ItemBaseInfo info;
            public int weight;
        }
    }
}
