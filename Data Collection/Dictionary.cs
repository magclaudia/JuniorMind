using System;
using System.Collections;
using System.Collections.Generic;

namespace DataCollection
{
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Element<TKey, TValue>[] elements;
        private int[] buckets;
        private int freeIndex = -1;
        private int capacity;

        public Dictionary(int maxCapacity)
        {
            capacity = maxCapacity;
            buckets = new int[capacity];
            Array.Fill(buckets, -1);
            elements = new Element<TKey, TValue>[capacity];
        }

        public TValue this[TKey key]
        {
            get
            {
                ExceptionArgumentNullException(key);
                ExceptionKeyNotFoundException(key);
                int index = FindPositionOfKeyInElements(key);
                return elements[index].Value;
            }
            set
            {
                ExceptionNotSupportedException();
                int index = FindPositionOfKeyInElements(key);
                if (index == -1)
                {
                    Add(key, value);
                }
                else
                {
                    elements[index].Value = value;
                }
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                var keys = new List<TKey>();
                foreach (var element in elements)
                {
                    keys.Add(element.Key);
                }

                return keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                var values = new List<TValue>();
                foreach (var element in elements)
                {
                    values.Add(element.Value);
                }

                return values;
            }
        }

        public int Count { get; set; } = 0;

        public bool IsReadOnly { get; }

        public void Add(TKey key, TValue value)
        {
            ExceptionArgumentNullException(key);
            ExceptionArgumentException(key);
            ExceptionNotSupportedException();
            var pair = new Element<TKey, TValue>();
            pair.Key = key;
            pair.Value = value;
            int bucket = GetBucketPosition(key);
            if (freeIndex == -1)
            {
                elements[Count] = pair;
                pair.Next = buckets[bucket];
                buckets[bucket] = Count;
            }
            else
            {
                int nextFreeIndex = elements[freeIndex].Next;
                elements[freeIndex] = pair;
                buckets[bucket] = freeIndex;
                freeIndex = nextFreeIndex;
                pair.Next = freeIndex;
            }

            Count++;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            Count = 0;
            Array.Clear(elements);
            Array.Clear(buckets);
            Array.Fill(buckets, -1);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            ExceptionArgumentNullException(item.Key);
            var bucketValue = FindPositionOfKeyInElements(item.Key);
            if (bucketValue == -1)
            {
                return false;
            }

            var element = elements[bucketValue];
            if (element.Key.Equals(item.Key))
            {
                return true;
            }

            return true;
        }

        public bool ContainsKey(TKey key)
        {
            ExceptionArgumentNullException(key);
            var element = elements[buckets[GetBucketPosition(key)]];
            if (!key.Equals(element.Key))
            {
                return false;
            }

            return true;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array is null)
            {
                throw new ArgumentNullException("Array is null.");
            }

            if (arrayIndex < 0 || arrayIndex > array.Length)
            {
                throw new ArgumentException("Index is less than zero");
            }

            if (Count > (array.Length - arrayIndex))
            {
                throw new ArgumentException("The number of elements in the source DictionaryBase" +
                    " is greater than the available space from index to the end" +
                    " of the destination array.");
            }

            for (int i = 0; i < Count; i++)
            {
                if (elements[i].Next >= -1)
                {
                    var element = new KeyValuePair<TKey, TValue>(elements[i].Key, elements[i].Value);
                    array[i + arrayIndex] = element;                
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            KeyValuePair<TKey, TValue>[] items = new KeyValuePair<TKey, TValue>[Count];
            for (int i = 0; i < Count; i++)
            {
                var element = new KeyValuePair<TKey, TValue>(elements[i].Key, elements[i].Value);
                items[i] = element;
                yield return items[i];
            }

        }

        public bool Remove(TKey key)
        {
            ExceptionArgumentNullException(key);
            ExceptionNotSupportedException();
            int bucketIndex = FindPositionOfKeyInElements(key, out int previous);
            if (bucketIndex == -1)
            {
                return false;
            }

            if (previous == -1)
            {
                buckets[GetBucketPosition(key)] = elements[bucketIndex].Next;
            }
            else
            {
                elements[previous].Next = elements[bucketIndex].Next;
            }

            elements[bucketIndex].Key = default;
            elements[bucketIndex].Value = default;
            elements[bucketIndex].Next = freeIndex;
            freeIndex = bucketIndex;
            Count--;
            return true;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            ExceptionArgumentNullException(key);
            if (FindPositionOfKeyInElements(key) == -1)
            {
                value = default;
                return false;
            }

            value = this[key];
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void ExceptionArgumentNullException(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("Key is null.");
            }
        }

        private void ExceptionKeyNotFoundException(TKey key)
        {
            if (!TryGetValue(key, out TValue value))
            {
                throw new KeyNotFoundException("Key is not found.");
            }
        }

        private void ExceptionNotSupportedException()
        {
            if (!IsReadOnly)
            {
                return;
            }

            throw new NotSupportedException();
        }

        private void ExceptionArgumentException(TKey key)
        {
            int countKeys = 0;
            var keys = new List<TKey>();
            foreach (var keyElem in keys)
            {
                if (keyElem.Equals(key))
                {
                    countKeys++;
                }

                if (countKeys >= 1)
                {
                    throw new ArgumentException("An element with the same key already exists in the IDictionary<TKey,TValue>.");
                }
            }
        }

        private int FindPositionOfKeyInElements(TKey key, out int previous)
        {
            int bucketValue = GetBucketPosition(key);
            int bucketIndex = buckets[bucketValue];
            previous = -1;
            while (bucketIndex != -1)
            {
                if (elements[bucketIndex].Key.Equals(key))
                {
                    return bucketIndex;
                }

                previous = bucketIndex;
                bucketIndex = elements[bucketIndex].Next;
            }

            return -1;
        }

        private int FindPositionOfKeyInElements(TKey key)
        {
            return FindPositionOfKeyInElements(key, out int previous);
        }

        private int GetBucketPosition(TKey key)
        {
            return Math.Abs(key.GetHashCode() % capacity);
        }
    }
}
