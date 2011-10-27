using System;
using System.Data.Services.Providers;
using System.Linq;

namespace BLToolkit.ServiceModel
{
	using Data.Linq;
	using Data.Sql;
	using Mapping;

	public class DataService<T> : System.Data.Services.DataService<T>, IServiceProvider
		where T : IDataContext
	{
		public DataService() : this(Map.DefaultSchema)
		{
		}

		public DataService(MappingSchema mappingSchema)
		{
			_mappingSchema = mappingSchema;
			_metadata = GetMetadataProvider(typeof(T));
		}

		readonly MappingSchema _mappingSchema;

		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IDataServiceMetadataProvider))
				return _metadata;

			if (serviceType == typeof(IDataServiceQueryProvider))
			{
			}

			return null;
		}

		readonly IDataServiceMetadataProvider _metadata;

		public IDataServiceMetadataProvider GetMetadataProvider(Type dataSourceType)
		{
			var properties =
				from p in dataSourceType.GetProperties()
				let t = p.PropertyType
				where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Table<>)
				select new SqlTable(_mappingSchema, t.GetGenericArguments()[0]);

			var list = properties.ToList();

			return null;
		}
	}
}
