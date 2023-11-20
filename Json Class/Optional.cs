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

        public IMatch Match(StringSpan text)
        {
            IMatch match = pattern.Match(text);

            return new Match(true, match.RemainingText());
        }
    }
}
