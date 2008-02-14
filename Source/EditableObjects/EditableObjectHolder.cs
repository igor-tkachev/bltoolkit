using System;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public class /*struct*/ EditableObjectHolder : IEditable, IMemberwiseEditable, ISetParent, IPrintDebugState
	{
		public EditableObjectHolder(EditableObject obj)
		{
			_original     = obj;
			_current      = obj;

			if (_current != null)
				_current.PropertyChanged += _current_PropertyChanged;
		}

		void _current_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			EditableObject obj = _parent as EditableObject;

			if (obj != null)
				obj.OnPropertyChanged(_propertyInfo.Name + "." + e.PropertyName);
		}

		private EditableObject _original;
		private EditableObject _current;
		private object         _parent;
		private PropertyInfo   _propertyInfo;

		[GetValue, SetValue]
		public EditableObject Value
		{
			get { return _current;  }
			set
			{
				if (_current != null)
					_current.PropertyChanged -= _current_PropertyChanged;

				_current = value;

				if (_current != null)
					_current.PropertyChanged += _current_PropertyChanged;
			}
		}

		#region IEditable Members

		public void AcceptChanges()
		{
			_original = _current;

			if (_current != null)
				_current.AcceptChanges();
		}

		public void RejectChanges()
		{
			_current = _original;

			if (_current != null)
				_current.RejectChanges();
		}

		public bool IsDirty
		{
			get
			{
				if (_current == null)
					return _original != null;

				if (_current != _original)
					return true;

				return _current.IsDirty;
			}
		}

		#endregion

		#region IMemberwiseEditable Members

		public bool AcceptMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			if (memberName != propertyInfo.Name)
				return false;

			AcceptChanges();

			return true;
		}

		public bool RejectMemberChanges(PropertyInfo propertyInfo, string memberName)
		{
			if (memberName != propertyInfo.Name)
				return false;

			RejectChanges();

			return true;
		}

		public bool IsDirtyMember(PropertyInfo propertyInfo, string memberName, ref bool isDirty)
		{
			if (memberName != propertyInfo.Name)
				return false;

			isDirty = IsDirty;

			return true;
		}

		public void GetDirtyMembers(PropertyInfo propertyInfo, ArrayList list)
		{
			if (IsDirty)
				list.Add(propertyInfo);
		}

		#endregion

		#region IPrintDebugState Members

		public void PrintDebugState(PropertyInfo propertyInfo, ref string str)
		{
			str += string.Format("{0,-20} {1} {2,-40} {3,-40} \r\n",
				propertyInfo.Name, IsDirty? "*": " ", _original, _current);
		}

		#endregion

		#region ISetParent Members

		public void SetParent(object parent, PropertyInfo propertyInfo)
		{
			_parent       = parent;
			_propertyInfo = propertyInfo;
		}

		#endregion
	}
}
