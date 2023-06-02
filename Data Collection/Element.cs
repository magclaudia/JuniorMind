using System;
using System.Collections.Generic;

namespace DataCollection
{
    public class Element<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public int Next { get; set; } = -1;
    }
}
