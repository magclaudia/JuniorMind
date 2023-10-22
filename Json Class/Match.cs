using System;

namespace JsonClasses
{
    public class Match : IMatch
    {
        private readonly bool succes;
        private StringSpan text;
        private static int failPosition = 0;

        public Match(bool succes, StringSpan text)
        {
            this.succes = succes;
            this.text = text;

            if (!succes)
            {
                UpdateFailPosition(text.Position());
            }
        }

        public void UpdateFailPosition(int position)
        {
            if (position > failPosition)
            {
                failPosition = position;
            }
        }

        public static (int line, int column) GetLineAndColumnFromPosition(string text)
        {
            int line = 1, column = 0;
            for (int i = 0; i < failPosition; i++)
            {
                var a = text[i];

                if (text[i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }


            return (line, column);
        }

        public void SetText(StringSpan text)
        {
            this.text = text;
        }

        public bool Succes()
        {
            return succes;
        }

        public StringSpan RemainingText()
        {
            return text;
        }
    }
}
