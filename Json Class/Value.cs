using System;

namespace JsonClasses
{
    public class Value : IPattern
    {
        private readonly IPattern pattern;
        public Value()
        {
            var value = new Choice(new String(), new Number(),
                new Text("true"), new Text("false"), new Text("null"));
            var ws = new Many(new Any(" \n\r\t"));
            var element = new Sequence(ws, value, ws);
            var elements = new List(element, new Character(','));
            var array = new Sequence(new Character('['), ws, elements, ws, new Character(']'));
            var member = new Sequence(ws, value, ws, new Character(':'), ws, new Choice(element, array));
            var members = new List(member, new Character(','));
            var obj = new Sequence(new Character('{'), ws, members, ws, new Character('}'));
            pattern = new Sequence(obj, ws);
        }

        public IMatch Match(StringSpan text)
        {
            return pattern.Match(text);
        }
    }
}
