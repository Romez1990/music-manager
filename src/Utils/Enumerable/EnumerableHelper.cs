using System;
using System.Collections.Generic;

namespace Utils.Enumerable {
    public static class EnumerableHelper {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
            foreach (var element in source) {
                action(element);
            }
        }
    }
}
