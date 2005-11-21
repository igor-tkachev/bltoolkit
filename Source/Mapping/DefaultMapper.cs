using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class DefaultMapper : Mapper
	{
		protected override IObjectMapper CreateMapper(Type type)
		{
			Attribute attr = TypeHelper.GetFirstAttribute(type, typeof(ObjectMapperAttribute));

			IObjectMapper om = attr == null? new ObjectMapper(): ((ObjectMapperAttribute)attr).ObjectMapper;

			om.Init(this, TypeAccessor.GetAccessor(type));

			return om;
		}
	}
}
