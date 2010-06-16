using System;
using System.Data;

namespace BLToolkit.Data.Linq
{
	using Mapping;

	public interface IDataContext
	{
		ILinqDataProvider DataProvider { get; }
		MappingSchema     MappingSchema   { get; }

		object            SetQuery        (IQueryContext queryContext);
		int               ExecuteNonQuery (object query);
		object            ExecuteScalar   (object query);
		IDataReader       ExecuteReader   (object query);
		string            GetSqlText      (object query);
		IDataContext      Clone           ();

		event EventHandler OnClosing;
	}
}
