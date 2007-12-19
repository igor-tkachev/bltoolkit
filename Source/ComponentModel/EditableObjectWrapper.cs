using System;
using System.ComponentModel;

using BLToolkit.Reflection;
using BLToolkit.Mapping;

namespace BLToolkit.ComponentModel
{
	class EditableObjectWrapper : IEditableObject, INotifyPropertyChanged
	{
		public EditableObjectWrapper(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");

			_wrappedObject = obj;
			_typeAccessor  = TypeAccessor.GetAccessor(obj.GetType());
		}

		private object _wrappedObject;
		public  object  WrappedObject
		{
			get { return _wrappedObject; }
		}

		private TypeAccessor _typeAccessor;
		private object       _copyObject;

		#region IEditableObject Members

		public void BeginEdit()
		{
			_copyObject = TypeAccessor.Copy(_wrappedObject);
		}

		public void CancelEdit()
		{
			TypeAccessor.Copy(_copyObject, _wrappedObject);
		}

		public void EndEdit()
		{
			_copyObject = null;
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;



		#endregion
	}
}
