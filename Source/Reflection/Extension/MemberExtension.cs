using System;
using System.Collections;

using BLToolkit.Reflection.Extension;

namespace BLToolkit.Reflection.Extension
{
	public class MemberExtension
	{
		static MemberExtension()
		{
			_null._attributes._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public AttributeExtension this[string attributeName]
		{
			get { return _attributes[attributeName]; }
		}

		private AttributeExtensionCollection _attributes = new AttributeExtensionCollection();
		public  AttributeExtensionCollection  Attributes
		{
			get { return _attributes; }
		}

		private static MemberExtension _null = new MemberExtension();
		public  static MemberExtension  Null
		{
			get { return _null; }
		}
	}
}
