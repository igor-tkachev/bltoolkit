using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field |
		AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class NullableAttribute : Attribute
	{
		public NullableAttribute()
		{
			_isNullable = true;
		}

		public NullableAttribute(bool isNullable)
		{
			_isNullable = isNullable;
		}

		public NullableAttribute(Type type)
		{
			_type       = type;
			_isNullable = true;
		}

		public NullableAttribute(Type type, bool isNullable)
		{
			_type       = type;
			_isNullable = isNullable;
		}

		private bool _isNullable;
		public  bool  IsNullable
		{
			get { return _isNullable;  }
			set { _isNullable = value; }
		}

		private readonly Type _type;
		public           Type  Type
		{
			get { return _type; }
		}
	}
}
