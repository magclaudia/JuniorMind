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
            return IsInteger(ExtractInteger(input, exponent, dot))
                && IsFraction(ExtractFraction(input, dot, exponent))
                && IsExponent(ExtractExponent(input, exponent));
        }

        static string ExtractInteger(string input, int indexOfExponent, int indexOfDot)
        {
            if (indexOfDot != -1)
            {
                return input[..indexOfDot];
            }

            if (indexOfExponent != -1)
            {
                return input[..indexOfExponent];
            }

            return input;
        }

        static bool IsInteger(string input)
        {
            if (input[0] == '-')
            {
                input = input[1..];
            }

            if (input.Length > 1 && input[0] == '0')
            {
                return false;
            }

            return IsDigits(input);
        }

        static string ExtractFraction(string input, int indexOfDot, int indexOfExponent)
        {
            if (indexOfDot != -1 && indexOfExponent == -1)
            {
                return input[indexOfDot..];
            }

            if (indexOfDot != -1 && indexOfExponent != -1)
            {
                return input[indexOfDot..indexOfExponent];
            }

            return string.Empty;
        }

        static bool IsFraction(string input)
        {
            return input == string.Empty || IsDigits(input[1..]);
        }

        static string ExtractExponent(string input, int indexOfExponent)
        {
            if (indexOfExponent != -1)
            {
                return input[indexOfExponent..];
            }

            return string.Empty;
        }

        static bool IsExponent(string input)
        {
            if (input.Length > 1 && input[1] == '-' || input.Length > 1 && input[1] == '+')
            {
                input = input[1..];
            }

            return input == string.Empty || IsDigits(input[1..]);
        }

        static bool IsDigits(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return input.Length > 0;
        }
    }
}