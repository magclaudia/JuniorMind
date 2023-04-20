using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class ReadOnlyList<T> : IList<T>
    {
        private readonly IList<T> originalList;

        public ReadOnlyList(IList<T> originalList)
        {
            this.originalList = originalList;
        }

        public int Count
        {
            get { return originalList.Count; }
        }

        public bool IsReadOnly => originalList.IsReadOnly;
        public T this[int index]
        {
            get
            {
                return originalList[index];
            }
            set
            {
                throw new NotSupportedException("The list is read-only.");
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            originalList.CopyTo(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return originalList.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }


        public void Add(T item)
        {
            throw new NotSupportedException("The list is read-only.");
        }

        public bool Contains(T item)
        {
            return originalList.Contains(item);
        }

        public int IndexOf(T item)
        {
            return originalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("The list is read-only.");
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("The list is read-only.");
        }

        public void Clear()
        {
            throw new NotSupportedException("The list is read-only.");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("The list is read-only.");
        }
    }
}
