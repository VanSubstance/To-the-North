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
        private string t;
        private List<WindowQuestContentConditionController> conditions;

        public string InitQuest(string _questCode)
        {
            string res = string.Empty;
            conditions = new();
            Queue<string> q = DataFunction.LoadTextFromFile($"Quest/{_questCode}");
            res = title.text = t = q.Dequeue();
            while (t != "?")
            {
                q.TryDequeue(out t);
            }
            while (t != "<")
            {
                WindowQuestContentConditionController c = Instantiate(conditionPrefab, conditionTf).GetComponent<WindowQuestContentConditionController>();
                conditions.Add(c);
                string[] token = (t = q.Dequeue()).Replace(" ", "").Split(":");
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

        public void NoticeChange()
        {
            foreach (WindowQuestContentConditionController cond in conditions)
            {
                cond.NoticeChange();
            }
        }
    }
}
