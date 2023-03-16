using System;

namespace JsonClasses
{
    public class Math : IPattern
    {
        private readonly IPattern pattern;
        public Math()
        {
            var ws = new Many(new Any(" "));
            var mathOperator = new Sequence(ws, new Any("+-*/^%"), ws);
            var value = new Choice(new Number());
            var elements = new List(value, mathOperator);
            var array = new Sequence(new Character('('), ws, elements, ws, new Character(')'));
            value.Add(array);
            pattern = elements;
        }

        public IMatch Match(string text)
        {
            return pattern.Match(text);
        }
    }
}
