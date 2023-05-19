using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components.Infos
{
    public class UIInfoTextContainerController : MonoBehaviour
    {
        [SerializeField]
        private Transform TextContentPrefab;
        public Transform visualTf, poolingTf;
        [SerializeField]
        private int maxPooling = 6;

        private UIInfoTextContentController[] infoTextContents;

        private Queue<string> queueForTextWait;

        private static UIInfoTextContainerController _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static UIInfoTextContainerController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(UIInfoTextContainerController)) as UIInfoTextContainerController;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            queueForTextWait = new Queue<string>();
            infoTextContents = new UIInfoTextContentController[maxPooling];
            for (int i = 0; i < maxPooling; i++)
            {
                infoTextContents[i] = Instantiate(TextContentPrefab, poolingTf).GetComponent<UIInfoTextContentController>();
                infoTextContents[i].IsInActive = false;
            }
        }

        public void PrintText(string _text)
        {
            foreach (UIInfoTextContentController _infoText in infoTextContents)
            {
                if (!_infoText.IsInActive)
                {
                    _infoText.ActivateText(_text);
                    return;
                }
            }
            queueForTextWait.Enqueue(_text);
            return;
        }

        /// <summary>
        /// 풀링에 여유가 생겼을 때 호출되는 함수
        /// </summary>
        public void NoticeVacancy()
        {
            string t;
            if (queueForTextWait.TryDequeue(out t))
            {
                PrintText(t);
            }
        }
    }
}
