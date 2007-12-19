namespace BLToolkit.Reflection.Extension
{
	public class AttributeExtension
	{
		public AttributeExtension()
		{
			_values = new ValueCollection();
		}

		private AttributeExtension(ValueCollection values)
		{
			_values = values;
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

		public object this[string valueName, object defaultValue]
		{
			get
			{
				object value = this[valueName];
				return value == null? defaultValue: value;
			}
		}

		private readonly ValueCollection _values;
		public           ValueCollection  Values
		{
			get { return _values; }
		}

		private AttributeNameCollection _attributes;
		public  AttributeNameCollection  Attributes
		{
			get
			{
				if (_attributes == null)
					_attributes = new AttributeNameCollection();
				return _attributes;
			}
		}

		private static readonly AttributeExtension _null = new AttributeExtension(ValueCollection.Null);
		public  static          AttributeExtension  Null
		{
			get { return _null;  }
		}
	}
}
