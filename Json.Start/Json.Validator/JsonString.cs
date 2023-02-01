using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return StringHasContent(input) && StringIsDoubleQuoted(input) && !CheckForContainControlCharacters(input);
        }

        static bool StringHasContent(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        static bool StringIsDoubleQuoted(string input)
        {
            return input[0] == '"' && input[^1] == '"';
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