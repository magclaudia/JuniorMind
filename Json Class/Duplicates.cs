using System;

namespace JsonClasses
{
    public class Duplicates : IPattern
    {
        private readonly string operationSign;
        public Duplicates(string operationSign)
        {
            this.operationSign = operationSign;
        }

        public IMatch Match(StringSpan text)
        {
            IMatch match = new Match(true, text);
            if (match.RemainingText().ToString()?.Length >= 2)
            {
                if (operationSign.Contains(text.CharPeek()) && operationSign.Contains(text.CharPeek()))
                {
                    return new Match(false, match.RemainingText());
                }
            }

            return new Match(true, text.Advance());
        }
    }
}
