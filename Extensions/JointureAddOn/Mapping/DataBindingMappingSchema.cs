using System;
using BLToolkit.Data;

namespace BLToolkit.Mapping
{
    public class DataBindingMappingSchema : MappingSchema
    {
        protected override ObjectMapper CreateObjectMapperInstance(Type type)
        {
            var res = new DataBindingObjectMapper(type);

            return res;
        }
    }

    public class FullDataBindingMappingSchema : FullMappingSchema
    {
        private readonly DbManager _db;
        private readonly bool _ignoreLazyLoad;

        public FullDataBindingMappingSchema(DbManager db, bool ignoreLazyLoad = false, MappingSchema parentMappingSchema = null) 
            : base(db, ignoreLazyLoad, parentMappingSchema, FactoryType.LazyLoadingWithDataBinding)
        {
            _db = db;
            _ignoreLazyLoad = ignoreLazyLoad;
        }

        protected override ObjectMapper CreateObjectMapperInstance(Type type)
        {
            return new FullDataBindingObjectMapper(_db, _ignoreLazyLoad);
        }
    }
}