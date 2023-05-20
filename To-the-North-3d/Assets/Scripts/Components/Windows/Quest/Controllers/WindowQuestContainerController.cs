using System;
using System.Collections.Generic;
using Assets.Scripts.Commons;
using UnityEngine;

namespace Assets.Scripts.Components.Windows
{
    public class WindowQuestContainerController : WindowFlexBaseController
    {
        [SerializeField]
        private Transform questContentPrefab;

        public Dictionary<string, WindowQuestContentController> questContentControllers;

        private static WindowQuestContainerController _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static WindowQuestContainerController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(WindowQuestContainerController)) as WindowQuestContainerController;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private new void Awake()
        {
            base.Awake();
            if (_instance == null)
            {
                _instance = this;
                MouseInteractionController.AttachMouseInteractor<MouseInteractionMoveableController>(transform);
                questContentControllers = new();
                // 퀘스트 띄우기
                foreach (string qCode in InGameStatus.Quest.Progress)
                {
                    ActivateQuest(qCode, true);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public string ActivateQuest(string _questCode, bool isLoad = false)
        {
            if (!isLoad)
                InGameStatus.Quest.Progress.Add(_questCode);
            questContentControllers[_questCode] = Instantiate(questContentPrefab, transform).GetComponent<WindowQuestContentController>();
            return questContentControllers[_questCode].InitQuest(_questCode);
        }

        public string ClearQuest(string _questCode)
        {
            InGameStatus.Quest.Progress.Remove(_questCode);
            InGameStatus.Quest.Done.Add(_questCode);
            return questContentControllers[_questCode].ClearQuest();
        }

        /// <summary>
        /// 인벤토리에 아이템이 추가/제거 되었을 때 해당 사실을 퀘스트의 조건 항목들에 전달해주는 함수
        /// </summary>
        /// <param name="_code">아이템 코드</param>
        public void NoticeItemChanged(string _code)
        {
            try
            {
                foreach (KeyValuePair<string, WindowQuestContentController> kv in questContentControllers)
                {
                    kv.Value.NoticeChange(_code);
                }
            }
            catch (NullReferenceException)
            {

            }
        }

        /// <summary>
        /// 단순 새로고침 함수
        /// </summary>
        public void Refresh()
        {
            foreach (KeyValuePair<string, WindowQuestContentController> kv in questContentControllers)
            {
                kv.Value.Refresh();
            }
        }

        public override void OnClose()
        {
        }

        public override void OnOpen()
        {
        }
    }
}
