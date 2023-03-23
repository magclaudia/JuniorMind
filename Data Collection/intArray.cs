using System;

namespace DataCollection
{
    class IntArray
    {
        private int[] array;
        public IntArray()
        {
            array = new int[0];
        }

        public void Add(int element)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = element;
        }

        public int Count()
        {
            return array.Length;
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
            Array.Resize(ref array, array.Length + 1);
            ShiftingElements(index);
            SetElement(index, element);
        }

        public void Remove(int element)
        {
            ShiftingElements(IndexOf(element));
            Array.Resize(ref array, array.Length - 1);
        }

        public void Clear()
        {
            Array.Clear(array, 0, array.Length);
        }

        public void RemoveAt(int index)
        {
            ShiftingElements(index);
            Array.Resize(ref array, array.Length - 1);
        }

        private void ShiftingElements(int index)
        {
            if (array[^1] == 0)
            {
                for (int i = array.Length - 1; i > index; i--)
                {
                    array[i] = array[i - 1];
                }
            }
            else
            {
                for (int i = index; i < array.Length - 1; i++)
                {
                    array[i] = array[i + 1];
                }
            }
        }
    }
}
