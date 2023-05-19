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
                DontDestroyOnLoad(gameObject);
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
            InGameStatus.Quest.Clear.Add(_questCode);
            return questContentControllers[_questCode].ClearQuest();
        }

        public void NoticeChange(string _code)
        {
            try
            {
                questContentControllers[_code].NoticeChange();
            }
            catch (NullReferenceException)
            {

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
