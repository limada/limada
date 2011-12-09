using System.Collections.Generic;
using System.Linq;

namespace Limada.Common.Linqish {
    public static class LinqishExtensions {
        public static IEnumerable<T> Yield<T>(this IEnumerable<T> query) {
            foreach (var item in query)
                yield return item;
        }
    }
}