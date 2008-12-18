using System;

using BLToolkit.Mapping;

namespace BLToolkit.Data.Sql
{
	public class Table<T> : Table
	{
		public Table()
			: base(typeof(T))
		{
		}

		public Table(MappingSchema mappingSchema)
			: base(mappingSchema, typeof(T))
		{
		}
	}
}
