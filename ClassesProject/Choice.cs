using System;
using static System.Net.Mime.MediaTypeNames;

namespace Classes
{
    public interface IPattern
    {
        bool Match(string text);
    }

    public class Choice : IPattern
    {
        private IPattern[] patterns;

        public Choice(params IPattern[] patterns)
        {
            this.patterns = patterns;
        }

        public bool Match(string text) 
        {
            foreach (var pattern in patterns)
            {
                if (pattern.Match(text))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
