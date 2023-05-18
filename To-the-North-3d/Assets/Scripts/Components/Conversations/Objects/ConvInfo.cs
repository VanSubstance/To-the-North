using System;
using System.Collections.Generic;

namespace Assets.Scripts.Components.Conversations.Objects
{
    [Serializable]
    public class ConvInfo
    {
        public string desc;
        public List<ConvChoiceInfo> choices;

        public ConvInfo()
        {
            choices = new List<ConvChoiceInfo>();
        }

        public ConvInfo(string desc)
        {
            this.desc = desc;
        }
    }
}
