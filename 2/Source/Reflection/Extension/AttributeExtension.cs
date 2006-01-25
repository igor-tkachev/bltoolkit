using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class AttributeExtension
	{
		static AttributeExtension()
		{
			_null._values._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public object Value
		{
			get { return _values.Value; }
		}

		private ValueCollection _values = new ValueCollection();
		public  ValueCollection  Values
		{
			get { return _values; }
		}

		private static AttributeExtension _null = new AttributeExtension();
		public  static AttributeExtension  Null
		{
			get { return _null;  }
		}
	}
}
