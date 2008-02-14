using System;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design;

namespace BLToolkit.ComponentModel
{
	public class TypeTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(
			ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
				return null;

			if (value is string)
			{
				string str = value.ToString();

				if (str.Length == 0)
					return null;

				ITypeResolutionService typeResolver =
					(ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));

				if (typeResolver != null)
				{
					Type type = typeResolver.GetType(str);

					if (type != null)
						return type;
				}

				try
				{
					return Type.GetType(str, true);
				}
				catch
				{
					return null;
				}
			}

			return null;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertTo(
			ITypeDescriptorContext context,
			CultureInfo culture,
			object value,
			Type destinationType)
		{
			try
			{
				if (destinationType == typeof(string))
				{
					if (value == null || value.ToString().Length == 0)
						return "(none)";

					return value.ToString();
				}
			}
			catch
			{
				return null;
			}

			return null;
		}
	}
}
