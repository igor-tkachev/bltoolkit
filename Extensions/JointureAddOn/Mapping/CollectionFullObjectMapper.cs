using System;
using System.Collections.Generic;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Emit;

namespace BLToolkit.Mapping
{
    public class CollectionFullObjectMapper : TableDescription, IObjectMapper
    {
        private readonly DbManager _db;
        private readonly FactoryType _factoryType;

        public CollectionFullObjectMapper(DbManager db, FactoryType factoryType)
        {
            _db = db;
            _factoryType = factoryType;

            PropertiesMapping = new List<IMapper>();
            PrimaryKeyValueGetters = new List<GetHandler>();
            PrimaryKeyNames = new List<string>();
        }

        public Type PropertyCollectionType { get; set; }

        #region IMapper Members

        public int DataReaderIndex { get; set; }
        public SetHandler Setter { get; set; }
        public Type PropertyType { get; set; }
        public string PropertyName { get; set; }

        #endregion

        #region IObjectMapper

        public bool IsLazy { get; set; }
        public bool ContainsLazyChild { get; set; }
        public GetHandler Getter { get; set; }

        public List<GetHandler> PrimaryKeyValueGetters { get; set; }
        public Association Association { get; set; }

        public List<string> PrimaryKeyNames { get; set; }

        #endregion

        #region ILazyMapper

        public GetHandler ParentKeyGetter { get; set; }

        #endregion

        public object CreateInstance()
        {
            object result = ContainsLazyChild
                    ? (_factoryType == FactoryType.LazyLoading
                           ? TypeFactory.LazyLoading.Create(PropertyType, this, LoadLazy)
                           : TypeFactory.LazyLoadingWithDataBinding.Create(PropertyType, this, LoadLazy))
                    : FunctionFactory.Remote.CreateInstance(PropertyType);

            return result;
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

        #region IPropertiesMapping Members

        public List<IMapper> PropertiesMapping { get; private set; }
        public IPropertiesMapping ParentMapping { get; set; }

        #endregion
    }
}