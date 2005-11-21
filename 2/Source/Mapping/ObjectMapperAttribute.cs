using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ObjectMapperAttribute : Attribute
	{
		public ObjectMapperAttribute(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			_objectMapper = Activator.CreateInstance(type) as IObjectMapper;

			if (_objectMapper == null)
				throw new ArgumentException(
					string.Format("Type '{0}' does not implement IObjectMapper interface.", type));
		}

		private IObjectMapper _objectMapper;
		public  IObjectMapper  ObjectMapper
		{
			get { return _objectMapper; }
		}
	}
}
