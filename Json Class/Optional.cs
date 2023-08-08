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

        public IMatch Match(StringWrapper text)
        {
            IMatch match = new Match(true, text);
            if (match.Succes())
            {
                match = pattern.Match(match.RemainingText());
            }

            return new Match(true, match.RemainingText());
        }
    }
}
