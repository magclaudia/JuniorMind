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

        public IMatch Match(string text)
        {
            IMatch match = new Match(true, text);
            if (match.RemainingText().Length >= 2)
            {
                if (operationSign.Contains(text[0]) && operationSign.Contains(text[2]))
                {
                    return new Match(false, match.RemainingText());
                }
            }

            return new Match(true, text[1..]);
        }
    }
}
