using System;
using System.Collections.Generic;

namespace Assets.Scripts.Components.Conversations.Objects
{
    [Serializable]
    public class ConvChoiceInfo
    {
        public string text;
        public string next;
        public List<ChoiceCondition> conditions;
        public ConvChoiceInfo()
        {
            conditions = new List<ChoiceCondition>();
        }

        public class ChoiceCondition
        {
            public ContentType contentType;
            public string conditionType;
            public string code;
            public int amount;
            public enum ContentType
            {
                Item,
                Quest,
            }
            public new string ToString()
            {
                return $"> {contentType} -> {conditionType}";
            }
        }
        public new string ToString()
        {
            return $"> {text} -> {next}";
        }
    }
}
