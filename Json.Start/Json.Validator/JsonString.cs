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
            return ContainsLargeUnicodeCharacters(input) && !CheckForContainControlCharacters(input);
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
    }
}