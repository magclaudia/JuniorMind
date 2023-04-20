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
            get
            {
                if (index < 0 || index > Count - 1 && Count > 0)
                {
                    throw new ArgumentOutOfRangeException("Index is negative or bigger than list length.");
                }

                return list[index];
            }
            set
            {
                if (index < 0 || index > Count - 1)
                {
                    throw new ArgumentOutOfRangeException("Index is negative or bigger than list length.");
                }

                list[index] = value;
            }
        }

        public void CopyTo(T[] array, int index)
        {
            for (int i = 0; i < Count; i++)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("Array can`t be null.");
                }

                if (index < 0 || index > Count)
                {
                    throw new ArgumentOutOfRangeException("Index is negativ or bigger then number of elements in the list.");
                }

                if (Count >= array.Length - index)
                {
                    throw new ArgumentException("Number of elements from the list is greater than available space from index to the end of the destination array. ");
                }

                array[index + i] = this[i];
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
            if (IsReadOnly)
            {
                throw new NotSupportedException("The list is read-only.");
            }

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
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException("Index is negativ or bigger then number of elements in the list.");
            }

            if (IsReadOnly)
            {
                throw new NotSupportedException("The list is read-only.");
            }

            if (item == null)
            {
                throw new ArgumentNullException("Array can`t be null.");
            }

            Capacity();
            RightShifting(index);
            list[index] = item;
            Count++;
        }

        public bool Remove(T item)
        {
            bool removedItem = false;

            if (IsReadOnly)
            {
                throw new NotSupportedException("The list is read-only.");
            }

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
            if (IsReadOnly)
            {
                throw new NotSupportedException("The list is read-only.");
            }

            Array.Clear(list);
            Count = 0;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException("Index is negativ or bigger then number of elements in the list.");
            }

            if (IsReadOnly)
            {
                throw new NotSupportedException("The list is read-only.");
            }

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
