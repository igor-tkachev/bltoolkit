using System.Collections.Generic;
using System.Linq;

namespace BLToolkit.Data.DataProvider
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ToPages<T>(this IEnumerable<T> source, int pageSize)
        {
            var page = new List<T>(pageSize);
            foreach (var x in source)
            {
                page.Add(x);
                if (page.Count < pageSize) continue;

                yield return page;
                page = new List<T>(pageSize);
            }

            // Last page
            if (page.Any())
                yield return page;
        }
    }
}