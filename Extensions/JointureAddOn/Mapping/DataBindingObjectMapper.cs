#region

using System;
using BLToolkit.Data;
using BLToolkit.Reflection;

#endregion

namespace BLToolkit.Mapping
{
    public class DataBindingObjectMapper : ObjectMapper
    {
        private readonly Type _type;

        public DataBindingObjectMapper(Type type)
        {
            _type = type;
        }

        public override object CreateInstance()
        {
            return TypeFactory.DataBindingFactory.Create(_type);
        }

        public override object CreateInstance(InitContext context)
        {
            return CreateInstance();
        }
    }

    public class FullDataBindingObjectMapper : FullObjectMapper
    {
        public FullDataBindingObjectMapper(DbManager db, bool ignoreLazyLoading) : base(db, ignoreLazyLoading, FactoryType.LazyLoadingWithDataBinding)
        {
        }

        public override object CreateInstance()
        {
            object result = ContainsLazyChild
                                ? TypeFactory.LazyLoadingWithDataBinding.Create(PropertyType, this, LoadLazy)
                                : base.CreateInstance();

            return result;
        }
    }
}