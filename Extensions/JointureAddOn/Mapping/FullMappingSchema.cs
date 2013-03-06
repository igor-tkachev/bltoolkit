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
        #region Fields

        private static readonly object SetterHandlersLock = new object();

        private static readonly Dictionary<Type, Dictionary<string, SetHandler>> SettersHandlers =
            new Dictionary<Type, Dictionary<string, SetHandler>>();

        private static readonly Dictionary<Type, Dictionary<string, GetHandler>> GettersHandlers =
            new Dictionary<Type, Dictionary<string, GetHandler>>();

        private readonly Dictionary<string, int> _columnOccurences = new Dictionary<string, int>();
        private readonly Dictionary<string, List<string>> _columnVariations = new Dictionary<string, List<string>>();

        private readonly DbManager _db;
        private readonly bool _ignoreLazyLoad;
        private readonly MappingOrder _mappingOrder;
        private DataTable _schema;
        private List<string> _schemaColumns;

        #endregion

        public FullMappingSchema(DbManager db, bool ignoreLazyLoad = false,
            MappingOrder mappingOrder = MappingOrder.ByColumnIndex, bool ignoreMissingColumns = false)
        {
            _db = db;
            _ignoreLazyLoad = ignoreLazyLoad;
            _mappingOrder = mappingOrder;

            // TODO Remove this option
            _ignoreMissingColumns = ignoreMissingColumns;
        }

        #region Overrides

        protected override ObjectMapper CreateObjectMapperInstance(Type type)
        {
            int startIndex = 0;

            var mapper = new FullObjectMapper(_db) { PropertyType = type };
            var res = (ObjectMapper)GetObjectMapper(mapper, ref startIndex);

            return res;
        }

        protected override void MapInternal(Reflection.InitContext initContext, IMapDataSource source, object sourceObject, IMapDataDestination dest, object destObject, params object[] parameters)
        {
            FullObjectMapper mapper = (FullObjectMapper)initContext.ObjectMapper;
            IDataReader dataReader = (IDataReader)sourceObject;

            //int[] index = GetIndex(source, dest);
            //IValueMapper[] mappers = GetValueMappers(source, dest, index);

            //foreach (var valueMapper in mappers)
            //{
                
            //}

            InitSchema(dataReader);

            if (mapper.ColParent)
            {
                FillObject(mapper, dataReader, destObject);
                while (dataReader.Read())
                {
                    destObject = FillObject(destObject, mapper, dataReader);
                }
            }
            else
                 FillObject(mapper, dataReader, destObject);
        }

        public override IList MapDataReaderToList(
            IDataReader reader,
            IList list,
            Type destObjectType,
            params object[] parameters)
        {
            FullObjectMapper mapper = (FullObjectMapper) GetObjectMapper(destObjectType);

            InitSchema(reader);

            object currentItem = null;
            while (reader.Read())
            {
                var result = mapper.CreateInstance();

                FillObject(mapper, reader, result);
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

        private readonly bool _ignoreMissingColumns;

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
                    object fillObject = ((CollectionFullObjectMapper)map).CreateInstance();
                    FillObject((CollectionFullObjectMapper) map, datareader, fillObject);

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

        private void FillObject(IObjectMapper mapper, IDataReader datareader, object result)
        {
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
                    if (SetDataReaderIndex(map)) continue;
                }

                if (datareader.IsDBNull(map.DataReaderIndex))
                    continue;

                if (map is ValueMapper)
                {
                    object value = datareader.GetValue(map.DataReaderIndex);

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
                    object fillObject = ((FullObjectMapper) map).CreateInstance();
                    FillObject((FullObjectMapper) map, datareader, fillObject);
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

                    object fillObject = ((CollectionFullObjectMapper)map).CreateInstance();
                    FillObject((CollectionFullObjectMapper) map, datareader, fillObject);
                    ((IList) listInstance).Add(fillObject);
                }
            }
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
                // Check if the accessor is an association
                object[] associationAttr = prop.GetCustomAttributes(typeof (AssociationAttribute), true);
                if (associationAttr.Length > 0)
                {
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
                        propertiesMapping = new FullObjectMapper(_db)
                            {
                                PropertyType = prop.PropertyType,
                                IsNullable = ass.CanBeNull,
                                Getter = GettersHandlers[mapperType][prop.Name],
                            };
                    }
                    else
                    {
                        Type listElementType = GetGenericType(prop.PropertyType);
                        TableDescription colElementTableDescription = GetTableDescription(listElementType);

                        propertiesMapping = new CollectionFullObjectMapper(_db)
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
                #region Check mapping recursion

                IObjectMapper cel = mapper;
                while (cel != null)
                {
                    if (mapper.PropertyType == objMap.PropertyType)
                        continue;

                    cel = (IObjectMapper) cel.ParentMapping;
                }

                #endregion

                objMap.ParentMapping = mapper;
                mapper.PropertiesMapping.Add(GetObjectMapper(objMap, ref startIndex));
            }

            return mapper;
        }

        #region Private methods

        private void InitSchema(IDataReader reader)
        {
            _schemaColumns = new List<string>();
            _schema = reader.GetSchemaTable();
            if (_schema != null)
                _schema.Rows.Cast<DataRow>().ToList().ForEach(dr => _schemaColumns.Add((string)dr["ColumnName"]));
        }

        private bool SetDataReaderIndex(IMapper map)
        {
            string colName = ((ValueMapper)map).ColumnName;
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
                    if (!_ignoreMissingColumns)
                    {
                        throw new Exception(string.Format("Couldnt find db column {0} in the query result", colName));
                    }
                    return true;
                }
            }
            else
                index = _schemaColumns.IndexOf(colName);

            map.DataReaderIndex = index;
            return false;
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

        #endregion
    }
}