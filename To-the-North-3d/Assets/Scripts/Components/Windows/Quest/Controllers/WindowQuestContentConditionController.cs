using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Components.Windows
{
    public class WindowQuestContentConditionController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        private string targetCode, title;
        private int maxCount, curCount;

        private bool isTargetItem = true;
        /// <summary>
        /// 조건 초기화 함수
        /// </summary>
        /// <param name="_targetCode"></param>
        /// <param name="count"></param>
        /// <param name="isTargetItem">타겟이 아이템 -> true; 타겟이 몬스터 -> false</param>
        public void Init(string _targetCode, int count, bool _isTargetItem = true)
        {
            targetCode = _targetCode;
            isTargetItem = _isTargetItem;
            title = isTargetItem ? DataFunction.LoadItemInfoByCode(_targetCode).Title : "몬스터 이름";
            maxCount = count;
            curCount = Mathf.Min(maxCount, InGameStatus.Item.CountItemByCode(targetCode));
            Print();
        }

        private void Print()
        {
            text.text = $"{title}    {curCount} / {maxCount}";
        }

        public bool IsCleared
        {
            get
            {
               return InGameStatus.Item.CountItemByCode(targetCode) >= curCount;
            }
        }

        public void NoticeChange(bool _isTargetItem = true)
        {
            if (isTargetItem != _isTargetItem) return;
            curCount = Mathf.Min(maxCount, InGameStatus.Item.CountItemByCode(targetCode));
        }
    }
}
