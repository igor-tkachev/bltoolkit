using System;
using System.Data;

namespace BLToolkit.Data.Linq
{
	using Data.Sql.SqlProvider;
	using Mapping;
	using Reflection;

	public interface IDataContext : IMappingSchemaProvider, IDisposable
	{
		string             ContextID         { get; }
		Func<ISqlProvider> CreateSqlProvider { get; }

		object             SetQuery        (IQueryContext queryContext);
		int                ExecuteNonQuery (object query);
		object             ExecuteScalar   (object query);
		IDataReader        ExecuteReader   (object query);
		void               ReleaseQuery    (object query);

		object             CreateInstance  (InitContext context);

		string             GetSqlText      (object query);
		IDataContext       Clone           ();

		event EventHandler OnClosing;
	}
}
