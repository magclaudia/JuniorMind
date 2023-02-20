using System;

namespace JsonClasses
{
    public class Optional : IPattern
    {
        private readonly IPattern pattern;
        public Optional(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new Match(true, text);
            }

            IMatch match = new Match(true, text);
            if (match.Succes())
            {
               match = pattern.Match(match.RemainingText());
            }

            return new Match(true, match.RemainingText());
        }
    }
}
