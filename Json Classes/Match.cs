using System;

namespace JsonClasses
{
    public class Match : IMatch
    {
        private readonly bool succes;
        private string text;

        public Match(bool succes, string text)
        {
            this.succes = succes;
            this.text = text;
        }

        public void SetText(string text)
        {
            this.text = text; 
        }

        public bool Succes()
        {
            return succes;
        }

        public string RemainingText()
        {
            return text;
        }
    }
}
