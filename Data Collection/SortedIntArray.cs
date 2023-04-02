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
            get => base[index];
            set
            {
                if (CheckIndexToBeCorrect(index, value))
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
            if (CheckIndexToBeCorrect(index, element))
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

        private bool CheckIndexToBeCorrect(int index, int element)
        {
            if (index == 0 && element < base[index + 1] || element < base[index + 1] && element > base[index - 1])
            {
                return true;
            }

            return false;
        }
    }
}
