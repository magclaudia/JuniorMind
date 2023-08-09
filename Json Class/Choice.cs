using System;

namespace JsonClasses
{
    public class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(StringSpan text)
        {
            foreach (var pattern in patterns)
            {
                var match = pattern.Match(text);
                if (match.Succes())
                {
                    return match;
                }
            }
            return new Match(false, text);
        }

        public void Add(IPattern pattern)
        {
            Array.Resize(ref patterns, patterns.Length + 1);
            patterns[^1] = pattern;
        }
    }
}
