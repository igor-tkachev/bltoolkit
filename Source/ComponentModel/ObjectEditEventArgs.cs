using System;

namespace BLToolkit.ComponentModel
{
	public class ObjectEditEventArgs
	{
		public ObjectEditEventArgs(ObjectEditType editType)
		{
			_editType = editType;
		}

		private ObjectEditType _editType;
		public  ObjectEditType  EditType
		{
			get { return _editType;  }
			set { _editType = value; }
		}
	}
}
