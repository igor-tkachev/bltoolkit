using System;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design;

namespace BLToolkit.ComponentModel
{
	/// <summary>
	/// Converts the value of an object into a <see cref="System.Type"/>.
	/// </summary>
	public class TypeTypeConverter: TypeConverter
	{
		// Human readable text for 'nothing selected'.
		//
		private const string NoType = "(none)";

		/// <summary>
		/// Returns whether this converter can convert an object of the given type to
		/// a <see cref="System.Type"/>, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="System.ComponentModel.ITypeDescriptorContext"/>
		/// that provides a format context. </param>
		/// <param name="sourceType">A <see cref="System.Type"/> that represents the type
		/// you want to convert from. </param>
		/// <returns>
		/// <see langword="true"/> if this converter can perform the conversion;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public override bool CanConvertFrom(
			ITypeDescriptorContext context,
			Type                   sourceType)
		{
			return sourceType == typeof(string) ||
				base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the given object to the corresponding <see cref="System.Type"/>,
		/// using the specified context and culture information.
		/// </summary>
		/// <param name="culture">The <see cref="System.Globalization.CultureInfo"/> to
		/// use as the current culture. </param>
		/// <param name="context">An 
		/// <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides a
		/// format context. </param>
		/// <param name="value">The <see cref="System.Object"/> to convert. </param>
		/// <returns>
		/// An <see cref="System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="System.NotSupportedException">The conversion cannot be
		/// performed. </exception>
		public override object ConvertFrom(
			ITypeDescriptorContext context,
			CultureInfo            culture,
			object                 value)
		{
			if (value == null)
				return null;

			if (!(value is string))
				return base.ConvertFrom(context, culture, value);

			string str = (string)value;

			if (str.Length == 0 || str == NoType)
				return null;

			// Try VisualStudio own service first.
			//
			ITypeResolutionService typeResolver =
				(ITypeResolutionService)context.GetService(typeof(ITypeResolutionService));

			if (typeResolver != null)
			{
				Type type = typeResolver.GetType(str);

				if (type != null)
					return type;
			}

			return Type.GetType(str);
		}

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type,
		/// using the specified context.
		/// </summary>
		/// <param name="context">An 
		/// <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides
		/// a format context. </param>
		/// <param name="destinationType">A <see cref="System.Type"/> that represents
		/// the type you want to convert to. </param>
		/// <returns>
		/// <see langword="true"/> if this converter can perform the conversion;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		public override bool CanConvertTo(
			ITypeDescriptorContext context,
			Type                   destinationType)
		{
			return destinationType == typeof(string) ||
				base.CanConvertTo(context, destinationType);
		}

		/// <summary>
		/// Converts the given value object to the specified type, using the specified
		/// context and culture information.
		/// </summary>
		/// <param name="culture">A <see cref="System.Globalization.CultureInfo"/>.
		/// If null is passed, the current culture is assumed. </param>
		/// <param name="context">An 
		/// <see cref="System.ComponentModel.ITypeDescriptorContext"/> that provides
		/// a format context. </param>
		/// <param name="destinationType">The <see cref="System.Type"/> to convert
		/// the value parameter to. </param>
		/// <param name="value">The <see cref="System.Object"/> to convert. </param>
		/// <returns>
		/// An <see cref="System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="System.NotSupportedException">The conversion cannot be
		/// performed. </exception>
		/// <exception cref="System.ArgumentNullException">
		/// The <paramref name="destinationType"/> parameter is null. </exception>
		public override object ConvertTo(
			ITypeDescriptorContext context,
			CultureInfo            culture,
			object                 value,
			Type                   destinationType)
		{
			if (destinationType != typeof(string))
				return base.ConvertTo(context, culture, value, destinationType);

			if (value == null || value.ToString().Length == 0)
				return NoType;

			return value.ToString();
		}
	}
}
