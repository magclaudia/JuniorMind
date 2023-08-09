using System;

namespace JsonClasses
{
    public class Match : IMatch
    {
        private readonly bool succes;
        private StringSpan text;

        public Match(bool succes, StringSpan text)
        {
            this.succes = succes;
            this.text = text;
        }

        public void SetText(StringSpan text)
        {
            this.text = text;
        }

        public bool Succes()
        {
            return succes;
        }

        public StringSpan RemainingText()
        {
            return text;
        }
    }
}
