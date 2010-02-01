using System;

namespace BLToolkit.Data
{
	using Linq;

	public partial class DbManager : IDataContext
	{
		public Table<T> GetTable<T>()
			where T : class
		{
			return new Table<T>(this);
		}
	}
}
