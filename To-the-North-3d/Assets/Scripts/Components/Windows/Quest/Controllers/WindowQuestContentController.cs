using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Windows
{
    public class WindowQuestContentController : MonoBehaviour
    {
        [SerializeField]
        private Transform conditionPrefab, conditionTf;
        [SerializeField]
        private TextMeshProUGUI title, desc;
        private string t, qCode;
        private List<WindowQuestContentConditionController> conditions;

        public string InitQuest(string _questCode)
        {
            qCode = _questCode;
            conditions = new();
            Queue<string> q = DataFunction.LoadTextFromFile($"Quest/{_questCode}");
            string res = title.text = t = q.Dequeue();
            while (t != "?")
            {
                q.TryDequeue(out t);
            }
            while ((t = q.Dequeue()) != "<")
            {
                WindowQuestContentConditionController c = Instantiate(conditionPrefab, conditionTf).GetComponent<WindowQuestContentConditionController>();
                conditions.Add(c);
                string[] token = t.Replace(" ", "").Split(":");
                switch (token[0])
                {
                    case "Have":
                        switch (token[1])
                        {
                            case "Item":
                                c.Init(token[2], int.Parse(token[3]));
                                break;
                        }
                        break;
                }
            }
            string te = string.Empty;
            while ((t = q.Dequeue()) != "!")
            {
                te += $"{t}\n";
            }
            desc.text = te;
            t = res;
            return res;
        }

        public string ClearQuest()
        {
            Destroy(gameObject);
            return t;
        }

        public bool IsClearable
        {
            get
            {
                if (conditions.Count == 0) return true;
                foreach (WindowQuestContentConditionController cond in conditions)
                {
                    if (!cond.IsCleared) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 인벤토리에 아이템이 추가/제거 되었을 때 해당 사실을 퀘스트의 조건 항목들에 전달해주는 함수
        /// </summary>
        /// <param name="_code">아이템 코드</param>
        public void NoticeChange(string _code)
        {
            foreach (WindowQuestContentConditionController cond in conditions)
            {
                cond.NoticeChange(_code);
            }
        }

        /// <summary>
        /// 단순 새로고침 함수
        /// </summary>
        public void Refresh()
        {
            foreach (WindowQuestContentConditionController c in conditions)
            {
                Destroy(c.gameObject);
            }
            InitQuest(qCode);
        }
    }
}
