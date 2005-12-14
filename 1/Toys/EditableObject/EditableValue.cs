using System;
using System.Collections;

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

		public bool AcceptChanges(string memberName, MapPropertyInfo propertyInfo)
		{
			if (memberName != propertyInfo.PropertyName)
				return false;

			AcceptChanges();

			return true;
		}

		public bool RejectChanges(string memberName, MapPropertyInfo propertyInfo)
		{
			if (memberName != propertyInfo.PropertyName)
				return false;

			RejectChanges();

			return true;
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

		void IEditable.GetDirtyMembers(MapPropertyInfo propertyInfo, ArrayList list)
		{
			if (IsDirty)
				list.Add(propertyInfo);
		}

		void IEditable.PrintDebugState(MapPropertyInfo propertyInfo, ref string str)
		{
			str += string.Format("{0,-25} {1} {2,-40} {3,-40} \r\n",
				propertyInfo.PropertyName, IsDirty? "*": " ", _original, _current);
		}
	}
}
