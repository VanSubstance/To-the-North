using TMPro;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemConsumableInfo : ItemBaseInfo
    {
        public ConsumbableType consumableType;

        [SerializeField]
        /// <summary>
        /// 현재 잔여량
        /// 최대치: 60
        /// </summary>
        private int amountCurrent = 60;

        [SerializeField]
        protected int secondConsume;
        public int SecondConsume
        {
            get
            {
                return secondConsume;
            }
        }

        /// <summary>
        /// 표기되는 잔여량
        /// </summary>
        public int AmountCount
        {
            get
            {
                return amountCurrent;
            }
            set
            {
                amountCurrent = value >= 0 ? value : 0;
                if (amountDisplay)
                {
                    amountDisplay.text = amountCurrent.ToString();
                }
            }
        }

        private TextMeshProUGUI amountDisplay;
        public TextMeshProUGUI AmountDisplay
        {
            set
            {
                amountDisplay = value;
                if (amountDisplay)
                {
                    amountDisplay.text = amountCurrent.ToString();
                    amountDisplay.gameObject.SetActive(true);
                }
            }
        }

        public void Use(int quantity)
        {
            AmountCount -= quantity;
            if (AmountCount == 0)
            {
                // 아이템 파기
                Ctrl.ItemTruncate();
            }
        }
    }
}
