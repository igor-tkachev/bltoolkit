using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Emit;
using BLToolkit.TypeBuilder;
using Castle.DynamicProxy;

namespace BLToolkit.Mapping
{
    public enum MappingOrder
    {
        ByColumnName,
        ByColumnIndex,
    }

    public class FullMappingSchema : MappingSchema
    {
        #region Private members

        private static readonly object SetterHandlersLock = new object();

        private static readonly Dictionary<Type, Dictionary<string, SetHandler>> SettersHandlers =
            new Dictionary<Type, Dictionary<string, SetHandler>>();

        private static readonly Dictionary<Type, Dictionary<string, GetHandler>> GettersHandlers =
            new Dictionary<Type, Dictionary<string, GetHandler>>();

        private readonly ProxyGenerator _proxy = new ProxyGenerator();

        private readonly Dictionary<Type, FullObjectMapper> _mappers = new Dictionary<Type, FullObjectMapper>();

        private readonly bool _ignoreLazyLoad;
        private readonly MappingOrder _mappingOrder;

        #endregion

        public FullMappingSchema(bool ignoreLazyLoad = false, MappingOrder mappingOrder = MappingOrder.ByColumnIndex)
        {
            _ignoreLazyLoad = ignoreLazyLoad;
            _mappingOrder = mappingOrder;
        }

        #region Overrides

        public override object MapDataReaderToObject(
            IDataReader dataReader,
            Type destObjectType,
            params object[] parameters)
        {
            // Get mapping for the type
            if (destObjectType == null) throw new ArgumentNullException("type");

            if (dataReader.FieldCount == 0)
                return null;

            int index = 0;
            FullObjectMapper mapper = GetObjectMapper(destObjectType, ref index);
            if (mapper.ColParent)
            {
                object result = FillObject(mapper, dataReader);
                while (dataReader.Read())
                {
                    result = FillObject(result, mapper, dataReader);
                }

                return result;
            }
            else
                return FillObject(mapper, dataReader);
        }


        public override IList<T> MapDataReaderToList<T>(
            IDataReader reader,
            IList<T> list,
            params object[] parameters)
        {
            FullObjectMapper mapper;
            if (_mappers.ContainsKey(typeof (T)))
            {
                mapper = _mappers[typeof (T)];
            }
            else
            {
                int index = 0;
                mapper = GetObjectMapper(typeof (T), ref index);
                _mappers[typeof (T)] = mapper;
            }

            while (reader.Read())
            {
                var result = FillObject<T>(mapper, reader);
                list.Add(result);
            }

            return list;
        }

        #endregion

        private T FillObject<T>(FullObjectMapper mapper, IDataReader datareader)
        {
            return (T)FillObject(mapper, datareader);
        }

        private object LoadLazy(IMapper mapper, object proxy, Type parentType)
        {
            var lazyMapper = (ILazyMapper)mapper;
            object key = lazyMapper.ParentKeyGetter(proxy);

            using (var db = new DbManager())
            {                
                var fullSqlQuery = new FullSqlQuery(db, true);
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
        }

        private object FillObject(object result, IObjectMapper mapper, IDataReader datareader)
        {
            foreach (IMapper map in mapper.PropertiesMapping)
            {
                if (map is IObjectMapper && (map as IObjectMapper).IsLazy)
                    continue;

                if (!datareader.IsDBNull(map.DataReaderIndex))
                {
                    ////IGNORE. TODO Add getter
                    //if (map is ValueMapper)
                    //{
                    //    //Type propType = (map as ValueMapper).PropertyType;
                    //    object value = datareader.GetValue(map.DataReaderIndex);
                    //    //if (value != null && value.GetType() != propType)
                    //    //{
                    //    //    value = ConvertChangeType(value, propType);    
                    //    //}                        
                    //    map.Setter(result, value);
                    //}

                    ////IGNORE. TODO Add getter
                    //if (map is FullObjectMapper)
                    //{
                    //    object fillObject = FillObject((FullObjectMapper)map, datareader);
                    //    map.Setter(result, fillObject);
                    //}
                    if (map is CollectionFullObjectMapper)
                    {
                        var collectionFullObjectMapper = (CollectionFullObjectMapper) map;
                        object listInstance = collectionFullObjectMapper.Getter(result);
                        if (listInstance == null)
                        {
                            listInstance =
                                Activator.CreateInstance((map as CollectionFullObjectMapper).PropertyCollectionType);
                            map.Setter(result, listInstance);
                        }
                        object fillObject = FillObject((CollectionFullObjectMapper) map, datareader);
                        ((IList) listInstance).Add(fillObject);
                    }
                }
            }

            return result;
        }

        private object FillObject(IObjectMapper mapper, IDataReader datareader)
        {
            object result = mapper.ContainsLazyChild
                                ? _proxy.CreateClassProxy(mapper.PropertyType, new LazyValueLoadInterceptor(mapper, LoadLazy))
                                : FunctionFactory.Remote.CreateInstance(mapper.PropertyType);

            foreach (IMapper map in mapper.PropertiesMapping)
            {
                if (map is IObjectMapper && (map as IObjectMapper).IsLazy)
                    continue;

                if (!(map.DataReaderIndex < datareader.FieldCount))
                    continue;

                if (!datareader.IsDBNull(map.DataReaderIndex))
                {
                    if (map is ValueMapper)
                    {
                        object value = _mappingOrder == MappingOrder.ByColumnIndex
                                           ? datareader.GetValue(map.DataReaderIndex)
                                           : datareader[((ValueMapper)map).ColumnName];

                        //Type propType = (map as ValueMapper).PropertyType;
                        //if (value != null && value.GetType() != propType)
                        //{
                        //    value = ConvertChangeType(value, propType);    
                        //}
                        try
                        {
                            map.Setter(result, value);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception(
                                string.Format("FillOject failed for field : {0} of class: {1}. Column name : {2} Db type is: {3} and value : {4}",
                                              map.PropertyName, mapper.PropertyType,
                                              ((ValueMapper)map).ColumnName,
                                              value == null ? "Null" : value.GetType().ToString(), value), exception);
                        }
                    }

                    if (map is FullObjectMapper)
                    {
                        object fillObject = FillObject((FullObjectMapper) map, datareader);
                        map.Setter(result, fillObject);
                    }

                    if (map is CollectionFullObjectMapper)
                    {
                        var collectionFullObjectMapper = (CollectionFullObjectMapper) map;

                        object listInstance = collectionFullObjectMapper.Getter(result);
                        if (listInstance == null)
                        {
                            listInstance =
                                Activator.CreateInstance((map as CollectionFullObjectMapper).PropertyCollectionType);
                            map.Setter(result, listInstance);
                        }
                        object fillObject = FillObject((CollectionFullObjectMapper) map, datareader);
                        ((IList) listInstance).Add(fillObject);
                    }
                }
            }

            return result;
        }

        public FullObjectMapper GetObjectMapper(Type mapperType, ref int startIndex)
        {
            var mapper = new FullObjectMapper {PropertyType = mapperType};
            return (FullObjectMapper) GetObjectMapper(mapper, ref startIndex);
        }

        public IMapper GetObjectMapper(IObjectMapper mapper, ref int startIndex)
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
                }
            }
            foreach (PropertyInfo prop in properties)
            {
                bool isLazy = false;
                if (!_ignoreLazyLoad)
                {
                    object[] lazy = prop.GetCustomAttributes(typeof (LazyInstanceAttribute), true);
                    if (lazy.Length > 0)
                    {
                        if (((LazyInstanceAttribute) lazy[0]).IsLazy)
                        {
                            isLazy = true;
                            mapper.ContainsLazyChild = true;

                            //  Getters
                            lock (SetterHandlersLock)
                                if (!GettersHandlers[mapperType].ContainsKey(primaryKeyPropInfo.Name))
                                {
                                    GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType,
                                                                                                primaryKeyPropInfo);
                                    GettersHandlers[mapperType].Add(primaryKeyPropInfo.Name, getHandler);
                                }
                        }
                    }
                }

                // Check if the accessor is an association
                object[] associationAttr = prop.GetCustomAttributes(typeof (AssociationAttribute), true);
                if (associationAttr.Length > 0)
                {
                    if (associationAttr.Length > 1)
                        throw new Exception("AssociationAttribute is used several times on the property " + prop.Name);
                    var ass = (AssociationAttribute) associationAttr[0];

                    //  Getters for IObjectMapper
                    lock (SetterHandlersLock)
                        if (!GettersHandlers[mapperType].ContainsKey(prop.Name))
                        {
                            GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType, prop);
                            GettersHandlers[mapperType].Add(prop.Name, getHandler);
                        }

                    bool isCollection = prop.PropertyType.GetInterfaces().ToList().Contains(typeof (IList));
                    IObjectMapper propertiesMapping;
                    if (!isCollection)
                    {
                        propertiesMapping = new FullObjectMapper
                                                {
                                                    PropertyType = prop.PropertyType,
                                                    IsNullable = ass.CanBeNull
                                                };
                    }
                    else
                    {
                        Type listElementType = GetGenericType(prop.PropertyType);
                        TableDescription colElementTableDescription = GetTableDescription(listElementType);

                        propertiesMapping = new CollectionFullObjectMapper
                                                {
                                                    PropertyType = listElementType,
                                                    Getter = GettersHandlers[mapperType][prop.Name],
                                                    TableName = colElementTableDescription.TableName,
                                                    PropertyCollectionType = prop.PropertyType,
                                                };

                        ((FullObjectMapper)mapper).ColParent = true;
                    }

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


                    object[] mapFields = prop.GetCustomAttributes(typeof (MapFieldAttribute), true);
                    if (mapFields.Length > 1)
                        throw new Exception("AssociationAttribute is used several times on the property " + prop.Name);


                    var map = new ValueMapper
                                  {
                                      PropertyName = prop.Name,
                                      PropertyType = prop.PropertyType,
                                      DataReaderIndex = startIndex,
                                      Setter = SettersHandlers[mapperType][prop.Name],
                                      TableName = tableDescription.TableName,
                                      /* Optimize with Provider.BuildTableName */
                                      ColumnName =
                                          mapFields.Length > 0 ? ((MapFieldAttribute) mapFields[0]).MapName : prop.Name
                                  };

                    mapper.PropertiesMapping.Add(map);

                    object[] pkFields = prop.GetCustomAttributes(typeof (PrimaryKeyAttribute), true);
                    if (pkFields.Length > 1)
                        throw new Exception("PrimaryKeyAttribute is used several times on the property " + prop.Name);

                    if (pkFields.Length == 1)
                        mapper.DataReaderIndex = startIndex;

                    startIndex++;
                }
            }

            foreach (IObjectMapper objMap in objectMappers)
            {
                IObjectMapper cel = mapper;
                while (cel != null)
                {
                    if (mapper.PropertyType == objMap.PropertyType)
                        continue;

                    cel = (IObjectMapper) cel.ParentMapping;
                }

                objMap.ParentMapping = mapper;
                mapper.PropertiesMapping.Add(GetObjectMapper(objMap, ref startIndex));
                //TODO startIndex++ ??? If we dont show the association column, dont increase the index
            }

            return mapper;
        }

        public static Type GetGenericType(Type t)
        {
            if (t.IsGenericType)
            {
                Type[] at = t.GetGenericArguments();
                return at.FirstOrDefault();
            }
            return null;
        }

        private TableDescription GetTableDescription(Type type)
        {
            var tableDescription = new TableDescription();
            object[] tableAtt = type.GetCustomAttributes(typeof (TableNameAttribute), true);

            if (tableAtt.Length > 0)
            {
                var tna = (TableNameAttribute) tableAtt[0];

                tableDescription.Database = tna.Database;
                tableDescription.Owner = tna.Owner;
                tableDescription.TableName = tna.Name;
            }

            return tableDescription;
        }
    }
}