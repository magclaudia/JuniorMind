using System;

namespace Json
{
    public static class JsonNumber
    {
        const string Sign = "-+";

        public static bool IsJsonNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            input = input.ToLower();
            var exponent = input.IndexOfAny("eE".ToCharArray());
            var dot = input.IndexOf('.');
            var indexOfSign = input.IndexOfAny(Sign.ToCharArray());
            return IsInteger(ExtractInteger(input, exponent, dot))
                && IsFraction(ExtractFraction(input, dot, exponent), dot, indexOfSign, exponent)
                && IsExponent(ExtractExponent(input, exponent, dot), exponent, dot);
        }

        static string ExtractInteger(string input, int indexOfExponent, int indexOfDot)
        {
            if (indexOfDot != -1)
            {
                return input[0..indexOfDot];
            }

            if (indexOfDot == -1 && indexOfExponent != -1)
            {
                return input[0..indexOfExponent];
            }

            return input;
        }

        static bool IsInteger(string input)
        {
            if (Sign.Contains(input[0]) && char.IsDigit(input[1]))
            {
                return true;
            }

            return input.Length > 1 && input[0] != '0' || input.Length == 1 && char.IsDigit(input[0]);
        }

        static string ExtractFraction(string input, int indexOfDot, int indexOfExponent)
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
                input = numberOfDots == 1 && containForbiddenLetters == 0 ? input[indexOfDot..input.Length] : input.Remove(0, input.Length);
            }

            return input;
        }

        static bool IsFraction(string input, int indexOfDot, int indexOfSign, int indexOfExponent)
        {
            if (indexOfSign + 1 == indexOfExponent)
            {
                return false;
            }

            return input.Length > 1 && input[^1] != '.' || input.Length == 1 && !input.Contains('.');
        }

        static string ExtractExponent(string input, int indexOfExponent, int indexOfDot)
        {
            if (indexOfExponent > indexOfDot && !Sign.Contains(input[0]))
            {
                input = input[indexOfExponent..input.Length];
            }

            return input;
        }

        static bool IsExponent(string input, int indexOfExponent, int indexOfDot)
        {
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

            if (indexOfExponent < indexOfDot && input.Contains('e') || letters > 0 || numbersOfExponents > 1)
            {
                return false;
            }

            return input[^1] != 'e' && !Sign.Contains(input[^1]);
        }
    }
}