using System;

namespace Json
{
    public static class JsonString
    {
        public static bool IsJsonString(string input)
        {
            return StringIsDoubleQuoted(input);
        }

        static bool StringIsDoubleQuoted(string input)
        {
            return input[0] == '"' && input[^1] == '"';
        }
    }
}