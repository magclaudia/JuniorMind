using System;

namespace JsonClasses
{
    public class Any : IPattern
    {
        private readonly string accepted;
        public Any(string accepted)
        {
            this.accepted = accepted;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(false, text);
            }

            foreach(var item in accepted)
            {
                if (text.Contains(item))
                {
                    return new Match(true, text[1..]);
                }
            }

            return new Match(false, text);
        }
    }
}
