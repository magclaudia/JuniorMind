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

        public IMatch Match(StringSpan text)
        {
            if (!text.IsNullOrEmpty() && text.CharPeek() >= start && text.CharPeek() <= end)
            {
                return new Match(true, text.Advance());
            }

            return new Match(false, text);
        }
    }
}