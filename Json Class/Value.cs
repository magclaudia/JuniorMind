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
            var member = new Sequence(ws, new String(), ws, new Character(':'), ws, element);
            var members = new List(member, new Character(','));
            var array = new Sequence(new Character('['), ws, members, ws, new Character(']'));
            var obj = new Sequence(new Character('{'), ws, members, ws, new Character('}'));
            var arrayObjValue = new Sequence(ws, obj, ws);
            var arrayObjList = new List(new Choice(arrayObjValue, element), new Character(','));
            var arrayObj = new Sequence(new Character('['), ws, arrayObjList, ws, new Character(']'));
            value.Add(array);
            value.Add(obj);
            value.Add(arrayObj);
            pattern = element;
        }

        public IMatch Match(StringSpan text)
        {
            return pattern.Match(text);
        }
    }
}