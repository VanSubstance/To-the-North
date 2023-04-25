using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContainerBaseController : MonoBehaviour
    {
        private TextMeshProUGUI titleUGUI;
        private ContentBaseController content;

        protected void Awake()
        {
            InitContent();
        }

        private void InitContent()
        {
            if (content != null) return;
            titleUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            content = transform.GetChild(1).GetChild(0).GetComponent<ContentBaseController>();
        }

        /// <summary>
        /// 컨텐츠 반환 함수
        /// 필요 시 제목도 동시에 변환 가능
        /// </summary>
        /// <typeparam name="TContent">컨텐츠의 반환 타입</typeparam>
        /// <param name="_newTitle">새로운 제목</param>
        /// <returns></returns>
        public TContent GetContent<TContent>(string _newTitle = null)
        {
            InitContent();
            if (!string.IsNullOrEmpty(_newTitle))
            {
                SetTitle(_newTitle);
            }
            try
            {
                return (TContent)(object)content;
            }
            catch (NullReferenceException)
            {
                // 컨텐츠 없음 = 빈칸
            }
            return (TContent)(object)null;
        }

        public void SetTitle(string _title)
        {
            titleUGUI.text = _title;
        }
    }
}
