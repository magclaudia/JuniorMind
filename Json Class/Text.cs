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

        public IMatch Match(StringSpan text)
        {
            if (text.IsNullOrEmpty() || !text.StartsWith(prefix))
            {
                return new Match(false, text);
            }

            return new Match(true, text.Advance(prefix.Length));
        }
    }
}
