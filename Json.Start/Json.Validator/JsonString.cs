using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return StringHasContent(input) && StringIsDoubleQuoted(input) && VerifyForJsonCharacters(input);
        }

        static bool StringHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static bool StringIsDoubleQuoted(string input)
        {
           return input[0] == '"' && input[^1] == '"';
        }

        static bool VerifyForJsonCharacters(string input)
        {
            return ContainsLargeUnicodeCharacters(input) && !CheckForContainControlCharacters(input) && CheckEscapeCharacter(input);
        }

        static bool ContainsLargeUnicodeCharacters(string input)
        {
            const int minValue = 0x20;
            const int delValueForControl = 0x7F;
            foreach (char c in input)
            {
                if (Convert.ToInt32(c) >= minValue && Convert.ToInt32(c) != delValueForControl)
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckForContainControlCharacters(string input)
        {
            foreach (char c in input)
            {
                if (char.IsControl(c))
                {
                    return true;
                }
            }

            return false;
        }

        static bool CheckEscapeCharacter(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == '\\')
                {
                    return CheckForEscapeChar(input, i + 1);
                }
            }

            return true;
        }

        static bool CheckForEscapeChar(string input, int i)
        {
            const string escapeChars = "\"\\/bfnrt";
            const int hexUnit = 4;
            if (input[i] == 'u' && input.Length - (1 - i) > hexUnit)
            {
                return IsHexValue(input, i + 1, hexUnit);
            }

            return i != input.Length - 1 && escapeChars.Contains(input[i]) || input[i - 1] == '\\';
        }

        static bool IsHexValue(string input, int i, int hexUnit)
        {
            for (int j = i; j <= hexUnit; j++)
            {
                if (!IsHexChar(input[j]))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsHexChar(char c)
        {
            c = char.ToLower(c);
            return char.IsDigit(c) || c >= 'a' && c <= 'f';
        }
    }
}