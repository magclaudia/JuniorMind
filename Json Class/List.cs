using System;

namespace JsonClasses
{
    public class List : IPattern
    {
        private readonly IPattern element;
        private readonly IPattern separator;
        public List(IPattern element, IPattern separator)
        {
            this.element = element;
            this.separator = separator;
        }

        public IMatch Match(StringSpan text)
        {
            IMatch currentMatch = element.Match(text);
            if (!currentMatch.Succes())
            {
                return new Match(false, currentMatch.RemainingText());
            }

            IMatch lastMatch = currentMatch;
            text = currentMatch.RemainingText();

            while (true)
            {
                currentMatch = separator.Match(text);
                if (!currentMatch.Succes())
                {
                    break;
                }

                text = currentMatch.RemainingText();
                currentMatch = element.Match(text);
                if (!currentMatch.Succes())
                {
                    return new Match(false, currentMatch.RemainingText());
                }

                lastMatch = currentMatch;
                text = currentMatch.RemainingText();
            }

            return new Match(true, lastMatch.RemainingText());
        }
    }
}
