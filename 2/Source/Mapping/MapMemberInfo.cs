using System;
using System.Diagnostics;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	public class MapMemberInfo
	{
		private MemberAccessor _memberAccessor;
		public  MemberAccessor  MemberAccessor
		{
			[DebuggerStepThrough] get { return _memberAccessor;  }
			[DebuggerStepThrough] set { _memberAccessor = value; }
		}

		private string _name;
		public  string  Name
		{
			[DebuggerStepThrough] get { return _name;  }
			[DebuggerStepThrough] set { _name = value; }
		}

		private bool _isTrimmable;
		public  bool  IsTrimmable
		{
			[DebuggerStepThrough] get { return _isTrimmable;  }
			[DebuggerStepThrough] set { _isTrimmable = value; }
		}

		private bool _isNullable;
		public  bool  IsNullable
		{
			[DebuggerStepThrough] get { return _isNullable;  }
			[DebuggerStepThrough] set { _isNullable = value; }
		}

		private object _nullValue;
		public  object  NullValue
		{
			[DebuggerStepThrough] get { return _nullValue;  }
			[DebuggerStepThrough] set { _nullValue = value; }
		}

		private object _defaultValue;
		public  object  DefaultValue
		{
			[DebuggerStepThrough] get { return _defaultValue;  }
			[DebuggerStepThrough] set { _defaultValue = value; }
		}

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			[DebuggerStepThrough] get { return _mappingSchema;  }
			[DebuggerStepThrough] set { _mappingSchema = value; }
		}

		private MapValue[] _mapValues;
		public  MapValue[]  MapValues
		{
			[DebuggerStepThrough] get { return _mapValues;  }
			[DebuggerStepThrough] set { _mapValues = value; }
		}

		private MemberExtension _memberExtension;
		public  MemberExtension  MemberExtension
		{
			[DebuggerStepThrough] get { return _memberExtension;  }
			[DebuggerStepThrough] set { _memberExtension = value; }
		}
	
	}
}
