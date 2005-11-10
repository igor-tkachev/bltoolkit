using System;

using Rsdn.Framework.Data.Mapping;

#if !VER2
using T = System.Object;
#endif

namespace Rsdn.Framework.EditableObject
{
	[Serializable]
	public class EditableValue
#if VER2
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

				return o == null? c == null: o.Equals(c) == false;
			}
		}

		bool IEditable.IsDirtyMember(string memberName, MapPropertyInfo propertyInfo, ref bool isDirty)
		{
			if (memberName != propertyInfo.PropertyName)
				return false;

			isDirty = IsDirty;

			return true;
		}
	}
}
