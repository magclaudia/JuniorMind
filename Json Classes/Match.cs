using System;

namespace JsonClasses
{
    public class Match : IMatch
    {
        private readonly bool succes;
        private readonly string text;

        public Match(bool succes, string text)
        {
            this.succes = succes;
            this.text = text;
        }

        public bool Success()
        {
            return succes;
        }

        public string RemainingText()
        {
            return text;
        }
    }
}
