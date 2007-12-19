using System;
using System.ComponentModel;

namespace BLToolkit.EditableObjects
{
	public class EditableListChangedEventArgs : ListChangedEventArgs
	{
		public EditableListChangedEventArgs(int newIndex, int oldIndex)
			: base(ListChangedType.ItemMoved, newIndex, oldIndex)
		{
		}

		public EditableListChangedEventArgs(ListChangedType listChangedType, int index)
			: base(listChangedType, index)
		{
		}

		public EditableListChangedEventArgs(ListChangedType listChangedType)
			: base(listChangedType, -1)
		{
		}

#if FW2
		public EditableListChangedEventArgs(int index, PropertyDescriptor propDesc)
			: base(ListChangedType.ItemChanged, index, propDesc)
		{
		}
#else
		public EditableListChangedEventArgs(int index, PropertyDescriptor propDesc)
			: base(ListChangedType.ItemChanged, index)
		{
			_propertyDescriptor = propDesc;
		}

		PropertyDescriptor _propertyDescriptor;
		PropertyDescriptor  PropertyDescriptor
		{
			get { return _propertyDescriptor; }
		}
#endif
	}
}
