using System;

namespace DataCollection
{
    class IntArray
    {
        private int[] array;

        public IntArray()
        {
            array = new int[4];
        }

        public int Count { get; private set; } = 0;
        public int this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public void Add(int element)
        {
            ResizeIfIsNeeded();
            array[Count++] = element;
        }

        public bool Contains(int element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(int element)
        {
            for (int i = 0; i <= Count; i++)
            {
                if (array[i] == element)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, int element)
        {
            ResizeIfIsNeeded();
            RightShifting(index);
            array[index] = element;
            Count++;
        }

        public void Remove(int element)
        {
            var index = IndexOf(element);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        public void Clear()
        {
            Array.Clear(array, 0, Count);
            Count = 0;
        }

        public void RemoveAt(int index)
        {
            LeftShifting(index);
            Count--;
        }

        private void ResizeIfIsNeeded()
        {
            if (Count == array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }
        }

        private void RightShifting(int index)
        {
            for (int i = Count; i >= index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void LeftShifting(int index)
        {
            for (int i = index; i <= Count - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
    }
}