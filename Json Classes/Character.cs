using System;

namespace Classes
{
    public class Character : IPattern
    {
        readonly private char pattern;

        public Character(char pattern)
        {
            this.pattern = pattern;
        }

        public bool Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text[0] == pattern;
        }
    }
}
