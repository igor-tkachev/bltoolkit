using System;
using BLToolkit.Reflection;

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
            return DataBindingFactory.Create(_type);
        }

        public override object CreateInstance(InitContext context)
        {
            return CreateInstance();
        }

        public override void Init(MappingSchema mappingSchema, Type type)
        {
            base.Init(mappingSchema, type);
        }
    }
}