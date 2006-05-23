using System;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Windows.Forms;

using BLToolkit.Reflection;

namespace BLToolkit.ComponentModel.Design
{
	public class ObjectViewTypeEditor : TypeEditor
	{
		protected override bool FilterTypeList(Type type)
		{
			return
				type.IsPublic     &&
				!type.IsInterface &&
				!type.ContainsGenericParameters &&
				TypeHelper.IsSameOrParent(typeof(IObjectView), type);
		}
	}
}
