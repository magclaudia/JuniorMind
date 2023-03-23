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

        public void Add(int element) // adaugă un nou element la sfârșitul șirului 
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = element;
        }

        public int Count() // întorce numărul de elemente din șir
        {
            return array.Length;
        }

        public int Element(int index) // întoarce elementul de la indexul dat
        {
            return array[index];
        }

        public void SetElement(int index, int element) // modifică valoarea elementului de la indexul dat
        {
            array[index] = element;
        }

        public bool Contains(int element) // întoarce true dacă elementul dat există în șir
        {
            return array.Contains(element);
        }

        public int IndexOf(int element) // întoarce indexul elementului sau -1 dacă elementul nu se regăsește în șir
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

        public void Insert(int index, int element) // adaugă un nou element pe poziția dată
        {
            Array.Resize(ref array, array.Length + 1);
            SifitingElements(index);
            SetElement(index, element);
        }

        public void Remove(int element) // șterge prima apariție a elementului din șir
        {
            SifitingElements(IndexOf(element));
            Array.Resize(ref array, array.Length - 1);
        }

        public void Clear() // șterge toate elementele din șir
        {
            Array.Clear(array, 0, array.Length);
        }

        public void RemoveAt(int index) // șterge elementul de pe poziția dată
        {
            while (index < array.Length - 1)
            {
                array[index] = array[index + 1];
                index++;
            }

            Array.Resize(ref array, array.Length - 1);
        }

        private int[] SifitingElements(int index)
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

            return array;
        }
    }
}
