using System;

namespace JsonClasses
{
    public class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public IMatch Match(string text) 
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(text).Succes())
                {
                    return new Match(true, text[1..]);
                }
            }

            return new Match(false, text);
        }
    }
}
