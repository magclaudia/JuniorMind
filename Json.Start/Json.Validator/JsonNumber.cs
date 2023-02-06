using System;
using System.Reflection;

namespace Json
{
    public static class JsonNumber
    {
        const string Sign = "-+";

        public static bool IsJsonNumber(string input)
        {
            return NumberHasContent(input) && CheckNumber(input);
        }

        static bool NumberHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static bool CheckNumber(string input)
        {
            int numberOfDots = 0;
            int numbersOfExponents = 0;
            foreach (char c in input)
            {
                if (char.IsDigit(c) && !input.Contains('.') && !IsInteger(input))
                {
                    return false;
                }

                if (c == '.' && !IsFractional(input, c, ref numberOfDots))
                {
                    return false;
                }

                if (!char.IsDigit(c) && c != '.' && !IsExponential(input, c, ref numbersOfExponents))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsInteger(string input)
        {
            return input.Length > 1 && input[0] != '0' || input.Contains(Sign) || input.Length == 1;
        }

        static bool IsFractional(string input, char c, ref int numberOfDots)
        {
            if (c == '.')
            {
                numberOfDots++;
            }

            if (input.Contains('e') && input.IndexOf(c) > input.IndexOf('e') || input.Contains('E') && input.IndexOf(c) > input.IndexOf('E'))
            {
                return false;
            }

            return input.Length > 1 && input.Contains('.') && numberOfDots == 1 && input[^1] != '.';
        }

        static bool IsExponential(string input, char c, ref int numbersOfExponents)
        {
            const string checkExponents = "eE+-";
            c = char.ToLower(c);
            if (c == 'e')
            {
                numbersOfExponents++;
            }

            return c == 'e' && numbersOfExponents == 1 && !checkExponents.Contains(input[^1]) || Sign.Contains(c);
        }
    }
}
