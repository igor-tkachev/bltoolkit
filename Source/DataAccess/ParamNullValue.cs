using System;
using System.ComponentModel;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class ParamNullValueAttribute : Attribute
	{
		public ParamNullValueAttribute(object value)
		{
			_value = value;
		}

		public ParamNullValueAttribute(Type type, string value)
		{
			_value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
		}

		private readonly object _value;
		public           object  Value
		{
			get { return _value; }
		}
	}
}
