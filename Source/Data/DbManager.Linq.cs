using System;

namespace BLToolkit.Data
{
	using Linq;

	public partial class DbManager
	{
		public Table<TEntity> GetTable<TEntity>()
			where TEntity : class
		{
			return new Table<TEntity>(this);
		}
	}
}
