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

        public IMatch Match(StringWrapper text)
        {
            if (text.IsNullOrEmpty() || text.FinalPosition() || !text.GetText().StartsWith(prefix))
            {
                return new Match(false, text);
            }

            for (int i = 0; i < prefix.Length; i++)
            {
                text.NextPosition();
            }

            return new Match(true, text);
        }
    }
}
