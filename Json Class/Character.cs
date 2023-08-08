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

        public IMatch Match(StringWrapper text)
        {
            if (!text.IsNullOrEmpty() && !text.FinalPosition() && text.CharPosition() == pattern)
            {
                return new Match(true, text.NextPosition());
            }

            return new Match(false, text);
        }
    }
}

