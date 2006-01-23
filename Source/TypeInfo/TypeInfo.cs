using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class TypeInfo
	{
		static TypeInfo()
		{
			_null._members.   _isNull = true;
			_null._attributes._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		private MemberInfoCollection _members = new MemberInfoCollection();
		public  MemberInfoCollection  Members
		{
			get { return _members; }
		}

		private AttributeInfoCollection _attributes = new AttributeInfoCollection();
		public  AttributeInfoCollection  Attributes
		{
			get { return _attributes; }
		}

		private static TypeInfo _null = new TypeInfo();
		public  static TypeInfo  Null
		{
			get { return _null; }
		}
	}
}
