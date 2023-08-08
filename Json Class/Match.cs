using System;

namespace JsonClasses
{
    public class Match : IMatch
    {
        private readonly bool succes;
        private StringWrapper text;

        public Match(bool succes, StringWrapper text)
        {
            this.succes = succes;
            this.text = text;
        }

        public void SetText(StringWrapper text)
        {
            this.text = text;
        }

        public bool Succes()
        {
            return succes;
        }

        public StringWrapper RemainingText()
        {
            return text;
        }
    }
}
