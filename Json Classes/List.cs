using System;

namespace JsonClasses
{
    public class List
    {
        private readonly IPattern pattern;
        public List(IPattern element, IPattern separator)
        {
            this.pattern = new Many(new Sequence(element, new Many(new Sequence(separator, element))));
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
