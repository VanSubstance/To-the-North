using System;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Windows.Inventory
{
    public class ContainerBaseController : MonoBehaviour
    {
        private TextMeshProUGUI titleUGUI;
        private ContentBaseController content;
        private ContentType contentType;
        public ContentType ContentType
        {
            get
            {
                return contentType;
            }
        }

        protected void Awake()
        {
            InitContent();
        }

        private void InitContent()
        {
            if (content != null) return;
            titleUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            content = transform.GetChild(1).GetChild(0).GetComponent<ContentBaseController>();
            contentType = ContentType.Undefined;
        }

        /// <summary>
        /// 컨텐츠 반환 함수
        /// 필요 시 제목도 동시에 변환 가능
        /// </summary>
        /// <typeparam name="TContent">컨텐츠의 반환 타입</typeparam>
        /// <param name="_newTitle">새로운 제목</param>
        /// <returns></returns>
        public TContent GetContent<TContent>(ContentType _contentType = ContentType.Undefined)
        {
            InitContent();
            switch (contentType = _contentType)
            {
                case ContentType.Inventory:
                    SetTitle("인벤토리");
                    break;
                case ContentType.Looting:
                    SetTitle("루팅");
                    break;
                case ContentType.Equipment:
                    SetTitle("장비");
                    break;
                case ContentType.None_L:
                case ContentType.None_C:
                case ContentType.None_R:
                case ContentType.Undefined:
                    break;
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
