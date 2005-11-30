using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ObjectMapperAttribute : Attribute
	{
		public ObjectMapperAttribute(Type objectMapperType)
		{
			if (objectMapperType == null) throw new ArgumentNullException("objectMapperType");

			_objectMapper = Activator.CreateInstance(objectMapperType) as IObjectMapper;

			if (_objectMapper == null)
				throw new ArgumentException(
					string.Format("Type '{0}' does not implement IObjectMapper interface.", objectMapperType));
		}

		private IObjectMapper _objectMapper;
		public  IObjectMapper  ObjectMapper
		{
			get { return _objectMapper; }
		}
	}
}
