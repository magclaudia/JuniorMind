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
            set => base[index] = value;
        }

        public override void Add(int element)
        {
            base.Add(element);
            ArraySort();
        }

        public void ArraySort()
        {
            int temp;
            bool arrayIsSort = true;
            while (arrayIsSort)
            {
                arrayIsSort = false;
                for (int i = 0; i < base.Count - 1; i++)
                {
                    if (base.array[i] > base.array[i + 1])
                    {
                        temp = base.array[i + 1];
                        base.array[i + 1] = base.array[i];
                        base.array[i] = temp;
                        arrayIsSort = true;
                    }
                }
            }
        }
    }
}
