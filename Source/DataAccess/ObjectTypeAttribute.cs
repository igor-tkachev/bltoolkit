using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ObjectTypeAttribute : Attribute
	{
		public ObjectTypeAttribute(Type objectType)
		{
			_objectType = objectType;
		}

		private readonly Type _objectType;
		public           Type  ObjectType
		{
			get { return _objectType; }
		}
	}
}
