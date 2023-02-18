using System;

namespace Classes
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

        public bool Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text.StartsWith('-') && text.Length > 1)
            {
                text = text.Substring(1);
            }

            return start <= text[0] && text[0] <= end ? true : false;
        }
    }
}