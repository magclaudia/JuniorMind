using System;

namespace JsonClasses
{
    public interface IMatch
    {
        bool Success();
        string RemainingText();
    }
}
