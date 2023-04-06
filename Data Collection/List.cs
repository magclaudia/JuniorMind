using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class List<T> : IEnumerable<T>
    {
        private T[] array;
        public List()
        {
            array = new T[4];
        }

        public int Count { get; protected set; } = 0;

        public T this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Add(T element)
        {
            Capacity();
            array[Count++] = element;
        }

        public bool Contains(T element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(T element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (array[i].Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Insert(int index, T element)
        {
            Capacity();
            RightShifting(index);
            array[index] = element;
            Count++;
        }

        public void Remove(object element)
        {
            for (int i = 0; i < Count; i++)
            {
                if (array[i].Equals(element))
                {
                    RemoveAt(i);
                }
            }
        }

        public void Clear()
        {
            Array.Clear(array);
            Count = 0;
        }

        public void RemoveAt(int index)
        {
            LeftShifting(index);
            Count--;
        }

        private void Capacity()
        {
            if (Count == array.Length)
            {
                Array.Resize(ref array, array.Length * 2);
            }
        }

        private void RightShifting(int index)
        {
            for (int i = Count - 1; i >= index; i--)
            {
                array[i + 1] = array[i];
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
