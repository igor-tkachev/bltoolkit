using System;
using System.Collections.Generic;
using BLToolkit.Properties;
using BLToolkit.Reflection;

namespace BLToolkit.Data.DataProvider
{
	public abstract class DataProviderVersionResolverBase
	{
		private readonly Dictionary<string, DataProviderBase> _providersToStrings =
			new Dictionary<string, DataProviderBase>(StringComparer.OrdinalIgnoreCase);

		private readonly Type _baseProviderType;

		private readonly Dictionary<Type, DataProviderBase> _providers = new Dictionary<Type, DataProviderBase>();

		protected DataProviderVersionResolverBase(Type baseProviderType)
		{
			if (baseProviderType == null)
				throw new ArgumentNullException("baseProviderType");

			_baseProviderType = baseProviderType;
		}

		public void AddDataProvider(DataProviderBase dataProvier)
		{
			if (dataProvier == null)
				throw new ArgumentNullException("dataProvier");

			var providerType = dataProvier.GetType();
			if (!TypeHelper.IsSameOrParent(_baseProviderType, providerType))
				throw new ArgumentException(
					string.Format(Resources.DataProviderVersionResolverBase_InvalidProviderType, providerType.FullName, _baseProviderType.FullName));

			lock (_providers)
			{
				_providers[providerType] = dataProvier;
			}
		}

		protected DataProviderBase GetProviderByType(Type providerType)
		{
			return _providers[providerType];
		}

		public DataProviderBase InvalidateDataProvider(DataProviderBase dataProvider, string configuration, string connectionString)
		{
			var dataProviderType = dataProvider.GetType();
			if (!_providers.ContainsKey(dataProviderType))
			{
				throw new InvalidOperationException(
					string.Format(Resources.DataProviderVersionResolverBase_ProviderIsNotRegistered, dataProvider.UniqueName,
					              dataProviderType.FullName));
			}

			DataProviderBase result;
			if (_providersToStrings.TryGetValue(connectionString, out result))
				return result;

			lock (_providersToStrings)
			{
				if (_providersToStrings.TryGetValue(connectionString, out result))
					return result;

				result = InvalidateDataProviderInternal(dataProvider, configuration, connectionString);

				if (result != null)
					_providersToStrings.Add(connectionString, result);
			}

			return result;
		}

		protected abstract DataProviderBase InvalidateDataProviderInternal(DataProviderBase dataProvider, string configuration, string connectionString);
	}
}
