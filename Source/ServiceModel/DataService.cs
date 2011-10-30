using System;
using System.Collections.Generic;
using System.Data.Services.Providers;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;

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
			_metadata      = new MetadataProvider(_mappingSchema, typeof(T));
			_query         = new QueryProvider(_metadata.TypeDic);
		}

		readonly MappingSchema    _mappingSchema;
		readonly MetadataProvider _metadata;
		readonly QueryProvider    _query;

		public object GetService(Type serviceType)
		{
			if (serviceType == typeof(IDataServiceMetadataProvider)) return _metadata;
			if (serviceType == typeof(IDataServiceQueryProvider))    return _query;

			return null;
		}

		#region MetadataProvider

		class TypeInfo
		{
			public ResourceType Type;
			public SqlTable     Table;
			public ObjectMapper Mapper;
		}

		class MetadataProvider : IDataServiceMetadataProvider
		{
			public MetadataProvider(MappingSchema mappingSchema, Type dataSourceType)
			{
				_mappingSchema  = mappingSchema;
				_dataSourceType = dataSourceType;

				LoadMetadata();
			}

			readonly MappingSchema                    _mappingSchema;
			readonly Type                             _dataSourceType;
			readonly Dictionary<string,ResourceType>  _types  = new Dictionary<string,ResourceType>();
			readonly Dictionary<string,ResourceSet>   _sets   = new Dictionary<string,ResourceSet>();

			public readonly Dictionary<Type,TypeInfo> TypeDic = new Dictionary<Type,TypeInfo>();

			void LoadMetadata()
			{
				var n = 0;
				var list =
				(
					from p in _dataSourceType.GetProperties()
					let t   = p.PropertyType
					where t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Table<>)
					let tt  = t.GetGenericArguments()[0]
					let tbl = new SqlTable(_mappingSchema, tt)
					where tbl.Fields.Values.Any(f => f.IsPrimaryKey)
					let m   = _mappingSchema.GetObjectMapper(tt)
					select new
					{
						p.Name,
						ID     = n++,
						Type   = tt,
						Table  = tbl,
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
						if (!TypeDic.ContainsKey(m.Type))
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

				if (!TypeDic.TryGetValue(type, out typeInfo))
				{
					var baseInfo = baseType != null ? TypeDic[baseType] : null;

					typeInfo = new TypeInfo
					{
						Type   = new ResourceType(
							type,
							ResourceTypeKind.EntityType,
							baseInfo != null ? baseInfo.Type : null,
							type.Namespace,
							type.Name,
							type.IsAbstract),
						Table  = table,
						Mapper = mapper,
					};

					foreach (var field in table.Fields.Values)
					{
						if (baseType != null && baseInfo.Table.Fields.ContainsKey(field.Name))
							continue;

						var kind  = ResourcePropertyKind.Primitive;
						var ptype = ResourceType.GetPrimitiveResourceType(field.SystemType);

						if (baseType == null && field.IsPrimaryKey)
							kind |= ResourcePropertyKind.Key;

						var p = new ResourceProperty(field.Name, kind, ptype);

						typeInfo.Type.AddProperty(p);
					}

					typeInfo.Type.SetReadOnly();

					_types.  Add(typeInfo.Type.FullName, typeInfo.Type);
					TypeDic.Add(type, typeInfo);
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
				return TypeDic[resourceType.InstanceType].Mapper.InheritanceMapping.Select(m => TypeDic[m.Type].Type);
			}

			public bool HasDerivedTypes(ResourceType resourceType)
			{
				return TypeDic[resourceType.InstanceType].Mapper.InheritanceMapping.Count > 0;
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

		class QueryProvider : IDataServiceQueryProvider
		{
			public QueryProvider(Dictionary<Type,TypeInfo> typeDic)
			{
				_typeDic = typeDic;
			}

			readonly Dictionary<Type,TypeInfo>                  _typeDic;
			readonly Dictionary<string,Func<object,IQueryable>> _sets = new Dictionary<string,Func<object,IQueryable>>();

			public IQueryable GetQueryRootForResourceSet(ResourceSet resourceSet)
			{
				Func<object,IQueryable> func;

				lock (_sets)
				{
					if (!_sets.TryGetValue(resourceSet.Name, out func))
					{
						var p = Expression.Parameter(typeof(object), "p");
						var l = Expression.Lambda<Func<object,IQueryable>>(
							Expression.PropertyOrField(
								Expression.Convert(p, typeof(T)),
								resourceSet.Name),
							p);

						func = l.Compile();

						_sets.Add(resourceSet.Name, func);
					}
				}

				return func(CurrentDataSource);
			}

			public ResourceType GetResourceType(object target)
			{
				return _typeDic[target.GetType()].Type;
			}

			public object GetPropertyValue(object target, ResourceProperty resourceProperty)
			{
				throw new NotImplementedException();
			}

			public object GetOpenPropertyValue(object target, string propertyName)
			{
				throw new NotImplementedException();
			}

			public IEnumerable<KeyValuePair<string,object>> GetOpenPropertyValues(object target)
			{
				throw new NotImplementedException();
			}

			public object InvokeServiceOperation(ServiceOperation serviceOperation, object[] parameters)
			{
				throw new NotImplementedException();
			}

			public object CurrentDataSource         { get; set; }
			public bool   IsNullPropagationRequired { get { return true; } }
		}

		#endregion
	}
}
