using System;

namespace BLToolkit.Reflection
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class ObjectFactoryAttribute : Attribute
	{
		public ObjectFactoryAttribute(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");

			object obj = (IObjectFactory)Activator.CreateInstance(type);

			if (obj is IObjectFactory)
				_objectFactory = (IObjectFactory)obj;
			else
				throw new ArgumentException(string.Format("Type '{0}' does not implement IObjectFactroty interface."));
		}

		private IObjectFactory _objectFactory;

		public IObjectFactory ObjectFactory
		{
			get { return _objectFactory; }
		}
	}
}
