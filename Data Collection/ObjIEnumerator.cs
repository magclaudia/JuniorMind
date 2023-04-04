using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    class ObjIEnumerator : IEnumerator
    {
        private ObjectArray array;
        private int index = -1;

        public ObjIEnumerator(ObjectArray objArray)
        {
            array = objArray;
        }

        public object Current
        {
            get
            {
                if (index < 0 || index >= array.Count)
                {
                    return null;
                }
                else
                {
                    return array[index];
                }
            }
        }

        public bool MoveNext()
        {
            index++;
            return index < array.Count;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
