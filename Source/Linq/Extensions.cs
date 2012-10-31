using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace BLToolkit.Linq
{
	public static class Extensions
	{
		public static IEnumerable<TResult> Zip<TFirst,TSecond,TResult>(
			[NotNull] this IEnumerable<TFirst>     first,
			[NotNull] IEnumerable<TSecond>         second,
			[NotNull] Func<TFirst,TSecond,TResult> resultSelector)
		{
			if (first          == null) throw new ArgumentNullException("first");
			if (second         == null) throw new ArgumentNullException("second");
			if (resultSelector == null) throw new ArgumentNullException("resultSelector");

			using (var e1 = first.GetEnumerator())
			using (var e2 = second.GetEnumerator())
				while (e1.MoveNext() && e2.MoveNext())
					yield return resultSelector(e1.Current, e2.Current);
		}
	}
}
