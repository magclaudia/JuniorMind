using System;

namespace JsonClasses
{
    public interface IPattern
    {
        IMatch Match(StringSpan text);
    }
}
