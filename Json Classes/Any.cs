using System;

namespace JsonClasses
{
    public class Any : IPattern
    {
        private readonly string accepted;
        public Any(string accepted)
        {
            this.accepted = accepted;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && accepted.Contains(text[0]) ? new Match(true, text[1..]) : new Match(false, text);
        }
    }
}
