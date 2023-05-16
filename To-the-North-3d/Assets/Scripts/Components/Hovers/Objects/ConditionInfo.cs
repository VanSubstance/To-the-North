using System.Collections.Generic;
using Assets.Scripts.Users;

namespace Assets.Scripts.Components.Hovers
{
    public class ConditionInfo
    {
        public string title;
        public string description;

        public static Dictionary<ConditionType, ConditionInfo> infos = new();

        public static void LoadConditionInfosFromText()
        {
            Queue<string> ss = DataFunction.LoadTextFromFile("Condition");
            ConditionInfo newCondition = new();
            ConditionType curType;
            string s;
            while (ss.TryDequeue(out s))
            {
                // 타입: 1줄
                curType = System.Enum.Parse<ConditionType>(s);
                // 제목: 1줄
                newCondition.title = ss.Dequeue();
                newCondition.description = string.Empty;
                // 효과: n줄
                while (ss.TryDequeue(out s))
                {
                    if (s == string.Empty) break;
                    newCondition.description += $"{s}\n";
                }
                infos[curType] = newCondition;
                newCondition = new();
            }
            return;
        }
    }
}
