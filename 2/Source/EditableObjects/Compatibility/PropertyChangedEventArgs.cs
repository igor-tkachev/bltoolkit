using System;

namespace BLToolkit.EditableObjects
{
	public class PropertyChangedEventArgs : EventArgs
	{
		public PropertyChangedEventArgs(string propertyName)
		{
			_propertyName = propertyName;
		}

		private string _propertyName;
		public  string  PropertyName
		{
			get { return _propertyName; }
		}
	}
}

