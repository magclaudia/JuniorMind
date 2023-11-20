using System;

namespace JsonClasses
{
    public class Sequence : IPattern
    {
        private IPattern[] patterns;

        public Sequence(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(StringSpan text)
        {
            IMatch match = new Match(true, text);
            var maxPosition = match.RemainingText().Position();
            foreach (var pattern in patterns)
            {
                match = pattern.Match(match.RemainingText());

                if (match.RemainingText().Position() > maxPosition)
                {
                    maxPosition = match.RemainingText().Position();
                }

                if (!match.Succes())
                {
                    return new Match(false, new StringSpan(text.GetText(), maxPosition));
                }
            }

            return new Match(true, new StringSpan(text.GetText(), maxPosition));
        }
    }
}
