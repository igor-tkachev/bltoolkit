using System;
using System.Collections;
using System.Reflection;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public struct EditableValue<T>: IEditable, IMemberwiseEditable, IPrintDebugState
	{
		private T _original;
		private T _current;

		public EditableValue(T value)
		{
			_original = value;
			_current  = value;
		}

		[GetValue, SetValue]
		public T Value
		{
			get { return _current;  }
			set { _current = value; }
		}

		#region IEditable Members

		public void AcceptChanges()
		{
			_original = _current;
		}

		public void RejectChanges()
		{
			_current = _original;
		}

		public bool IsDirty
		{
			get
			{
				object o = _original;
				object c = _current;

				return o == null? c != null: o.Equals(c) == false;
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
	}
}
