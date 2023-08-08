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

        public IMatch Match(StringWrapper text)
        {
            if (!text.IsNullOrEmpty() && !text.FinalPosition() && text.CharPosition() >= start && text.CharPosition() <= end)
            {
                return new Match(true, text.NextPosition());
            }

            return new Match(false, text);
        }
    }
}