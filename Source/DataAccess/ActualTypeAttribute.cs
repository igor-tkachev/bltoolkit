using System;

namespace BLToolkit.DataAccess
{
	[JetBrains.Annotations.BaseTypeRequired(typeof(DataAccessor))]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public class ActualTypeAttribute : Attribute
	{
		public ActualTypeAttribute(Type baseType, Type actualType)
		{
			_baseType   = baseType;
			_actualType = actualType;
		}

		private readonly Type _baseType;
		public           Type  BaseType
		{
			get { return _baseType;  }
		}

		private readonly Type _actualType;
		public           Type  ActualType
		{
			get { return _actualType;  }
		}
	}
}
