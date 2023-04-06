using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class SortedList<T> : List<T> where T : IComparable<T>
    {
        public SortedList()
           : base()
        {

        }

        public override T this[int index]
        {
            set
            {
                if (index >= 0 && index < Count && CheckIndexToBeCorrect(index - 1, index + 1, value))
                {
                    base[index] = value;
                }
            }
        }

        public override void Add(T element)
        {
            base.Add(element);
            Sort();
        }

        public override void Insert(int index, T element)
        {
            if (index >= 0 && index <= Count && CheckIndexToBeCorrect(index - 1, index, element))
            {
                base.Insert(index, element);
            }
        }

        private void Sort()
        {
            T temp;
            bool isSorted = true;
            while (isSorted)
            {
                isSorted = false;
                for (int i = 0; i < base.Count - 1; i++)
                {
                    if (base[i].CompareTo(base[i + 1]) > 0)
                    {
                        temp = base[i + 1];
                        base[i + 1] = base[i];
                        base[i] = temp;
                        isSorted = true;
                    }
                }
            }
        }

        private bool CheckIndexToBeCorrect(int leftIndex, int rightIndex, T element)
        {
            return leftIndex == -1 && element.CompareTo(base[rightIndex]) <= 0 || element.CompareTo(base[rightIndex]) <= 0 && element.CompareTo(base[leftIndex]) >= 0
                || rightIndex == Count && element.CompareTo(base[leftIndex]) > 0;
        }
    }
}