using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data
{
	using Linq;

	public partial class DbManager
	{
		public Table<T> GetTable<T>()
			where T : class
		{
			return new Table<T>(this);
		}

		public T Select<T>(Expression<Func<T>> selector)
		{
			if (selector == null) throw new ArgumentNullException("selector");

			var q = new Table<T>(this, selector);

			foreach (var item in q)
				return item;

			throw new InvalidOperationException();
		}
	}
}
