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
            var arrayValue = new Choice(new String(), new String(), new Number(),
                new Text("true"), new Text("false"), new Text("null"));
            var arrayElement = new Sequence(ws, arrayValue, ws);
            var arrayElements = new List(arrayElement, new Character(','));
            var array = new Sequence(new Character('['), ws, arrayElements, ws, new Character(']'));
            var element = new Sequence(ws, value, ws);
            var member = new Sequence(ws, new String(), ws, new Character(':'), ws, element);
            var members = new List(member, new Character(','));
            var obj = new Sequence(new Character('{'), ws, members, ws, new Character('}'));
            var emptyObj = new Sequence(new Character('{'), ws, new Character('}'));
            var emptyArray = new Sequence(new Character('['), ws, new Character(']'));
            value.Add(array);
            value.Add(obj);
            value.Add(emptyObj);
            value.Add(emptyArray);
            pattern = element;
        }

        public IMatch Match(StringSpan text)
        {
            return pattern.Match(text);
        }
    }
}