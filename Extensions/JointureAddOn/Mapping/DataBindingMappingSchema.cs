using System;

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
}