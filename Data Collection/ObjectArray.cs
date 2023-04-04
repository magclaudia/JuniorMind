using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class ObjectArray : IEnumerable
    {
        private object[] array;

        public ObjectArray()
        {
            array = new object[4];
        }

        public int Count { get; protected set; } = 0;

        public object this[int index]
        {
            get => array[index];
            set => array[index] = value;
        }

        public IEnumerator GetEnumerator()
        {
            return new ObjIEnumerator(this);
        }

        public void Add(object element)
        {
            ResizeIfIsNeeded();
            array[Count++] = element;
        }

        public bool Contains(object element)
        {
            return IndexOf(element) > -1;
        }

        public int IndexOf(object element)
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

        public void Insert(int index, object element)
        {
            ResizeIfIsNeeded();
            RightShifting(index);
            array[index] = element;
            Count++;
        }

        public void Remove(object element)
        {
            var index = IndexOf(element);
            if (index >= 0)
            {
                RemoveAt(index);
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

        private void ResizeIfIsNeeded()
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
