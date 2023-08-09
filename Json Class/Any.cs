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

        public IMatch Match(StringSpan text)
        {
            if (text.IsNullOrEmpty() || !accepted.Contains(text.CharPeek()))
            {
                return new Match(false, text);
            }

            return new Match(true, text.Advance());
        }
    }
}
