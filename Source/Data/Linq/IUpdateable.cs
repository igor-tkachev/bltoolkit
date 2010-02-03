using System;
using System.Linq;

namespace BLToolkit.Data.Linq
{
	public interface IUpdateable<T> : IOrderedQueryable<T>
	{
	}
}
