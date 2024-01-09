using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq
{
    public class SourceComparer<TSource, TKey> : IComparer<TSource>
    {
        private readonly IComparer<TKey> comparer;
        private readonly Func<TSource, TKey> keySelector;

        public SourceComparer(IComparer<TKey> comparer, Func<TSource, TKey> keySelector)
        {
            this.comparer = comparer;
            this.keySelector = keySelector;
        }

        public int Compare(TSource? x, TSource? y)
        {
            return comparer.Compare(keySelector(x), keySelector(y));
        }
    }
}
