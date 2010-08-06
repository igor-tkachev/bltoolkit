using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using JetBrains.Annotations;

namespace BLToolkit.ServiceModel
{
	using Data.Linq;
	using Data.Sql.SqlProvider;
	using Mapping;
	using Reflection;

	using NotNullAttribute = NotNullAttribute;

	public class ServiceModelDataContext : IDataContext
	{
		#region Init

		ServiceModelDataContext()
		{
			MappingSchema = Map.DefaultSchema;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName)
			: this()
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");

			_endpointConfigurationName = endpointConfigurationName;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName, [NotNull] string remoteAddress)
			: this()
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");
			if (remoteAddress             == null) throw new ArgumentNullException("remoteAddress");

			_endpointConfigurationName = endpointConfigurationName;
			_remoteAddress             = remoteAddress;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName, [NotNull] EndpointAddress endpointAddress)
			: this()
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");
			if (endpointAddress           == null) throw new ArgumentNullException("endpointAddress");

			_endpointConfigurationName = endpointConfigurationName;
			_endpointAddress           = endpointAddress;
		}

		public ServiceModelDataContext([NotNull] Binding binding, [NotNull] EndpointAddress endpointAddress)
			: this()
		{
			if (binding         == null) throw new ArgumentNullException("binding");
			if (endpointAddress == null) throw new ArgumentNullException("endpointAddress");

			Binding          = binding;
			_endpointAddress = endpointAddress;
		}

		string          _endpointConfigurationName;
		string          _remoteAddress;
		EndpointAddress _endpointAddress;

		public Binding Binding { get; private set; }

		#endregion

		LinqServiceClient GetClient()
		{
			if (Binding != null)
				return new LinqServiceClient(Binding, _endpointAddress);

			if (_endpointAddress != null)
				return new LinqServiceClient(_endpointConfigurationName, _endpointAddress);

			if (_remoteAddress != null)
				return new LinqServiceClient(_endpointConfigurationName, _remoteAddress);

			return new LinqServiceClient(_endpointConfigurationName);
		}

		string IDataContext.ContextID
		{
			get { return "ServiceModelDataContext"; }
		}

		public         MappingSchema MappingSchema   { get; set; }

		private        Type _sqlProviderType;
		public virtual Type  SqlProviderType
		{
			get
			{
				if (_sqlProviderType == null)
					using (var client = GetClient())
						_sqlProviderType = Type.GetType(client.GetSqlProviderType());

				return _sqlProviderType;
			}

			set { _sqlProviderType = value;  }
		}

		static readonly Dictionary<Type,Func<ISqlProvider>> _sqlProviders = new Dictionary<Type, Func<ISqlProvider>>();

		Func<ISqlProvider> _createSqlProvider;

		Func<ISqlProvider> IDataContext.CreateSqlProvider
		{
			get
			{
				if (_createSqlProvider == null)
				{
					var type = SqlProviderType;

					if (!_sqlProviders.TryGetValue(type, out _createSqlProvider))
						lock (_sqlProviderType)
							if (!_sqlProviders.TryGetValue(type, out _createSqlProvider))
								_sqlProviders.Add(type, _createSqlProvider = Expression.Lambda<Func<ISqlProvider>>(Expression.New(type)).Compile());
				}

				return _createSqlProvider;
			}
		}

		class QueryContext
		{
			public IQueryContext     Query;
			public LinqServiceClient Client;
		}

		object IDataContext.SetQuery(IQueryContext queryContext)
		{
			return new QueryContext { Query = queryContext };
		}

		int IDataContext.ExecuteNonQuery(object query)
		{
			var ctx = (QueryContext)query;

			ctx.Client = GetClient();

			var q = ctx.Query.SqlQuery.ProcessParameters();

			return ctx.Client.ExecuteNonQuery(new LinqServiceQuery
			{
				Query      = q,
				Parameters = q.ParameterDependent ? q.Parameters.ToArray() : ctx.Query.GetParameters()
			});
		}

		object IDataContext.ExecuteScalar(object query)
		{
			var ctx = (QueryContext)query;

			ctx.Client = GetClient();

			var q = ctx.Query.SqlQuery.ProcessParameters();

			return ctx.Client.ExecuteScalar(new LinqServiceQuery
			{
				Query      = q,
				Parameters = q.ParameterDependent ? q.Parameters.ToArray() : ctx.Query.GetParameters()
			});
		}

		IDataReader IDataContext.ExecuteReader(object query)
		{
			var ctx = (QueryContext)query;

			ctx.Client = GetClient();

			var q = ctx.Query.SqlQuery.ProcessParameters();

			LinqServiceResult ret;

			ret = ctx.Client.ExecuteReader(new LinqServiceQuery
			{
				Query      = q,
				Parameters = q.ParameterDependent ? q.Parameters.ToArray() : ctx.Query.GetParameters()
			});

			return new ServiceModelDataReader(ret);
		}

		public void ReleaseQuery(object query)
		{
			var ctx = (QueryContext)query;

			((IDisposable)ctx.Client).Dispose();
		}

		object IDataContext.CreateInstance(InitContext context)
		{
			return null;
		}

		string IDataContext.GetSqlText(object query)
		{
			var ctx         = (QueryContext)query;
			var sqlProvider = ((IDataContext)this).CreateSqlProvider();
			var sb          = new StringBuilder();

			sb
				.Append("-- ")
				.Append("ServiceModel")
				.Append(' ')
				.Append(((IDataContext)this).ContextID)
				.Append(' ')
				.Append(sqlProvider.Name)
				.AppendLine();

			if (ctx.Query.SqlQuery.Parameters != null && ctx.Query.SqlQuery.Parameters.Count > 0)
			{
				foreach (var p in ctx.Query.SqlQuery.Parameters)
					sb
						.Append("-- DECLARE ")
						.Append(p.Name)
						.Append(' ')
						.Append(p.Value == null ? p.SystemType.ToString() : p.Value.GetType().Name)
						.AppendLine();

				sb.AppendLine();

				foreach (var p in ctx.Query.SqlQuery.Parameters)
				{
					var value = p.Value;

					if (value is string || value is char)
						value = "'" + value.ToString().Replace("'", "''") + "'";

					sb
						.Append("-- SET ")
						.Append(p.Name)
						.Append(" = ")
						.Append(value)
						.AppendLine();
				}

				sb.AppendLine();
			}

			var cc       = sqlProvider.CommandCount(ctx.Query.SqlQuery);
			var commands = new string[cc];

			for (var i = 0; i < cc; i++)
			{
				sb.Length = 0;

				sqlProvider.BuildSql(i, ctx.Query.SqlQuery, sb, 0, 0, false);
				commands[i] = sb.ToString();
			}

			if (!ctx.Query.SqlQuery.ParameterDependent)
				ctx.Query.Context = commands;

			foreach (var command in commands)
				sb.AppendLine(command);

			return sb.ToString();
		}

		IDataContext IDataContext.Clone()
		{
			return new ServiceModelDataContext
			{
				MappingSchema              = MappingSchema,
				_endpointConfigurationName = _endpointConfigurationName,
				_remoteAddress             = _remoteAddress,
				_endpointAddress           = _endpointAddress,
				Binding                   = Binding,
			};
		}

		public event EventHandler OnClosing;

		public void Dispose()
		{
			if (OnClosing != null)
				OnClosing(this, EventArgs.Empty);
		}
	}
}
