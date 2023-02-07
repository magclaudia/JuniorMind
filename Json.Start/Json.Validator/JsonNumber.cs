using System;
using System.Reflection;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            return NumberHasContent(input) && IsInteger(ExtractInteger(input)) && IsFraction(ExtractFraction(input)) && IsExponent(ExtractExponent(input));
        }

        static bool NumberHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static string ExtractInteger(string input)
        {
            var indexOfExponent = input.IndexOfAny("eE".ToCharArray());
            var indexOfDot = input.IndexOf('.');
            if (indexOfDot != -1)
            {
               input = input.Substring(0, indexOfDot);
            }
            else if (indexOfDot == -1 && indexOfExponent != -1)
            {
                input = input.Substring(0, indexOfExponent);
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
            var indexOfDot = input.IndexOf('.');
            int numberOfDots = 0;
            int containForbiddenLetters = 0;
            foreach (char c in input)
            {
                if (c == '.')
                {
                    numberOfDots++;
                }

                if (char.IsLetter(c) && c != 'e' && c != 'E' && indexOfDot != -1)
                {
                    containForbiddenLetters++;
                }
            }

            if (indexOfDot != -1)
            {
                input = numberOfDots == 1 && containForbiddenLetters == 0 ? input.Substring(indexOfDot, input.Length - indexOfDot) : input.Remove(0, input.Length);
            }

            return input;
        }

        static bool IsFraction(string input)
        {
            return input.Length > 1 && input[^1] != '.' || input.Length == 1 && !input.Contains('.');
        }

        static string ExtractExponent(string input)
        {
            var indexOfExponent = input.IndexOfAny("eE".ToCharArray());
            var indexOfDot = input.IndexOf('.');
            int numbersOfExponents = 0;
            int letters = 0;
            foreach (char c in input)
            {
                if (c == 'e' || c == 'E')
                {
                    numbersOfExponents++;
                }

                if (char.IsLetter(c) && c != 'e' && c != 'E')
                {
                    letters++;
                }
            }

            if (indexOfExponent > indexOfDot && numbersOfExponents == 1 && letters == 0)
            {
                return input.Substring(indexOfExponent, input.Length - indexOfExponent);
            }

            return input.Remove(0, input.Length - indexOfDot);
        }

        static bool IsExponent(string input)
        {
            const string sign = "-+";
            return input[^1] != 'e' && input[^1] != 'E' && !sign.Contains(input[^1]) && !input.Contains('.');
        }
    }
}
