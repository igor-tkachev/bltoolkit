using System;
using System.Collections;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Reflection.Extension
{
	public class MemberExtension
	{
		public MemberExtension()
		{
			_attributes = new AttributeNameCollection();
		}

		private MemberExtension(int i)
		{
			_attributes = AttributeNameCollection.Null;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public AttributeExtensionCollection this[string attributeName]
		{
			get { return _attributes[attributeName]; }
		}

		private AttributeNameCollection _attributes;
		public  AttributeNameCollection  Attributes
		{
			get { return _attributes; }
		}

		private static MemberExtension _null = new MemberExtension(0);
		public  static MemberExtension  Null
		{
			get { return _null; }
		}
	}
}
