using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

using BLToolkit.Aspects;

namespace BLToolkit.Data
{
	using DataProvider;
	using Linq;
	using Reflection;
	using Sql;
	using Sql.SqlProvider;

	public partial class DbManager : IDataContext
	{
		public Table<T> GetTable<T>()
			where T : class
		{
			return new Table<T>(this);
		}

		public Table<T> GetTable<T>(bool dispose)
			where T : class
		{
			return new Table<T>(new DataContextInfo(this, dispose));
		}

		public Table<T> GetTable<T>(object instance, [NotNull]MethodInfo methodInfo, [NotNull] params object[] parameters)
			where T : class
		{
			return Linq.Extensions.GetTable<T>(this, instance, methodInfo, parameters);
		}

		class PreparedQuery
		{
			public string[]           Commands;
			public List<SqlParameter> SqlParameters;
			public IDbDataParameter[] Parameters;
			public SqlQuery           SqlQuery;
			public ISqlProvider       SqlProvider;
		}

		#region SetQuery

		object IDataContext.SetQuery(IQueryContext queryContext)
		{
			var query = GetCommand(queryContext);

			GetParameters(queryContext, query);

			if (TraceSwitch.TraceInfo)
				WriteTraceLine(((IDataContext)this).GetSqlText(query), TraceSwitch.DisplayName);

			return query;
		}

		PreparedQuery GetCommand(IQueryContext query)
		{
			if (query.Context != null)
			{
				return new PreparedQuery
				{
					Commands      = (string[])query.Context,
					SqlParameters = query.SqlQuery.Parameters,
					SqlQuery      = query.SqlQuery
				 };
			}

			var sql = query.SqlQuery.ProcessParameters();

			var newSql = ProcessQuery(sql);

			if (sql != newSql)
			{
				sql = newSql;
				sql.ParameterDependent = true;
			}

			var sqlProvider = DataProvider.CreateSqlProvider();

			var cc = sqlProvider.CommandCount(sql);
			var sb = new StringBuilder();

			var commands = new string[cc];

			for (var i = 0; i < cc; i++)
			{
				sb.Length = 0;

				sqlProvider.BuildSql(i, sql, sb, 0, 0, false);
				commands[i] = sb.ToString();
			}

			if (!query.SqlQuery.ParameterDependent)
				query.Context = commands;

			return new PreparedQuery
			{
				Commands      = commands,
				SqlParameters = sql.Parameters,
				SqlQuery      = sql,
				SqlProvider   = sqlProvider
			};
		}

		protected virtual SqlQuery ProcessQuery(SqlQuery sqlQuery)
		{
			return sqlQuery;
		}

		void GetParameters(IQueryContext query, PreparedQuery pq)
		{
			var parameters = query.GetParameters();

			if (parameters.Length == 0 && pq.SqlParameters.Count == 0)
				return;

			var x = DataProvider.Convert("x", ConvertType.NameToQueryParameter).ToString();
			var y = DataProvider.Convert("y", ConvertType.NameToQueryParameter).ToString();

			var parms = new List<IDbDataParameter>(x == y ? pq.SqlParameters.Count : parameters.Length);

			if (x == y)
			{
				for (var i = 0; i < pq.SqlParameters.Count; i++)
				{
					var sqlp = pq.SqlParameters[i];

					if (sqlp.IsQueryParameter)
					{
						var parm = parameters.Length > i && parameters[i] == sqlp ? parameters[i] : parameters.First(p => p == sqlp);
						parms.Add(Parameter(x, parm.Value));
					}
				}
			}
			else
			{
				foreach (var parm in parameters)
				{
					if (parm.IsQueryParameter && pq.SqlParameters.Contains(parm))
					{
						var name = DataProvider.Convert(parm.Name, ConvertType.NameToQueryParameter).ToString();
						parms.Add(Parameter(name, parm.Value));
					}
				}
			}

			pq.Parameters = parms.ToArray();
		}

		#endregion

		#region ExecuteXXX

		int IDataContext.ExecuteNonQuery(object query)
		{
			var pq = (PreparedQuery)query;

			SetCommand(pq.Commands[0], pq.Parameters);

			return ExecuteNonQuery();
		}

		object IDataContext.ExecuteScalar(object query)
		{
			var pq = (PreparedQuery)query;

			SetCommand(pq.Commands[0], pq.Parameters);

			IDbDataParameter idparam = null;

			if ((pq.SqlProvider ?? DataProvider.CreateSqlProvider()).IsIdentityParameterRequired)
			{
				var sql = pq.SqlQuery;

				if (sql.QueryType == QueryType.Insert && sql.Set.WithIdentity)
				{
					var pname = DataProvider.Convert("IDENTITY_PARAMETER", ConvertType.NameToQueryParameter).ToString();
					idparam = OutputParameter(pname, DbType.Decimal);
					DataProvider.AttachParameter(Command, idparam);
				}
			}

			if (pq.Commands.Length == 1)
			{
				var ret = ExecuteScalar();
				return idparam != null ? idparam.Value : ret;
			}

			ExecuteNonQuery();

			return SetCommand(pq.Commands[1]).ExecuteScalar();
		}

		IDataReader IDataContext.ExecuteReader(object query)
		{
			var pq = (PreparedQuery)query;

			SetCommand(pq.Commands[0], pq.Parameters);

			return ExecuteReader();
		}

		void IDataContext.ReleaseQuery(object query)
		{
		}

		#endregion

		#region GetSqlText

		string IDataContext.GetSqlText(object query)
		{
			var pq = (PreparedQuery)query;

			var sqlProvider = pq.SqlProvider ?? DataProvider.CreateSqlProvider();

			var sb = new StringBuilder();

			sb.Append("-- ").Append(ConfigurationString);

			if (ConfigurationString != DataProvider.Name)
				sb.Append(' ').Append(DataProvider.Name);

			if (DataProvider.Name != sqlProvider.Name)
				sb.Append(' ').Append(sqlProvider.Name);

			sb.AppendLine();

			if (pq.Parameters != null && pq.Parameters.Length > 0)
			{
				foreach (var p in pq.Parameters)
					sb
						.Append("-- DECLARE ")
						.Append(p.ParameterName)
						.Append(' ')
						.Append(p.Value == null ? p.DbType.ToString() : p.Value.GetType().Name)
						.AppendLine();

				sb.AppendLine();

				foreach (var p in pq.Parameters)
				{
					var value = p.Value;

					if (value is string || value is char)
						value = "'" + value.ToString().Replace("'", "''") + "'";

					sb
						.Append("-- SET ")
						.Append(p.ParameterName)
						.Append(" = ")
						.Append(value)
						.AppendLine();
				}

				sb.AppendLine();
			}

			foreach (var command in pq.Commands)
				sb.AppendLine(command);

			return sb.ToString();
		}

		#endregion

		#region IDataContext Members

		IDataContext IDataContext.Clone()
		{
			return Clone();
		}

		string IDataContext.ContextID
		{
			get { return DataProvider.Name; }
		}

		static Func<ISqlProvider> GetCreateSqlProvider(DataProviderBase dp)
		{
			return dp.CreateSqlProvider;
		}

		Func<ISqlProvider> IDataContext.CreateSqlProvider
		{
			get { return GetCreateSqlProvider(DataProvider); }
		}

		object IDataContext.CreateInstance(InitContext context)
		{
			return null;
		}

		#endregion
	}
}
