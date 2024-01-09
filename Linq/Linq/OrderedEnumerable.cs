using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq
{
    public class OrderedEnumerable<TSource> : IOrderedEnumerable<TSource>
    {
        private readonly IEnumerable<TSource> source;
        private readonly IComparer<TSource> comparer;
       
        public OrderedEnumerable(IEnumerable<TSource> source, IComparer<TSource> comparer) 
        {
            this.source = source;
            this.comparer = comparer;
        }
        
        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new OrderedEnumerable<TSource>(source, new CombinedComparers<TSource>(this.comparer, new SourceComparer<TSource, TKey>(comparer, keySelector)));
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            var list = source.ToList();
            QuickSort(list, 0, list.Count - 1);
            foreach (var item in list)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void QuickSort(List<TSource> list, int lowIndex, int hightIndex)
        {
            if (lowIndex < hightIndex)
            {
                var partitionIndex = Partition(list, lowIndex, hightIndex);
                QuickSort(list, lowIndex, partitionIndex - 1);
                QuickSort(list, partitionIndex + 1, hightIndex);
            }
        }
         
        private int Partition(List<TSource> list, int lowIndex, int hightIndex)
        {
            var pivot = hightIndex;
            int i = lowIndex - 1; 
            for (int j = lowIndex; j < hightIndex; j++)
            {
                if (comparer.Compare(list[j], list[pivot]) < 0)
                {
                    i++;
                    Swap(list, i, j);
                }
            }

            Swap(list, i + 1, hightIndex);
            return i + 1;
        }

        private void Swap(List<TSource> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
