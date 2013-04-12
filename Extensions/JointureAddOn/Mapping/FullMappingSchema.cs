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
using BLToolkit.Reflection.Extension;
using BLToolkit.TypeBuilder;
using Castle.DynamicProxy;

#endregion

namespace BLToolkit.Mapping
{
    public class FullMappingSchema : MappingSchema
    {
        #region Fields

        private readonly DbManager _db;
        private readonly bool _ignoreLazyLoad;
        private readonly MappingOrder _mappingOrder;
        private DataTable _schema;
        private List<string> _schemaColumns;
        private readonly MappingSchema _parentMappingSchema;

        private ExtensionList _extensions;

        #endregion

        public FullMappingSchema(DbManager db, bool ignoreLazyLoad = false, MappingOrder mappingOrder = MappingOrder.ByColumnIndex, MappingSchema parentMappingSchema = null)
        {
            _db = db;
            _parentMappingSchema = parentMappingSchema;
            _ignoreLazyLoad = ignoreLazyLoad;
            _mappingOrder = mappingOrder;
        }

        #region Overrides

        public override ExtensionList Extensions
        {
            get
            {
                if (_parentMappingSchema != null) 
                    return this._parentMappingSchema.Extensions;
                return _extensions;
            }
            set
            {
                if (_parentMappingSchema != null) 
                    this._parentMappingSchema.Extensions = value;
                _extensions = value;
            }
        }

        protected override ObjectMapper CreateObjectMapperInstance(Type type)
        {
            return new FullObjectMapper(_db, _ignoreLazyLoad);
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
            return internalMapDataReaderToList(reader, list, destObjectType, parameters);
        }

        private IList internalMapDataReaderToList(
            IDataReader reader,
            IList list,
            Type destObjectType,
            params object[] parameters)
        {
            FullObjectMapper mapper = (FullObjectMapper)GetObjectMapper(destObjectType);

            InitSchema(reader);

            object currentItem = null;

            List<int> pkIndexes = new List<int>();
            foreach (var nm in mapper.PrimaryKeyNames)
            {
                pkIndexes.Add(mapper.PropertiesMapping.First(x => x.PropertyName == nm).DataReaderIndex);
            }
            
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

                bool pkIsNull = false;
                bool allPksEqual = true;

                //This is needed, because DBValue can be Null, but the Field can be Guid, wich then is filled with Guid.Empty and this is also a valid value!
                foreach (var pkIndex in pkIndexes)
                {
                    var dbValue = reader.GetValue(pkIndex);
                    if (dbValue == DBNull.Value)
                    {
                        pkIsNull = true;
                        break;
                    }
                }

                if (!pkIsNull)
                    foreach (var pkGetter in mapper.PrimaryKeyValueGetters)
                    {
                        object resultPk = pkGetter.Invoke(result);
                        object currentItemPk = pkGetter.Invoke(currentItem);

                        if (!resultPk.Equals(currentItemPk))
                        {
                            allPksEqual = false;
                            break;
                        }
                    }

                if (!pkIsNull && !allPksEqual)
                {
                    currentItem = result;
                    list.Add(result);
                    //continue;
                }

                if (mapper.ColParent)
                {
                    FillObject(currentItem, mapper, reader);
                }
            }

            return list;
        }
        #endregion

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
                        var curMapper = (FullObjectMapper)GetObjectMapper(fillObject.GetType());

                        object lastElement = list[list.Count - 1];

                        //bool pkIsNull = false;
                        bool allPksEqual = true;

                        //This is needed, because DBValue can be Null, but the Field can be Guid, wich then is filled with Guid.Empty and this is also a valid value!
                        /*foreach (var pkIndex in pkIndexes)
                        {
                            var dbValue = reader.GetValue(pkIndex);
                            if (dbValue == DBNull.Value)
                            {
                                pkIsNull = true;
                                break;
                            }
                        }*/

                        foreach (var pkGetter in curMapper.PrimaryKeyValueGetters)
                        {
                            object lastPk = pkGetter.Invoke(lastElement);
                            object currentPk = pkGetter.Invoke(fillObject);

                            if (!lastPk.Equals(currentPk))
                            {
                                allPksEqual = true;
                                break;
                            }
                        }

                        //object lastPk = curMapper.PrimaryKeyValueGetter.Invoke(lastElement);
                        //object currentPk = curMapper.PrimaryKeyValueGetter.Invoke(fillObject);
                        if (allPksEqual) //lastPk.Equals(currentPk))
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
                    if (((ValueMapper)map).SetDataReaderIndex(_schemaColumns))
                        continue;
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

        #region Private methods

        private void InitSchema(IDataReader reader)
        {
            _schemaColumns = new List<string>();
            _schema = reader.GetSchemaTable();
            if (_schema != null)
                _schema.Rows.Cast<DataRow>().ToList().ForEach(dr => _schemaColumns.Add((string)dr["ColumnName"]));
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

        #endregion
    }
}