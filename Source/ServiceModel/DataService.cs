using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using BLToolkit.Data.Sql;

namespace BLToolkit.ServiceModel
{
	using Data.Linq;
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
			_metadata      = new MetadataProvider(_mappingSchema, typeof(T));
			_query         = new QueryProvider();
		}

		readonly MappingSchema                _mappingSchema;
		readonly IDataServiceMetadataProvider _metadata;
		readonly IDataServiceQueryProvider    _query;

		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IDataServiceMetadataProvider)) return _metadata;
			if (serviceType == typeof(IDataServiceQueryProvider))    return _query;

			return null;
		}

		#region MetadataProvider

		class MetadataProvider : IDataServiceMetadataProvider
		{
			public MetadataProvider(MappingSchema mappingSchema, Type dataSourceType)
			{
				_mappingSchema  = mappingSchema;
				_dataSourceType = dataSourceType;

				LoadMetadata();
			}

			class TypeInfo
			{
				public ResourceType Type;
				public SqlTable     Table;
				public ObjectMapper Mapper;
			}

			readonly MappingSchema                   _mappingSchema;
			readonly Type                            _dataSourceType;
			readonly Dictionary<string,ResourceType> _types   = new Dictionary<string,ResourceType>();
			readonly Dictionary<string,ResourceSet>  _sets    = new Dictionary<string,ResourceSet>();
			readonly Dictionary<Type,TypeInfo>       _typeDic = new Dictionary<Type,TypeInfo>();

			void LoadMetadata()
			{
				var n = 0;
				var list =
				(
					from p in _dataSourceType.GetProperties()
					let t  = p.PropertyType
					where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Table<>)
					let tt = t.GetGenericArguments()[0]
					let m  = _mappingSchema.GetObjectMapper(tt)
					select new
					{
						p.Name,
						ID     = n++,
						Type   = tt,
						Table  = new SqlTable(_mappingSchema, tt),
						Mapper = m
					}
				).ToList();

				var baseTypes = new Dictionary<Type,Type>();

				foreach (var item in list)
					foreach (var m in item.Mapper.InheritanceMapping)
						if (!baseTypes.ContainsKey(m.Type))
							baseTypes.Add(m.Type, item.Type);

				list.Sort((x,y) =>
				{
					Type baseType;

					if (baseTypes.TryGetValue(x.Type, out baseType))
						if (y.Type == baseType)
							return 1;

					if (baseTypes.TryGetValue(y.Type, out baseType))
						if (x.Type == baseType)
							return -1;

					return x.ID - y.ID;
				});

				foreach (var item in list)
				{
					Type baseType;
					baseTypes.TryGetValue(item.Type, out baseType);

					var type = GetTypeInfo(item.Type, baseType, item.Table, item.Mapper);
					var set  = new ResourceSet(item.Name, type.Type);

					set.SetReadOnly();

					_sets.Add(set.Name, set);
				}

				foreach (var item in list)
				{
					foreach (var m in item.Mapper.InheritanceMapping)
					{
						if (!_typeDic.ContainsKey(m.Type))
						{
							GetTypeInfo(
								m.Type,
								item.Type,
								new SqlTable(_mappingSchema, item.Type),
								_mappingSchema.GetObjectMapper(item.Type));
						}
					}
				}
			}

			TypeInfo GetTypeInfo(Type type, Type baseType, SqlTable table, ObjectMapper mapper)
			{
				TypeInfo typeInfo;

				if (!_typeDic.TryGetValue(type, out typeInfo))
				{
					var baseResourceType = baseType != null ? _typeDic[baseType].Type : null;

					typeInfo = new TypeInfo
					{
						Type   = new ResourceType(
							type,
							ResourceTypeKind.EntityType,
							baseResourceType,
							type.Namespace,
							type.Name,
							type.IsAbstract),
						Table  = table,
						Mapper = mapper,
					};

					foreach (var field in table.Fields.Values)
					{
						var kind  = ResourcePropertyKind.Primitive;
						var ptype = ResourceType.GetPrimitiveResourceType(field.SystemType);

						if (field.IsPrimaryKey)
							kind |= ResourcePropertyKind.Key;

						var p = new ResourceProperty(field.Name, kind, ptype);

						typeInfo.Type.AddProperty(p);
					}

					typeInfo.Type.SetReadOnly();

					_types.  Add(typeInfo.Type.FullName, typeInfo.Type);
					_typeDic.Add(type, typeInfo);
				}

				return typeInfo;
			}

			public bool TryResolveResourceSet(string name, out ResourceSet resourceSet)
			{
				return _sets.TryGetValue(name, out resourceSet);
			}

			public ResourceAssociationSet GetResourceAssociationSet(ResourceSet resourceSet, ResourceType resourceType, ResourceProperty resourceProperty)
			{
				throw new NotImplementedException();
			}

			public bool TryResolveResourceType(string name, out ResourceType resourceType)
			{
				return _types.TryGetValue(name, out resourceType);
			}

			public IEnumerable<ResourceType> GetDerivedTypes(ResourceType resourceType)
			{
				return _typeDic[resourceType.InstanceType].Mapper.InheritanceMapping.Select(m => _typeDic[m.Type].Type);
			}

			public bool HasDerivedTypes(ResourceType resourceType)
			{
				return _typeDic[resourceType.InstanceType].Mapper.InheritanceMapping.Count > 0;
			}

			public bool TryResolveServiceOperation(string name, out ServiceOperation serviceOperation)
			{
				serviceOperation = null;
				return false;
			}

			public string                        ContainerNamespace { get { return _dataSourceType.Namespace; } }
			public string                        ContainerName      { get { return _dataSourceType.Name;      } }
			public IEnumerable<ResourceSet>      ResourceSets       { get { return _sets.Values;              } }
			public IEnumerable<ResourceType>     Types              { get { return _types.Values;             } }
			public IEnumerable<ServiceOperation> ServiceOperations  { get { yield break;                      } }
		}

		#endregion

		#region QueryProvider

		public class QueryProvider : IDataServiceQueryProvider
		{
			public IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
			{
				throw new NotImplementedException();
			}

			public ResourceType GetResourceType(object target)
			{
				throw new NotImplementedException();
			}

			public object GetPropertyValue(object target, ResourceProperty resourceProperty)
			{
				throw new NotImplementedException();
			}

			public object GetOpenPropertyValue(object target, string propertyName)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<KeyValuePair<string, object>> GetOpenPropertyValues(object target)
			{
				throw new NotImplementedException();
			}

			public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
			{
				throw new NotImplementedException();
			}

			public object CurrentDataSource { get; set; }

			public bool IsNullPropagationRequired
			{
				get { throw new NotImplementedException(); }
			}
		}

		#endregion
	}
}
