using TMPro;
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

        [SerializeField]
        /// <summary>
        /// 현재 잔여량
        /// 최대치: 60
        /// </summary>
        private int amountCurrent = 60;

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
    }
}
