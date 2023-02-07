using System;

namespace Json
{
    public static class JsonNumber
    {
        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            input = input.ToLower();
            var exponent = input.IndexOfAny("eE".ToCharArray());
            var dot = input.IndexOf('.');
            return IsInteger(ExtractInteger(input, exponent, dot)) && IsFraction(ExtractFraction(input, dot)) && IsExponent(ExtractExponent(input, exponent, dot), exponent, dot);
        }

        static string ExtractInteger(string input, int indexOfExponent, int indexOfDot)
        {
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

        static string ExtractFraction(string input, int indexOfDot)
        {
            int numberOfDots = 0;
            int containForbiddenLetters = 0;
            foreach (char c in input)
            {
                if (c == '.')
                {
                    numberOfDots++;
                }

                if (char.IsLetter(c) && c != 'e' && indexOfDot != -1)
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

        static string ExtractExponent(string input, int indexOfExponent, int indexOfDot)
        {
            if (indexOfExponent > indexOfDot)
            {
                input = input.Substring(indexOfExponent, input.Length - indexOfExponent);
            }

            if (indexOfDot > indexOfExponent && indexOfExponent != -1)
            {
                input = input.Substring(indexOfExponent, indexOfDot);
            }

            return input;
        }

        static bool IsExponent(string input, int indexOfExponent, int indexOfDot)
        {
            const string sign = "-+";
            int numbersOfExponents = 0;
            int letters = 0;
            foreach (char c in input)
            {
                    if (c == 'e')
                    {
                        numbersOfExponents++;
                    }

                    if (char.IsLetter(c) && c != 'e')
                    {
                        letters++;
                    }
            }

            if (indexOfExponent < indexOfDot && input.Contains('e'))
            {
                return false;
            }

            return input[^1] != 'e' && !sign.Contains(input[^1]) && numbersOfExponents <= 1 && letters == 0;
        }
    }
}