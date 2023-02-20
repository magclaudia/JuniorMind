using System;

namespace JsonClasses
{
    public class Text : IPattern
    {
        private readonly string prefix;

        public Text(string prefix)
        {
            this.prefix = prefix;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            if (text.StartsWith(prefix))
            {
                return new Match(true, text[prefix.Length..]);
            }

            return new Match(false, text);
        }
    }
}
