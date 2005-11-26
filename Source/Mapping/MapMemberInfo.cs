using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping
{
	public class MapMemberInfo
	{
		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			get { return _memberAccessor;  }
			set { _memberAccessor = value; }
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		private bool _isTrimmable;
		public  bool  IsTrimmable
		{
			get { return _isTrimmable;  }
			set { _isTrimmable = value; }
		}

		private bool _isNullable;
		public  bool  IsNullable
		{
			get { return _isNullable;  }
			set { _isNullable = value; }
		}

		private object _nullValue;
		public  object  NullValue
		{
			get { return _nullValue;  }
			set { _nullValue = value; }
		}

		private object _defaultValue;
		public  object  DefaultValue
		{
			get { return _defaultValue;  }
			set { _defaultValue = value; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			get { return _mappingSchema;  }
			set { _mappingSchema = value; }
		}

		private MapValue[] _mapValues;
		public  MapValue[]  MapValues
		{
			get { return _mapValues;  }
			set { _mapValues = value; }
		}
	}
}
