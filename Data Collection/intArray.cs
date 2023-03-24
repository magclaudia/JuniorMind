using System;

namespace DataCollection
{
    class IntArray
    {
        private int[] array;
        private int capacity;
        private int size;
        public IntArray()
        {
            capacity = 4;
            size = 0;
            array = new int[capacity];
        }

        public void Add(int element)
        {
            ResizeIfIsNeeded();
            array[size++] = element;
        }

        public int Count()
        {
            return size;
        }

        public int Element(int index)
        {
            return array[index];
        }

        public void SetElement(int index, int element)
        {
            array[index] = element;
        }

        public bool Contains(int element)
        {
            return array.Contains(element);
        }

        public int IndexOf(int element)
        {
            for (int i = 0; i < array.Length; i++)
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
            SetElement(index, element);
            size++;
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
            Array.Clear(array, 0, size);
            size = 0;
        }

        public void RemoveAt(int index)
        {
            LeftShifting(index);
            size--;
        }

        private void ResizeIfIsNeeded()
        {
            if (size == capacity)
            {
                capacity *= 2;
                Array.Resize(ref array, capacity);
            }
        }

        private void RightShifting(int index)
        {
            for (int i = array.Length - 1; i > index; i--)
            {
                array[i] = array[i - 1];
            }
        }

        private void LeftShifting(int index)
        {
            for (int i = index; i < array.Length - 1; i++)
            {
                array[i] = array[i + 1];
            }
        }
    }
}