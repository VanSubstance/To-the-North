using System;

namespace Assets.Scripts.Components.Conversations.Objects
{
    [Serializable]
    internal class ConvInfo
    {
        public string desc;
        public ConvChoiceInfo[] choices;

        public ConvInfo()
        {

        }

        public ConvInfo(string desc)
        {
            this.desc = desc;
        }

        public ConvInfo(string desc, ConvChoiceInfo[] _choices)
        {
            this.desc = desc;
            choices = _choices;
        }
    }
}
