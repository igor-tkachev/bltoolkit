using System;

using BLToolkit.Mapping;

namespace BLToolkit.Data.Sql
{
	public class SqlTable<T> : SqlTable
	{
		public SqlTable()
			: base(typeof(T))
		{
		}

		public SqlTable(MappingSchema mappingSchema)
			: base(mappingSchema, typeof(T))
		{
		}
	}
}
