using System;

namespace DataCollection
{
    class IntArray
    {
        public int[] array;
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
            for (int i = 0; i < array.Length; i++)
            {
                if (i == index)
                {
                    return array[i];
                }
            }

            return -1;
        }

        public void SetElement(int index, int element) // modifică valoarea elementului de la indexul dat
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i == index)
                {
                    array[i] = element;
                }
            }
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
            for (int i = array.Length - 1; i > index; i--)
            {
                array[i] = array[i - 1];
            }

            SetElement(index, element);
        }

        public void Clear() // șterge toate elementele din șir
        {
            Array.Clear(array, 0, array.Length);
        }

        public void Remove(int element) // șterge prima apariție a elementului din șir
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] == element)
                {
                    while (i < array.Length - 1)
                    {
                        array[i] = array[i + 1];
                        i++;
                    }
                }
            }

            Array.Resize(ref array, array.Length - 1);
        }

        public void RemoveAt(int index) // șterge elementul de pe poziția dată
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (i == index)
                {
                    while (i < array.Length - 1)
                    {
                        array[i] = array[i + 1];
                        i++;
                    }
                }
            }

            Array.Resize(ref array, array.Length - 1);
        }
    }
}
