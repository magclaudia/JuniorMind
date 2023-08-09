using System;

namespace JsonClasses
{
    public interface IMatch
    {
        bool Succes();
        StringSpan RemainingText();
    }
}
