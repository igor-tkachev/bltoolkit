using System;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Windows.Forms;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel.Design
{
	public class ObjectViewTypeEditor : TypeEditor
	{
		public override object EditValue(
			ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			Predicate<Type> filter = delegate(Type type)
			{
				return
					type.IsPublic     &&
					!type.IsInterface &&
					!type.ContainsGenericParameters &&
					TypeHelper.IsSameOrParent(typeof(IObjectView), type);
			};

			DataSourceProviderService dspService =
				(DataSourceProviderService)provider.GetService(typeof(DataSourceProviderService));

			if (dspService == null || !dspService.SupportsAddNewDataSource)
			{
				GetTypeDialog dlg = new GetTypeDialog(provider, typeof(IObjectView), filter);

				DialogResult result = dlg.ShowDialog();

				return result == DialogResult.OK && dlg.ResultType != null?
					dlg.ResultType: value;
			}

			return new TypePicker().PickType(provider, value as Type, filter);
		}
	}
}
