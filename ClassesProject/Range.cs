using System;

namespace Classes
{
    class Range
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
            var c = new Range('a', 'f');
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return c.start <= text[0] && text[0] <= c.end ? true : false;
        }
    }
}