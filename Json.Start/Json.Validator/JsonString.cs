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
            return CheckUnicodeCharactersValue(input) && CheckEscapeCharacter(input);
        }

        static bool CheckUnicodeCharactersValue(string input)
        {
            const int minValue = 0x20;
            int controlCase = 0;
            foreach (char c in input)
            {
                if (Convert.ToInt32(c) < minValue)
                {
                    controlCase++;
                }
            }

            return controlCase < 1;
        }

        static bool CheckEscapeCharacter(string input)
        {
            for (int i = 0; i <= input.Length - 1; i++)
            {
                if (input[i] == '\\' && !CheckForEscapeChar(input, i + 1))
                {
                    return false;
                }
            }

            return true;
        }

        static bool CheckForEscapeChar(string input, int i)
        {
            const string escapeChars = "\"\\/bfnrt";
            if (input[i] == 'u')
            {
                return IsHexValue(input, i + 1);
            }

            if (input[input.Length - 1 - 1] == '\\')
            {
                return false;
            }

            return escapeChars.Contains(input[i]) || input[i - 1 - 1] == '\\' && escapeChars.Contains(input[i - 1]);
        }

        static bool IsHexValue(string input, int i)
        {
            const int hexUnit = 4;
            for (int j = 0; j < hexUnit; j++)
            {
                if (!IsHexChar(input[i]))
                {
                    return false;
                }

                i++;
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