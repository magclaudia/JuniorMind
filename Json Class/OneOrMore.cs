using JsonClasses;
using System;

namespace JsonClasses
{
    public class OneOrMore : IPattern
    {
        private readonly IPattern pattern;
        public OneOrMore(IPattern pattern)
        {
            this.pattern = new Sequence(pattern, new Many(pattern));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
