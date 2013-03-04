#region

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

#endregion

namespace BLToolkit.Mapping
{
    public class FullMappingSchema : MappingSchema
    {
        #region Private members

        private static readonly object SetterHandlersLock = new object();

        private static readonly Dictionary<Type, Dictionary<string, SetHandler>> SettersHandlers =
            new Dictionary<Type, Dictionary<string, SetHandler>>();

        private static readonly Dictionary<Type, Dictionary<string, GetHandler>> GettersHandlers =
            new Dictionary<Type, Dictionary<string, GetHandler>>();

        private readonly Dictionary<string, int> _columnOccurences = new Dictionary<string, int>();
        private readonly Dictionary<string, List<string>> _columnVariations = new Dictionary<string, List<string>>();

        private readonly DbManager _db;
        private readonly bool _ignoreLazyLoad;
        private readonly Dictionary<Type, FullObjectMapper> _mappers = new Dictionary<Type, FullObjectMapper>();
        private readonly MappingOrder _mappingOrder;
        private readonly ProxyGenerator _proxy = new ProxyGenerator();
        private DataTable _schema;
        private List<string> _schemaColumns;

        #endregion

        public FullMappingSchema(DbManager db, MappingSchema inheritedMappingSchema = null, bool ignoreLazyLoad = false, MappingOrder mappingOrder = MappingOrder.ByColumnIndex, bool ignoreMissingColumns = false)
        {
            _db = db;
            _ignoreLazyLoad = ignoreLazyLoad;
            _mappingOrder = mappingOrder;

            IgnoreMissingColumns = ignoreMissingColumns;
            InheritedMappingSchema = inheritedMappingSchema;
        }

        #region Overrides

        public override object MapDataReaderToObject(
            IDataReader dataReader,
            Type destObjectType,
            params object[] parameters)
        {
            // Get mapping for the type
            if (destObjectType == null) throw new ArgumentNullException("destObjectType");

            if (dataReader.FieldCount == 0)
                return null;

            int index = 0;
            FullObjectMapper mapper = GetObjectMapper(destObjectType, ref index);
            InitSchema(dataReader);

            if (mapper.ColParent)
            {
                object result = FillObject(mapper, dataReader);
                while (dataReader.Read())
                {
                    result = FillObject(result, mapper, dataReader);
                }

                return result;
            }
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

            InitSchema(reader);

            T currentItem = default(T);
            while (reader.Read())
            {
                var result = FillObject<T>(mapper, reader);
                if (currentItem == null)
                {
                    currentItem = result;
                    list.Add(result);
                    continue;
                }

                object resultPk = mapper.PrimaryKeyValueGetter.Invoke(result);
                object currentItemPk = mapper.PrimaryKeyValueGetter.Invoke(currentItem);

                if (!resultPk.Equals(currentItemPk))
                {
                    currentItem = result;
                    list.Add(result);
                    continue;
                }

                if (mapper.ColParent)
                {
                    FillObject(currentItem, mapper, reader);
                }
            }

            return list;
        }

        #endregion

        public bool IgnoreMissingColumns { get; set; }
        public MappingSchema InheritedMappingSchema { get; set; }

        private void InitSchema(IDataReader reader)
        {
            _schemaColumns = new List<string>();
            _schema = reader.GetSchemaTable();
            _schema.Rows.Cast<DataRow>().ToList().ForEach(dr => _schemaColumns.Add((string) dr["ColumnName"]));
        }

        private T FillObject<T>(FullObjectMapper mapper, IDataReader datareader)
        {
            return (T) FillObject(mapper, datareader);
        }

        private object LoadLazy(IMapper mapper, object proxy, Type parentType)
        {
            var lazyMapper = (ILazyMapper) mapper;
            object key = lazyMapper.ParentKeyGetter(proxy);

            var fullSqlQuery = new FullSqlQuery(_db, true);
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

        private object FillObject(object result, IObjectMapper mapper, IDataReader datareader)
        {
            foreach (IMapper map in mapper.PropertiesMapping)
            {
                if (map is IObjectMapper && (map as IObjectMapper).IsLazy)
                    continue;

                if (_mappingOrder == MappingOrder.ByColumnIndex && (datareader.IsDBNull(map.DataReaderIndex)))
                    continue;

                if (map is CollectionFullObjectMapper)
                {
                    var collectionFullObjectMapper = (CollectionFullObjectMapper) map;
                    object listInstance = collectionFullObjectMapper.Getter(result);
                    if (listInstance == null)
                    {
                        listInstance = Activator.CreateInstance((map as CollectionFullObjectMapper).PropertyCollectionType);
                        map.Setter(result, listInstance);
                    }
                    var list = (IList) listInstance;
                    object fillObject = FillObject((CollectionFullObjectMapper) map, datareader);
                    if (list.Count > 0)
                    {
                        object lastElement = list[list.Count - 1];
                        object lastPk = mapper.PrimaryKeyValueGetter.Invoke(lastElement);
                        object currentPk = mapper.PrimaryKeyValueGetter.Invoke(fillObject);
                        if (lastPk.Equals(currentPk))
                            continue;
                    }

                    ((IList) listInstance).Add(fillObject);
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

                if (_mappingOrder == MappingOrder.ByColumnIndex)
                {
                    if (!(map.DataReaderIndex < datareader.FieldCount))
                        continue;
                }

                if ((_mappingOrder == MappingOrder.ByColumnIndex) && datareader.IsDBNull(map.DataReaderIndex))
                    continue;

                if (_mappingOrder == MappingOrder.ByColumnName && map is ValueMapper)
                {
                    string colName = ((ValueMapper) map).ColumnName;
                    int index = -1;
                    if (!_schemaColumns.Contains(colName))
                    {
                        bool found = false;
                        int order = 1;
                        foreach (string key in _columnVariations.Keys)
                        {
                            List<string> variations = _columnVariations[key];
                            if (variations.Contains(colName))
                            {
                                if (colName.Contains(key + "_"))
                                {
                                    string orderString = colName.Replace(key + "_", "");
                                    order = int.Parse(orderString) + 1;
                                    colName = key;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (found)
                        {
                            int i = 0, occurenceCnt = 0;
                            foreach (string column in _schemaColumns)
                            {
                                if (column == colName)
                                {
                                    occurenceCnt++;
                                    if (occurenceCnt == order)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                i++;
                            }
                        }
                        else
                        {
                            if (!IgnoreMissingColumns)
                            {
                                throw new Exception(string.Format("Couldnt find db column {0} in the query result", colName));
                            }
                            continue;
                        }
                    }
                    else
                        index = _schemaColumns.IndexOf(colName);

                    if (datareader.IsDBNull(index))
                        continue;

                    //value = datareader[colName];
                    map.DataReaderIndex = index;
                }


                if (map is ValueMapper)
                {
                    object value = datareader.GetValue(map.DataReaderIndex);

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
                            string.Format("FillOject failed for field : {0} of class: {1}.\nColumn name : {2} Db type is: {3} and value : {4}",
                                          map.PropertyName, mapper.PropertyType,
                                          ((ValueMapper) map).ColumnName,
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
                        listInstance = Activator.CreateInstance((map as CollectionFullObjectMapper).PropertyCollectionType);
                        map.Setter(result, listInstance);
                    }
                    object fillObject = FillObject((CollectionFullObjectMapper) map, datareader);
                    ((IList) listInstance).Add(fillObject);
                }
            }

            return result;
        }

        public FullObjectMapper GetObjectMapper(Type mapperType, ref int startIndex)
        {
            var mapper = new FullObjectMapper {PropertyType = mapperType};
            return (FullObjectMapper) GetObjectMapper(mapper, ref startIndex);
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

                object[] pkFields = prop.GetCustomAttributes(typeof (PrimaryKeyAttribute), true);
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
                                    GetHandler getHandler = FunctionFactory.Il.CreateGetHandler(mapperType, primaryKeyPropInfo);
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

                        ((FullObjectMapper) mapper).ColParent = true;
                    }

                    if (string.IsNullOrWhiteSpace(ass.ThisKey))
                        ass.ThisKey = primaryKeyPropInfo.Name;

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
                    object[] nomapAttr = prop.GetCustomAttributes(typeof (MapIgnoreAttribute), true);
                    if (nomapAttr.Length > 0)
                        continue;

                    object[] mapFields = prop.GetCustomAttributes(typeof (MapFieldAttribute), true);
                    if (mapFields.Length > 1)
                        throw new Exception("AssociationAttribute is used several times on the property " + prop.Name);

                    string columnName = mapFields.Length > 0 ? ((MapFieldAttribute) mapFields[0]).MapName : prop.Name;

                    var mapColumnName = GetColumnName(columnName);

                    var map = new ValueMapper
                        {
                            PropertyName = prop.Name,
                            PropertyType = prop.PropertyType,
                            DataReaderIndex = startIndex,
                            Setter = SettersHandlers[mapperType][prop.Name],
                            TableName = tableDescription.TableName,
                            ColumnName = columnName,
                            ColumnAlias = columnName == mapColumnName ? null : mapColumnName,
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
            }

            return mapper;
        }

        private string GetColumnName(string columnName)
        {
            int occurenceCount;
            if (_columnOccurences.ContainsKey(columnName))
            {
                occurenceCount = _columnOccurences[columnName] + 1;
                _columnOccurences[columnName] = occurenceCount;
            }
            else
            {
                _columnOccurences[columnName] = 1;
                occurenceCount = 1;
            }

            string res = columnName + (occurenceCount > 1 ? string.Format("_{0}", occurenceCount - 1) : "");

            var variations = new List<string>();
            if (_columnVariations.ContainsKey(columnName))
            {
                variations = _columnVariations[columnName];
            }

            variations.Add(res);
            _columnVariations[columnName] = variations;

            return res;
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