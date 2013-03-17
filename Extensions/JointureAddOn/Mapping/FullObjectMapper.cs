#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Emit;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using Castle.DynamicProxy;

#endregion

namespace BLToolkit.Mapping
{
    public class FullObjectMapper : ObjectMapper, IObjectMapper
    {
        #region Fields

        private static readonly object SetterHandlersLock = new object();

        private static readonly Dictionary<Type, Dictionary<string, SetHandler>> SettersHandlers =
            new Dictionary<Type, Dictionary<string, SetHandler>>();

        private static readonly Dictionary<Type, Dictionary<string, GetHandler>> GettersHandlers =
            new Dictionary<Type, Dictionary<string, GetHandler>>();

        private readonly DbManager _db;
        private readonly bool _ignoreLazyLoad;
        private readonly ProxyGenerator _proxy;

        #endregion

        public FullObjectMapper(DbManager db, bool ignoreLazyLoading)
        {
            _db = db;
            _ignoreLazyLoad = ignoreLazyLoading;

            _proxy = new ProxyGenerator();
            PropertiesMapping = new List<IMapper>();
        }

        #region IPropertiesMapping Members

        public List<IMapper> PropertiesMapping { get; private set; }
        public IPropertiesMapping ParentMapping { get; set; }

        #endregion

        public bool IsNullable { get; set; }
        public bool ColParent { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }
        public AssociationAttribute Association { get; set; }

        #endregion

        #region IObjectMapper

        public bool IsLazy { get; set; }
        public bool ContainsLazyChild { get; set; }
        public GetHandler Getter { get; set; }

        #endregion

        #region ILazyMapper

        public GetHandler ParentKeyGetter { get; set; }
        public GetHandler PrimaryKeyValueGetter { get; set; }

        #endregion

        #region Overrides

        public override void Init(MappingSchema mappingSchema, Type type)
        {
            PropertyType = type;

            // TODO implement this method
            base.Init(mappingSchema, type);

            int startIndex = 0;
            GetObjectMapper(this, ref startIndex);
        }

        public override object CreateInstance()
        {
            object result = ContainsLazyChild
                                ? _proxy.CreateClassProxy(PropertyType, new LazyValueLoadInterceptor(this, LoadLazy))
                                : FunctionFactory.Remote.CreateInstance(PropertyType);

            return result;
        }

        public override object CreateInstance(InitContext context)
        {
            return CreateInstance();
        }

        #endregion

        #region Private methods

        private TableDescription GetTableDescription(Type type)
        {
            var tableDescription = new TableDescription();
            object[] tableAtt = type.GetCustomAttributes(typeof(TableNameAttribute), true);

            if (tableAtt.Length > 0)
            {
                var tna = (TableNameAttribute)tableAtt[0];

                tableDescription.Database = tna.Database;
                tableDescription.Owner = tna.Owner;
                tableDescription.TableName = tna.Name;
            }

            return tableDescription;
        }

        private IMapper GetObjectMapper(IObjectMapper mapper, ref int startIndex)
        {
            Type mapperType = mapper.PropertyType;
            var objectMappers = new List<IObjectMapper>();

            TableDescription tableDescription = GetTableDescription(mapperType);

            lock (SetterHandlersLock)
            {
                if (!SettersHandlers.ContainsKey(mapperType))
                    SettersHandlers.Add(mapperType, new Dictionary<string, SetHandler>());

                if (!GettersHandlers.ContainsKey(mapperType))
                    GettersHandlers.Add(mapperType, new Dictionary<string, GetHandler>());
            }

            PropertyInfo[] properties = mapperType.GetProperties();

            PropertyInfo primaryKeyPropInfo = null;
            foreach (PropertyInfo prop in properties)
            {
                //  Setters
                lock (SetterHandlersLock)
                {
                    if (!SettersHandlers[mapper.PropertyType].ContainsKey(prop.Name))
                    {
                        SetHandler setHandler = FunctionFactory.Il.CreateSetHandler(mapper.PropertyType, prop);
                        SettersHandlers[mapper.PropertyType].Add(prop.Name, setHandler /* IL.Setter*/);
                    }
                }

                object[] pkFields = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                if (pkFields.Length > 0)
                {
                    primaryKeyPropInfo = prop;

                    lock (SetterHandlersLock)
                    {
                        if (!GettersHandlers[mapperType].ContainsKey(prop.Name))
                        {
                            GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType, prop);
                            GettersHandlers[mapperType].Add(prop.Name, getHandler);
                        }
                    }
                    mapper.PrimaryKeyValueGetter = GettersHandlers[mapperType][prop.Name];

                    if (mapper.Association != null && string.IsNullOrWhiteSpace(mapper.Association.OtherKey))
                    {
                        mapper.Association.OtherKey = prop.Name;
                    }
                }
            }
            if (primaryKeyPropInfo == null)
                throw new Exception("PrimaryKey attribute not found on type: " + mapperType);

            foreach (PropertyInfo prop in properties)
            {
                // Check if the accessor is an association
                object[] associationAttr = prop.GetCustomAttributes(typeof(AssociationAttribute), true);
                if (associationAttr.Length > 0)
                {
                    var ass = (AssociationAttribute)associationAttr[0];

                    //  Getters for IObjectMapper
                    lock (SetterHandlersLock)
                        if (!GettersHandlers[mapperType].ContainsKey(prop.Name))
                        {
                            GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType, prop);
                            GettersHandlers[mapperType].Add(prop.Name, getHandler);
                        }

                    bool isCollection = prop.PropertyType.GetInterfaces().ToList().Contains(typeof(IList));
                    IObjectMapper propertiesMapping;
                    if (!isCollection)
                    {
                        propertiesMapping = new FullObjectMapper(_db, _ignoreLazyLoad)
                        {
                            PropertyType = prop.PropertyType,
                            IsNullable = ass.CanBeNull,
                            Getter = GettersHandlers[mapperType][prop.Name],
                        };
                    }
                    else
                    {
                        Type listElementType = FullMappingSchema.GetGenericType(prop.PropertyType);
                        TableDescription colElementTableDescription = GetTableDescription(listElementType);

                        propertiesMapping = new CollectionFullObjectMapper(_db)
                        {
                            PropertyType = listElementType,
                            Getter = GettersHandlers[mapperType][prop.Name],
                            TableName = colElementTableDescription.TableName,
                            PropertyCollectionType = prop.PropertyType,
                        };

                        ((FullObjectMapper)mapper).ColParent = true;
                    }

                    if (string.IsNullOrWhiteSpace(ass.ThisKey))
                        ass.ThisKey = primaryKeyPropInfo.Name;

                    bool isLazy = false;
                    if (!_ignoreLazyLoad)
                    {
                        object[] lazy = prop.GetCustomAttributes(typeof(LazyInstanceAttribute), true);
                        if (lazy.Length > 0)
                        {
                            if (((LazyInstanceAttribute)lazy[0]).IsLazy)
                            {
                                isLazy = true;
                                mapper.ContainsLazyChild = true;

                                //  Getters
                                lock (SetterHandlersLock)
                                    if (!GettersHandlers[mapperType].ContainsKey(primaryKeyPropInfo.Name))
                                    {
                                        GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType, primaryKeyPropInfo);
                                        GettersHandlers[mapperType].Add(primaryKeyPropInfo.Name, getHandler);
                                    }
                            }
                        }
                    }

                    propertiesMapping.Association = ass;
                    propertiesMapping.PropertyName = prop.Name;
                    propertiesMapping.IsLazy = isLazy;
                    propertiesMapping.Setter = SettersHandlers[mapperType][prop.Name];

                    if (propertiesMapping.IsLazy)
                    {
                        propertiesMapping.ParentKeyGetter = GettersHandlers[mapperType][primaryKeyPropInfo.Name];
                    }
                    objectMappers.Add(propertiesMapping);
                }
                else
                {
                    object[] nomapAttr = prop.GetCustomAttributes(typeof(MapIgnoreAttribute), true);
                    if (nomapAttr.Length > 0)
                        continue;

                    object[] mapFields = prop.GetCustomAttributes(typeof(MapFieldAttribute), true);
                    if (mapFields.Length > 1)
                        throw new Exception("AssociationAttribute is used several times on the property " + prop.Name);

                    string columnName = mapFields.Length > 0 ? ((MapFieldAttribute)mapFields[0]).MapName : prop.Name;

                    var map = new ValueMapper
                    {
                        PropertyName = prop.Name,
                        PropertyType = prop.PropertyType,
                        DataReaderIndex = startIndex,
                        Setter = SettersHandlers[mapperType][prop.Name],
                        TableName = tableDescription.TableName,
                        ColumnName = columnName,
                    };

                    var mapColumnName = map.GetColumnName(columnName);
                    map.ColumnAlias = columnName == mapColumnName ? null : mapColumnName;

                    mapper.PropertiesMapping.Add(map);

                    object[] pkFields = prop.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                    if (pkFields.Length > 1)
                        throw new Exception("PrimaryKeyAttribute is used several times on the property " + prop.Name);

                    if (pkFields.Length == 1)
                        mapper.DataReaderIndex = startIndex;

                    startIndex++;
                }
            }

            foreach (IObjectMapper objMap in objectMappers)
            {
                #region Check mapping recursion

                IObjectMapper cel = mapper;
                while (cel != null)
                {
                    if (mapper.PropertyType == objMap.PropertyType)
                        continue;

                    cel = (IObjectMapper)cel.ParentMapping;
                }

                #endregion

                objMap.ParentMapping = mapper;
                mapper.PropertiesMapping.Add(GetObjectMapper(objMap, ref startIndex));
            }

            return mapper;
        }

        private object LoadLazy(IMapper mapper, object proxy, Type parentType)
        {
            var lazyMapper = (ILazyMapper) mapper;
            object key = lazyMapper.ParentKeyGetter(proxy);

            var fullSqlQuery = new FullSqlQuery(_db, ignoreLazyLoad: true);
            object parentLoadFull = fullSqlQuery.SelectByKey(parentType, key);
            if (parentLoadFull == null)
            {
                object value = Activator.CreateInstance(mapper is CollectionFullObjectMapper
                                                            ? (mapper as CollectionFullObjectMapper).PropertyCollectionType
                                                            : mapper.PropertyType);
                return value;
            }

            var objectMapper = (IObjectMapper) mapper;
            return objectMapper.Getter(parentLoadFull);
        }

        #endregion
    }
}