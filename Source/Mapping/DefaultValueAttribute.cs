using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Mapping
{
	[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Interface |
		AttributeTargets.Property | AttributeTargets.Field |
		AttributeTargets.Enum,
		AllowMultiple=true)]
	public class DefaultValueAttribute : Attribute
	{
		public DefaultValueAttribute()
		{
		}

		public DefaultValueAttribute(object value)
		{
			_value = value;
		}

		public DefaultValueAttribute(Type type, object value)
		{
			_type  = type;
			_value = value;
		}

		private object _value;
		public  object  Value
		{
			get { return _value; }
		}

		private Type _type;
		public  Type  Type
		{
			get { return _type; }
		}
	}
}
