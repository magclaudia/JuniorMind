using System;

namespace JsonClasses
{
    public class Many : IPattern
    {
        private readonly IPattern pattern;
        public Many(IPattern pattern)
        {
            this.pattern = pattern;
        }

        public IMatch Match(string text)
        {
            IMatch match = new Match(true, text);
            while (match.Succes())
            {
                match = pattern.Match(match.RemainingText());
            }

            return new Match(true, match.RemainingText());
        }
    }
}
