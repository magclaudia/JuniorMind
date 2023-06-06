using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        }

        public bool ContainsKey(TKey key)
        {
           
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
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

        private int FindPositionOfKeyInElements(TKey key)
        {
            int bucketIndex = buckets[GetBucketPosition(key)];
            int prevBucket = -1;
            while (bucketIndex != -1)
            {
                if (elements[bucketIndex].Key.Equals(key))
                {
                    return bucketIndex;
                }

                prevBucket = bucketIndex;
                bucketIndex = elements[bucketIndex].Next;
            }

            return -1;
        }

        private int GetBucketPosition(TKey key)
        {
            return Math.Abs(key.GetHashCode() % capacity);
        }
    }
}
