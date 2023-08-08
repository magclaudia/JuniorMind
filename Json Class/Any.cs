using System;

namespace JsonClasses
{
    public class Any : IPattern
    {
        private readonly string accepted;
        public Any(string accepted)
        {
            this.accepted = accepted;
        }

        public IMatch Match(StringWrapper text)
        {
            if (text.IsNullOrEmpty() || text.FinalPosition() || !accepted.Contains(text.CharPosition()))
            {
                return new Match(false, text);
            }

            return new Match(true, text.NextPosition());
        }
    }
}
