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
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            if (text.StartsWith('-') && text.Length > 1)
            {
                text = text[1..];
            }

            if (start <= text[0] && text[0] <= end)
            {
                return new Match(true, text);
            }

            return new Match(false, text);
        }
    }
}