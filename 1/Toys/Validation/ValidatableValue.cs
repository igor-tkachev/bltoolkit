using System;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
#if VER2
	public struct ValidatableValue<T> : IValidatable
	{
		public ValidatableValue(T value)
		{
			Value = value;
		}

		public T Value;
#else
	public class ValidatableValue : IValidatable
	{
		public ValidatableValue(object value)
		{
			_value = value;
		}

		private object _value;
		public  object  Value
		{
			get { return _value;  }
			set { _value = value; }
		}
#endif

		public void Validate(MapPropertyInfo info)
		{
			object [] attrs =
				info.PropertyInfo.GetCustomAttributes(typeof(ValidatorBaseAttribute), true);

			foreach (ValidatorBaseAttribute attr in attrs)
				attr.Validate(Value, info.PropertyInfo);
		}
	}
}
