using System;

namespace Assets.Scripts.Components.Conversations.Objects
{
    [Serializable]
    internal class ConvChoiceInfo
    {
        public string text;
        public int next;
        public ConvChoiceInfo()
        {
        }
        public ConvChoiceInfo(string text)
        {
            this.text = text;
            this.next = -1;
        }
        public ConvChoiceInfo(string text, int _next)
        {
            this.text = text;
            this.next = _next;
        }
    }
}
