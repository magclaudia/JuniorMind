using System;

namespace JsonClasses
{
    public class Range : IPattern
    {
        private readonly char start;
        private readonly char end;

        public Range(char start, char end)
        {
            this.start = start;
            this.end = end;
        }

        public IMatch Match(string text)
        {
            return !string.IsNullOrEmpty(text) && start <= text[0] && text[0] <= end 
                ? new Match(true, text[1..])
                : new Match(false, text);
        }
    }
}