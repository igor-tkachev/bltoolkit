using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace BLToolkit.ComponentModel.Design
{
	public class TypeEditor : UITypeEditor
	{
		public override object EditValue(
			ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			DataSourceProviderService dspService =
				(DataSourceProviderService)provider.GetService(typeof(DataSourceProviderService));

			if (dspService == null || !dspService.SupportsAddNewDataSource)
			{
				GetTypeDialog dlg = new GetTypeDialog(provider, typeof(object), FilterTypeList);

				DialogResult result = dlg.ShowDialog();

				return result == DialogResult.OK && dlg.ResultType != null?
					dlg.ResultType: value;
			}

			return new TypePicker().PickType(provider, value as Type, FilterTypeList);
		}

		protected virtual bool FilterTypeList(Type type)
		{
			return
				type.IsPublic     &&
				type.IsClass      &&
				//!type.IsInterface &&
				!type.ContainsGenericParameters &&
				!typeof(ICollection).IsAssignableFrom(type) &&
				!typeof(Attribute).  IsAssignableFrom(type) &&
				!typeof(Exception).  IsAssignableFrom(type) &&
				!typeof(EventArgs).  IsAssignableFrom(type) &&
				!typeof(Control).    IsAssignableFrom(type) &&
				!typeof(DataTable).  IsAssignableFrom(type) &&
				!typeof(DataView).   IsAssignableFrom(type) &&
				!typeof(DataRow).    IsAssignableFrom(type) &&
				!typeof(DataRowView).IsAssignableFrom(type) &&
				!typeof(DataSet).    IsAssignableFrom(type);
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context == null)
				return UITypeEditorEditStyle.DropDown;

			DataSourceProviderService dspService =
				(DataSourceProviderService)context.GetService(typeof(DataSourceProviderService));

			return dspService == null || !dspService.SupportsAddNewDataSource?
				UITypeEditorEditStyle.Modal: UITypeEditorEditStyle.DropDown;
		}

		public override bool IsDropDownResizable
		{
			get { return true; }
		}
	}
}
