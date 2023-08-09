using System;

namespace JsonClasses
{
    public class Character : IPattern
    {
        readonly private char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(StringSpan text)
        {
            if (!text.IsNullOrEmpty() && text.CharPeek() == pattern)
            {
                return new Match(true, text.Advance());
            }

            return new Match(false, text);
        }
    }
}

