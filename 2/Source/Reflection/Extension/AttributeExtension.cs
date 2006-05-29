using System;

namespace BLToolkit.Reflection.Extension
{
	public class AttributeExtension
	{
		public AttributeExtension()
		{
			_values = new ValueCollection();
		}

		private AttributeExtension(int i)
		{
			_values = ValueCollection.Null;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public object Value
		{
			get { return this == _null? null: _values.Value; }
		}

		public object this[string valueName]
		{
			get { return this == _null? null: _values[valueName]; }
		}

		private ValueCollection _values;
		public  ValueCollection  Values
		{
			get { return _values; }
		}

		private static AttributeExtension _null = new AttributeExtension(0);
		public  static AttributeExtension  Null
		{
			get { return _null;  }
		}
	}
}
