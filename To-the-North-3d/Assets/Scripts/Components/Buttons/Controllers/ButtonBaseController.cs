using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Components.Buttons.Controllers
{
    internal class ButtonBaseController : MonoBehaviour
    {
        private Button btn;

        private void Awake()
        {
            btn = GetComponent<Button>();
        }

        /// <summary>
        /// 버튼에 신규 클릭 액션을 설정
        /// </summary>
        /// <param name="action"></param>
        public void SetButtonAction(UnityAction action)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
        }

        /// <summary>
        /// 버튼 오브젝트 엑티브 설정
        /// </summary>
        /// <param name="isAwaking">true: 깨우기 | false: 재우기</param>
        public void SetActice(bool isAwaking)
        {
            gameObject.SetActive(isAwaking);
        }

        public virtual void Clear()
        {
            btn.onClick.RemoveAllListeners();
        } 
    }
}
