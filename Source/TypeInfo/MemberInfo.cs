using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class MemberInfo
	{
		static MemberInfo()
		{
			_null._attributes._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		private AttributeInfoCollection _attributes = new AttributeInfoCollection();
		public  AttributeInfoCollection  Attributes
		{
			get { return _attributes; }
		}

		private static MemberInfo _null = new MemberInfo();
		public  static MemberInfo  Null
		{
			get { return _null; }
		}
	}
}
