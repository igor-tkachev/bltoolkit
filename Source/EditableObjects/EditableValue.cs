using System;
using System.Collections;
using System.Reflection;

using BLToolkit.TypeBuilder;

#if !FW2
using T = System.Object;
#endif

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public class EditableValue
#if FW2
	<T>
#endif
	: IEditable
	{
		private T _original;
		private T _current;

		public EditableValue()
		{
#if VER2
			_original = default(T);
			_current  = default(T);
#endif
		}

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

		public virtual void AcceptChanges()
		{
			_original = _current;
		}

		public virtual void RejectChanges()
		{
			_current = _original;
		}

		public virtual bool IsDirty
		{
			get
			{
				object o = _original;
				object c = _current;

				return o == null? c != null: o.Equals(c) == false;
			}
		}

		public virtual bool IsDirtyMember(PropertyInfo propertyInfo, string memberName, ref bool isDirty)
		{
			if (memberName != propertyInfo.Name)
				return false;

			isDirty = IsDirty;

			return true;
		}

		public virtual void GetDirtyMembers(PropertyInfo propertyInfo, ArrayList list)
		{
			if (IsDirty)
				list.Add(propertyInfo);
		}
	}
}
