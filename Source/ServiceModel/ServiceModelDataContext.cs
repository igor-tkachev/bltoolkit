using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

using JetBrains.Annotations;

namespace BLToolkit.ServiceModel
{
	using Data.Linq;
	using Data.Sql.SqlProvider;
	using Mapping;

	using NotNullAttribute = NotNullAttribute;

	public class ServiceModelDataContext : IDataContext
	{
		#region Init

		ServiceModelDataContext()
		{
			MappingSchema = Map.DefaultSchema;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName)
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");

			_endpointConfigurationName = endpointConfigurationName;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName, [NotNull] string remoteAddress)
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");
			if (remoteAddress             == null) throw new ArgumentNullException("remoteAddress");

			_endpointConfigurationName = endpointConfigurationName;
			_remoteAddress             = remoteAddress;
		}

		public ServiceModelDataContext([NotNull] string endpointConfigurationName, [NotNull] EndpointAddress endpointAddress)
		{
			if (endpointConfigurationName == null) throw new ArgumentNullException("endpointConfigurationName");
			if (endpointAddress           == null) throw new ArgumentNullException("endpointAddress");

			_endpointConfigurationName = endpointConfigurationName;
			_endpointAddress           = endpointAddress;
		}

		public ServiceModelDataContext([NotNull] Binding binding, [NotNull] EndpointAddress endpointAddress)
		{
			if (binding         == null) throw new ArgumentNullException("binding");
			if (endpointAddress == null) throw new ArgumentNullException("endpointAddress");

			_binding         = binding;
			_endpointAddress = endpointAddress;
		}

		string          _endpointConfigurationName;
		string          _remoteAddress;
		EndpointAddress _endpointAddress;
		Binding         _binding;

		#endregion

		LinqServiceClient GetClient()
		{
			if (_binding != null)
				return new LinqServiceClient(_binding, _endpointAddress);

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
				{
					using (var client = GetClient())
						_sqlProviderType = client.GetSqlProviderType();
				}

				return _sqlProviderType;
			}

			set { _sqlProviderType = value;  }
		}

		Func<ISqlProvider> IDataContext.CreateSqlProvider
		{
			get { throw new NotImplementedException(); }
		}

		object IDataContext.SetQuery(IQueryContext queryContext)
		{
			throw new NotImplementedException();
		}

		int IDataContext.ExecuteNonQuery(object query)
		{
			throw new NotImplementedException();
		}

		object IDataContext.ExecuteScalar(object query)
		{
			throw new NotImplementedException();
		}

		System.Data.IDataReader IDataContext.ExecuteReader(object query)
		{
			throw new NotImplementedException();
		}

		object IDataContext.CreateInstance(Reflection.InitContext context)
		{
			throw new NotImplementedException();
		}

		string IDataContext.GetSqlText(object query)
		{
			if (OnClosing != null)
				throw new NotImplementedException();
			throw new NotImplementedException();
		}

		IDataContext IDataContext.Clone()
		{
			return new ServiceModelDataContext
			{
				MappingSchema              = MappingSchema,
				_endpointConfigurationName = _endpointConfigurationName,
				_remoteAddress             = _remoteAddress,
				_endpointAddress           = _endpointAddress,
				_binding                   = _binding,
			};
		}

		public event EventHandler OnClosing;
	}
}
