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
            return !string.IsNullOrEmpty(text) && text[0] == pattern 
                ? new Match(true, text[1..])
                : new Match(false, text);
        }
    }
}
