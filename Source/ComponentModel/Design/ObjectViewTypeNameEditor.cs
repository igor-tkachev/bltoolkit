using System;
using System.Globalization;
using System.ComponentModel;

namespace BLToolkit.ComponentModel.Design
{
	public class ObjectViewTypeNameEditor : ObjectViewTypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			TypeTypeConverter converter = new TypeTypeConverter();

			value = converter.ConvertFrom(context, CultureInfo.CurrentCulture, value);
			value = base.EditValue(context, provider, value);
			value = converter.ConvertTo  (context, CultureInfo.CurrentCulture, value, typeof(string));

			return value;
		}
	}
}
