#region

using System;
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
}