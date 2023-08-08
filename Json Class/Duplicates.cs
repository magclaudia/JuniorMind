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

        public IMatch Match(StringWrapper text)
        {
            IMatch match = new Match(true, text);
            if (match.RemainingText().GetText().Length >= 2)
            {
                if (operationSign.Contains(text.CharPosition()) && operationSign.Contains(text.CharPosition()))
                {
                    return new Match(false, match.RemainingText());
                }
            }

            return new Match(true, text.NextPosition());
        }
    }
}
