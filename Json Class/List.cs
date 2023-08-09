using System;

namespace JsonClasses
{
    public class List : IPattern
    {
        private readonly IPattern pattern;
        public List(IPattern element, IPattern separator)
        {
            pattern = new Optional(new Sequence(element, new Many(new Sequence(separator, element))));
        }

        public IMatch Match(StringSpan text)
        {
            return pattern.Match(text);
        }
    }
}
