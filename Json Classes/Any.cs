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

            int j = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == accepted[0] || text[i] == accepted[1])
                {
                    j++;
                    text = text[j..];
                    return new Match(true, text);
                }
            }

            return new Match(false, text);
        }
    }
}
