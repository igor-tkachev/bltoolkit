﻿using System;
using System.Data;

namespace BLToolkit.Data.Linq
{
	using Data.Sql.SqlProvider;
	using Mapping;

	public interface IDataContext : IMappingSchemaProvider, IDisposable
	{
		string             ContextID         { get; }
		Func<ISqlProvider> CreateSqlProvider { get; }

		object             SetQuery        (IQueryContext queryContext);
		int                ExecuteNonQuery (object query);
		object             ExecuteScalar   (object query);
		IDataReader        ExecuteReader   (object query);
		void               ReleaseQuery    (object query);

		string             GetSqlText      (object query);
		IDataContext       Clone           (bool forNestedQuery);

		event EventHandler OnClosing;
	}
}
