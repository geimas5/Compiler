﻿namespace Compiler.Common
{
    using System.Collections.Generic;

    public static class ListExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
