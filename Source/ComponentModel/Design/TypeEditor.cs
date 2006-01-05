using System;
using System.Drawing.Design;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace BLToolkit.ComponentModel.Design
{
	public class TypeEditor : UITypeEditor
	{
		public TypeEditor()
		{
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			return new TypePicker().PickType(provider, value as Type, delegate(Type type)
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
			});
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;	
		}

		public override bool IsDropDownResizable
		{
			get { return true; }
		}
	}
}
