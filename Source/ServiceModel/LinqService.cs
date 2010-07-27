using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;

namespace BLToolkit.ServiceModel
{
	using Common;
	using Data;
	using Data.Linq;
	using Data.Sql;

	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.Single,
		ConcurrencyMode     = ConcurrencyMode.Multiple)]
	public class LinqService : ILinqService
	{
		public string Configuration { get; set; }

		public LinqService()
		{
		}

		public LinqService(string configuration)
		{
			Configuration = configuration;
		}

		public virtual IDataContext CreateDataContext()
		{
			return Settings.CreateDefaultDataContext(Configuration);
		}

		#region ILinqService Members

		public Type SqlProviderType { get; set; }

		public virtual string GetSqlProviderType()
		{
			if (SqlProviderType == null)
			{
				var ctx = CreateDataContext();

				try
				{
					SqlProviderType = ctx.CreateSqlProvider().GetType();
				}
				finally
				{
					if (ctx is IDisposable)
						((IDisposable)ctx).Dispose();
				}
			}

			return SqlProviderType.FullName;
		}

		class QueryContext : IQueryContext
		{
			public SqlQuery       SqlQuery   { get; set; }
			public object         Context    { get; set; }
			public SqlParameter[] Parameters { get; set; }

			public SqlParameter[] GetParameters()
			{
				return Parameters;
			}
		}

		public virtual int ExecuteNonQuery(LinqServiceQuery query)
		{
			ValidateQuery(query.Query, query.Parameters);

			var db = CreateDataContext();

			try
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query.Query, Parameters = query.Parameters });
				return db.ExecuteNonQuery(obj);
			}
			finally
			{
				if (db is IDisposable)
					((IDisposable)db).Dispose();
			}
		}

		public object ExecuteScalar(LinqServiceQuery query)
		{
			ValidateQuery(query.Query, query.Parameters);

			var db = CreateDataContext();

			try
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query.Query, Parameters = query.Parameters });
				return db.ExecuteScalar(obj);
			}
			finally
			{
				if (db is IDisposable)
					((IDisposable)db).Dispose();
			}
		}

		public LinqServiceResult ExecuteReader(LinqServiceQuery query)
		{
			ValidateQuery(query.Query, query.Parameters);

			var db = CreateDataContext();

			try
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query.Query, Parameters = query.Parameters });

				using (var rd = db.ExecuteReader(obj))
				{
					var ret = new LinqServiceResult
					{
						QueryID    = Guid.NewGuid(),
						FieldCount = rd.FieldCount,
						FieldNames = new string[rd.FieldCount],
						FieldTypes = new Type  [rd.FieldCount],
						Data       = new List<string[]>(),
					};

					for (var i = 0; i < ret.FieldCount; i++)
					{
						ret.FieldNames[i] = rd.GetName(i);
						ret.FieldTypes[i] = rd.GetFieldType(i);
					}

					while (rd.Read())
					{
						var data = new string[rd.FieldCount];

						ret.RowCount++;

						for (var i = 0; i < ret.FieldCount; i++)
						{
							if (!rd.IsDBNull(i))
								data[i] = (rd.GetValue(i) ?? "").ToString();
						}

						ret.Data.Add(data);
					}

					return ret;
				}
			}
			finally
			{
				if (db is IDisposable)
					((IDisposable)db).Dispose();
			}
		}

		#endregion

		protected virtual void ValidateQuery(SqlQuery query, SqlParameter[] parameters)
		{
			
		}
	}
}
