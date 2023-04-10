using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class List<T> : IList<T>
    {
        private T[] list;
        public List()
        {
            list = new T[4];
        }

        public int Count { get; protected set; } = 0;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }

        public void CopyTo(T[] array, int index)
        {
            for (int i = 0; i < Count; i++)
            {
                if (array != null && index > 0 && Count < array.Length - index)
                {
                    array[index + i] = this[i];
                }
            }
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

        public virtual void Add(T item)
        {
            Capacity();
            list[Count++] = item;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) > -1;
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (list[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual void Insert(int index, T item)
        {
            Capacity();
            RightShifting(index);
            list[index] = item;
            Count++;
        }

        public bool Remove(T item)
        {
            bool removedItem = false;
            for (int i = 0; i < Count; i++)
            {
                if (list[i].Equals(item))
                {
                    RemoveAt(i);
                    removedItem = true;
                }
            }

            return removedItem;
        }

        public void Clear()
        {
            Array.Clear(list);
            Count = 0;
        }

        public void RemoveAt(int index)
        {
            LeftShifting(index);
            Count--;
        }

        private void Capacity()
        {
            if (Count == list.Length)
            {
                Array.Resize(ref list, list.Length * 2);
            }
        }

        private void RightShifting(int index)
        {
            for (int i = Count - 1; i >= index; i--)
            {
                list[i + 1] = list[i];
            }
        }

        private void LeftShifting(int index)
        {
            for (int i = index; i <= Count - 1; i++)
            {
                list[i] = list[i + 1];
            }
        }
    }
}
