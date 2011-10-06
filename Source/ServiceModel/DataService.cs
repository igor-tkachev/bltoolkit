using System;
using System.Data.Services.Providers;

namespace BLToolkit.ServiceModel
{
	using Data.Linq;

	public class DataService<T> : System.Data.Services.DataService<T>, IServiceProvider
		where T : IDataContext
	{
		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IDataServiceMetadataProvider))
				return _metadata;

			if (serviceType == typeof(IDataServiceQueryProvider))
			{
			}

			return null;
		}

		IDataServiceMetadataProvider _metadata;

		public DataService()
		{
			_metadata = GetMetadataProvider(typeof(T)); 
		}

		public IDataServiceMetadataProvider GetMetadataProvider(Type dataSourceType)
		{
			return null;
		}
	}
}
