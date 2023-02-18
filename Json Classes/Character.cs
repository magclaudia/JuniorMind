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

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            if (text[0] == pattern)
            {
                return new Match(true, text);
            }

            return new Match(false, text);
        }
    }
}
