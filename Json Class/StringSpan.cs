namespace JsonClasses
{
    public class StringSpan
    {
        private readonly string? text;
        private readonly int position;

        public StringSpan(string? text)
        {
            this.text = text;
        }

        public StringSpan(string? text, int position)
        {
            this.text = text;
            this.position = position;
        }

        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(text) || text.Length == position;
        }

        public char CharPeek()
        {
            return text![position];
        } 

        public StringSpan Advance(int nextPosition = 1)
        {
            int newPosition = position + nextPosition;
            return new StringSpan(text, newPosition);
        }

        public bool StartsWith(string prefix)
        {
            return text!.StartsWith(prefix);
        }

        public bool CheckIfEqualTo(StringSpan expectedResult)
        {
            return text == expectedResult.text && position == expectedResult.position;
        }

        public (int line, int column) GetLineAndColumnOfPosition()
        {
            int line = 1;
            int column = 1;

            for (int i = 0; i < position; i++)
            {
                if (text![i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else if (text[i] == '\r')
                {
                    column = 1;
                }
                else
                {
                    column++;
                }
            }

            return (line, column);
        }
    }
}