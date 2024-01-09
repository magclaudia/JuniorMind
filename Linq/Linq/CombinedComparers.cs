﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linq
{
    public class CombinedComparers<TSource> : IComparer<TSource>
    {
        private readonly IComparer<TSource> firstComparer;
        private readonly IComparer<TSource> secondComparer;

        public CombinedComparers(IComparer<TSource> firstComparer, IComparer<TSource> secondComparer)
        {
            this.firstComparer = firstComparer;
            this.secondComparer = secondComparer;
        }

        public int Compare(TSource? x, TSource? y)
        {
            var comparerResult = firstComparer.Compare(x, y);
            if (comparerResult != 0)
            {
                return comparerResult;
            }

            return secondComparer.Compare(x, y);
        }
    }
}