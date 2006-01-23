using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class AttributeInfo
	{
		static AttributeInfo()
		{
			_null._values._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public string Value
		{
			get { return _values.Value; }
		}

		private ValueCollection _values = new ValueCollection();
		public  ValueCollection  Values
		{
			get { return _values; }
		}

		private static AttributeInfo _null = new AttributeInfo();
		public  static AttributeInfo  Null
		{
			get { return _null;  }
		}
	}
}
