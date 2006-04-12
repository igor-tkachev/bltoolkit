using System;

using BLToolkit.TypeBuilder;

namespace BLToolkit.EditableObjects
{
	[Serializable]
	public struct EditableObjectHolder
	{
		public EditableObjectHolder(EditableObject obj)
		{
			_original = obj;
			_current  = null;
		}

		private EditableObject _original;
		private EditableObject _current;

		[GetValue, SetValue]
		public EditableObject Value
		{
			get { return _current;  }
			set { _current = value; }
		}
	}
}
