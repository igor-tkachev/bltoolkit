using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace BLToolkit.ServiceModel
{
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
				using (var ctx = CreateDataContext())
					SqlProviderType = ctx.CreateSqlProvider().GetType();

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

			using (var db = CreateDataContext())
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query.Query, Parameters = query.Parameters });
				return db.ExecuteNonQuery(obj);
			}
		}

		public object ExecuteScalar(LinqServiceQuery query)
		{
			ValidateQuery(query.Query, query.Parameters);

			using (var db = CreateDataContext())
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query.Query, Parameters = query.Parameters });
				return db.ExecuteScalar(obj);
			}
		}

		public LinqServiceResult ExecuteReader(LinqServiceQuery query)
		{
			ValidateQuery(query.Query, query.Parameters);

			using (var db = CreateDataContext())
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
		}

		#endregion

		protected virtual void ValidateQuery(SqlQuery query, SqlParameter[] parameters)
		{
		}

		public static Func<string,Type> TypeResolver = _ => null;
	}
}
