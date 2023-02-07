using System;
using System.Reflection;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return NumberHasContent(input) && IsInteger(ExtractInteger(input)) && IsFraction(ExtractFraction(input));
        }

        static bool NumberHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static string ExtractInteger(string input)
        {
            var indexOfExponent = input.IndexOfAny("eE".ToCharArray());
            var indexOfDot = input.IndexOf('.');
            if (indexOfDot != indexOfExponent)
            {
               input = input.Substring(0, indexOfDot);
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

        static string ExtractFraction(string input)
        {
            var indexOfExponent = input.IndexOfAny("eE".ToCharArray());
            var indexOfDot = input.IndexOf('.');
            int numberOfDots = 0;
            int containForbiddenLetters = 0;
            foreach (char c in input)
            {
                if (c == '.')
                {
                    numberOfDots++;
                }

                if (char.IsLetter(c) && c != 'e' && c != 'E')
                {
                    containForbiddenLetters++;
                }
            }

            if (input.Contains('.'))
            {
                input = indexOfDot != indexOfExponent && numberOfDots == 1 && containForbiddenLetters == 0 ? input.Substring(indexOfDot, input.Length - indexOfDot) : input.Remove(0, input.Length);
            }

            return input;
        }

        static bool IsFraction(string input)
        {
            return input.Length > 1 && input[^1] != '.' || input.Length == 1 && !input.Contains('.');
        }
    }
}
