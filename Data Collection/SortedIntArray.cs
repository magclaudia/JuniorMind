using System;

namespace DataCollection
{
    class SortedIntArray : IntArray
    {
        public SortedIntArray()
            : base()
        {

        }

        public override int this[int index]
        {
            set
            {
                if (index >= 0 && index < Count && CheckIndexToBeCorrect(index - 1, index + 1, value))
                {
                    base[index] = value;
                }
            }
        }

        public override void Add(int element)
        {
            base.Add(element);
            ArraySort();
        }

        public override void Insert(int index, int element)
        {
            if (index >= 0 && index <= Count && CheckIndexToBeCorrect(index - 1, index, element))
            {
                base.Insert(index, element);
            }
        }

        private void ArraySort()
        {
            int temp;
            bool arrayIsSort = true;
            while (arrayIsSort)
            {
                arrayIsSort = false;
                for (int i = 0; i < base.Count - 1; i++)
                {
                    if (base[i] > base[i + 1])
                    {
                        temp = base[i + 1];
                        base[i + 1] = base[i];
                        base[i] = temp;
                        arrayIsSort = true;
                    }
                }
            }
        }

        private bool CheckIndexToBeCorrect(int leftIndex, int rightIndex, int element)
        {
            return leftIndex == -1 && element < base[rightIndex] || element < base[rightIndex] && element > base[leftIndex]
                || rightIndex == Count && element > base[leftIndex];
        }
    }
}