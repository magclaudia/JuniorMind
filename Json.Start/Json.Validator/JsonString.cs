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
            return VerifyControlCharacters(input) && CheckEscapeCharacter(input);
        }

        static bool VerifyControlCharacters(string input)
        {
            foreach (char c in input)
            {
                if (Convert.ToInt32(c) < ' ')
                {
                    return false;
                }
            }

            return true;
        }

        static bool CheckEscapeCharacter(string input)
        {
            for (int i = 0; i <= input.Length - 1; i++)
            {
                if (input[i] == '\\' && !CheckForEscapeChar(input, ref i))
                {
                    return false;
                }
            }

            return true;
        }

        static bool CheckForEscapeChar(string input, ref int indexPosition)
        {
            const string escapeChars = "\"\\/bfnrt";
            indexPosition++;
            if (input[indexPosition] == 'u')
            {
                return IsHexValue(input, indexPosition + 1);
            }

            return indexPosition < input.Length - 1 && escapeChars.Contains(input[indexPosition]);
        }

        static bool IsHexValue(string input, int indexPosition)
        {
            const int hexUnit = 4;
            for (int j = 0; j < hexUnit; j++)
            {
                if (!IsHexChar(input[indexPosition]))
                {
                    return false;
                }

                indexPosition++;
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