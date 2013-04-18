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
using BLToolkit.Reflection.Extension;

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
        private readonly FactoryType _factoryType;
        private readonly bool _ignoreLazyLoad;

        #endregion

        public FullObjectMapper(DbManager db, bool ignoreLazyLoading, FactoryType factoryType)
        {
            _db = db;
            _ignoreLazyLoad = ignoreLazyLoading;
            _factoryType = factoryType;

            PropertiesMapping = new List<IMapper>();
            PrimaryKeyValueGetters = new List<GetHandler>();
            PrimaryKeyNames = new List<string>();
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
        public Association Association { get; set; }

        #endregion

        #region IObjectMapper

        public bool IsLazy { get; set; }
        public bool ContainsLazyChild { get; set; }
        public GetHandler Getter { get; set; }

        public List<string> PrimaryKeyNames { get; set; }

        #endregion

        #region ILazyMapper

        public GetHandler ParentKeyGetter { get; set; }
        public List<GetHandler> PrimaryKeyValueGetters { get; set; }

        #endregion

        #region Overrides

        public override void Init(MappingSchema mappingSchema, Type type)
        {
            PropertyType = type;

            // TODO implement this method
            base.Init(mappingSchema, type);

            int startIndex = 0;
            GetObjectMapper(this, ref startIndex, _typeAccessor);
        }

        public override object CreateInstance()
        {
            object result = ContainsLazyChild
                                ? (_factoryType == FactoryType.LazyLoading
                                       ? TypeFactory.LazyLoading.Create(PropertyType, this, LoadLazy)
                                       : TypeFactory.LazyLoadingWithDataBinding.Create(PropertyType, this, LoadLazy))
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

        private IMapper GetObjectMapper(IObjectMapper mapper, ref int startIndex, TypeAccessor akTypeAccessor)
        {
            //Todo: Remove this Call!
            _extension = TypeExtension.GetTypeExtension(mapper.PropertyType /*_typeAccessor.OriginalType*/, MappingSchema.Extensions);

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

            MemberAccessor primaryKeyMemberAccessor = null;
            foreach (MemberAccessor ma in akTypeAccessor)
            {
                //  Setters
                lock (SetterHandlersLock)
                {
                    if (!SettersHandlers[mapper.PropertyType].ContainsKey(ma.Name))
                    {
                        SettersHandlers[mapper.PropertyType].Add(ma.Name, ma.SetValue);
                    }
                }

                if (GetPrimaryKey(ma) != null)
                {
                    primaryKeyMemberAccessor = ma;

                    lock (SetterHandlersLock)
                    {
                        if (!GettersHandlers[mapperType].ContainsKey(ma.Name))
                        {
                            GettersHandlers[mapperType].Add(ma.Name, ma.GetValue);
                        }
                    }
                    mapper.PrimaryKeyValueGetters.Add(GettersHandlers[mapperType][ma.Name]);
                    mapper.PrimaryKeyNames.Add(ma.Name);

                    if (mapper.Association != null && (mapper.Association.OtherKey == null || mapper.Association.OtherKey.Length == 0))
                    {
                        mapper.Association.OtherKey = new[] {ma.Name};
                    }
                }
            }
            if (primaryKeyMemberAccessor == null)
                throw new Exception("PrimaryKey attribute not found on type: " + mapperType);

            foreach (PropertyInfo prop in properties)
            {
                var ma = akTypeAccessor.First(x => x.Name == prop.Name);

                // Check if the accessor is an association
                var association = GetAssociation(ma);
                if (association != null)
                {
                    //  Getters for IObjectMapper
                    lock (SetterHandlersLock)
                        if (!GettersHandlers[mapperType].ContainsKey(prop.Name))
                        {
                            GettersHandlers[mapperType].Add(prop.Name, ma.GetValue);
                        }

                    bool isCollection = prop.PropertyType.GetInterfaces().ToList().Contains(typeof (IList));
                    IObjectMapper propertiesMapping;
                    if (!isCollection)
                    {
                        // TODO Generate this instance using the CreateObjectMapperInstance method of fullMappingSchema
                        // _db.MappingSchema.CreateObjectMapperInstance(prop.PropertyType)

                        propertiesMapping = new FullObjectMapper(_db, _ignoreLazyLoad, _factoryType)
                            {
                                PropertyType = prop.PropertyType,
                                IsNullable = association.CanBeNull,
                                Getter = GettersHandlers[mapperType][prop.Name],
                            };
                    }
                    else
                    {
                        Type listElementType = GetGenericType(prop.PropertyType);
                        TableDescription colElementTableDescription = GetTableDescription(listElementType);

                        // TODO Generate this instance using the CreateObjectMapperInstance method of fullMappingSchema
                        propertiesMapping = new CollectionFullObjectMapper(_db, _factoryType)
                            {
                                PropertyType = listElementType,
                                Getter = GettersHandlers[mapperType][prop.Name],
                                TableName = colElementTableDescription.TableName,
                                PropertyCollectionType = prop.PropertyType,
                            };

                        if (mapper is FullObjectMapper)
                            ((FullObjectMapper) mapper).ColParent = true;
                    }

                    if (association.ThisKey == null || association.ThisKey.Length == 0)
                        association.ThisKey = new[] {primaryKeyMemberAccessor.Name};

                    bool isLazy = false;
                    if (!_ignoreLazyLoad)
                    {
                        var lazy = GetLazyInstance(ma); // prop.GetCustomAttributes(typeof(LazyInstanceAttribute), true);
                        if (lazy)
                        {
                            isLazy = true;
                            mapper.ContainsLazyChild = true;

                            //  Getters
                            lock (SetterHandlersLock)
                                if (!GettersHandlers[mapperType].ContainsKey(primaryKeyMemberAccessor.Name))
                                {
                                    GettersHandlers[mapperType].Add(primaryKeyMemberAccessor.Name, primaryKeyMemberAccessor.GetValue);
                                }
                        }
                    }

                    propertiesMapping.Association = association;
                    propertiesMapping.PropertyName = prop.Name;
                    propertiesMapping.IsLazy = isLazy;
                    propertiesMapping.Setter = SettersHandlers[mapperType][prop.Name];

                    if (propertiesMapping.IsLazy)
                    {
                        propertiesMapping.ParentKeyGetter = GettersHandlers[mapperType][primaryKeyMemberAccessor.Name];
                    }
                    objectMappers.Add(propertiesMapping);
                }
                else
                {
                    var mapIgnore = GetMapIgnore(ma);
                    if (mapIgnore)
                        continue;

                    var mapField = GetMapField(ma);
                    string columnName = mapField != null ? mapField.MapName : prop.Name;

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

                    var pkField = GetPrimaryKey(ma);
                    if (pkField != null)
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
                mapper.PropertiesMapping.Add(GetObjectMapper(objMap, ref startIndex, MappingSchema.GetObjectMapper(objMap.PropertyType).TypeAccessor));
            }

            return mapper;
        }

        protected object LoadLazy(IMapper mapper, object proxy, Type parentType)
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

        private static Type GetGenericType(Type t)
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