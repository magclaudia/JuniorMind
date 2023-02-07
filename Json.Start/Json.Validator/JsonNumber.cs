using System;
using System.Reflection;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return NumberHasContent(input) && IsInteger(ExtractInteger(input));
        }

        static bool NumberHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static string ExtractInteger(string input)
        {
            var indexOfExponent = input.IndexOfAny("eE".ToCharArray());
            var indexOfDot = input.IndexOf('.');
            if (indexOfDot < indexOfExponent)
            {
               input = input.Remove(indexOfDot, input.Length - indexOfDot);
            }

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    var indexOfLetter = input.IndexOf(c);
                    input = input.Remove(indexOfLetter, input.Length - indexOfLetter);
                }
            }

            return input;
        }

        static bool IsInteger(string input)
        {
            return input.Length > 1 && input[0] != '0' || input.Length == 1;
        }
    }
}
