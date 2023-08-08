namespace JsonClasses
{
    public class StringWrapper
    {
        private readonly string text;
        private int position;

        public StringWrapper(string text)
        {
            this.text = text;
            position = 0;
        }

        public int GetPosition()
        {
            return position;
        }

        public string GetText()
        {
            return text;
        }

        public bool IsNullOrEmpty()
        {
            return string.IsNullOrEmpty(text);
        }

        public char CharPosition()
        {
            return text[position];
        }

        public StringWrapper NextPosition()
        {
            int nextPosition = position + 1;
            var stringWrapper = new StringWrapper(text);
            stringWrapper.position = nextPosition;
            return stringWrapper;
        }

        public bool FinalPosition()
        {
            return text.Length == position;
        }
    }
}
