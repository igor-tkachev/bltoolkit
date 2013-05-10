using System;
using System.Linq;

namespace BLToolkit.Data.Linq
{
	public interface IUpdatable<out T>
	{
		IQueryable<T> Query { get; }
	}
}
