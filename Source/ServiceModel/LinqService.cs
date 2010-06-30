using System;
using System.ServiceModel;

using BLToolkit.Data.Linq;
using BLToolkit.Data.Sql;

namespace BLToolkit.ServiceModel
{
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
			public SqlQuery       SqlQuery { get; set; }
			public object         Context  { get; set; }
			public SqlParameter[] Parameters { get; set; }

			public SqlParameter[] GetParameters()
			{
				return Parameters;
			}
		}

		public virtual int ExecuteNonQuery(SqlQuery query, SqlParameter[] parameters)
		{
			ValidateQuery(query, parameters);

			var db = CreateDataContext();

			try
			{
				var obj = db.SetQuery(new QueryContext { SqlQuery = query, Parameters = parameters });
				return db.ExecuteNonQuery(obj);
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
