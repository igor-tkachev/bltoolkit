using System;
using System.Reflection;

using Rsdn.Framework.Data.Mapping;

namespace Rsdn.Framework.Validation
{
	public class ValidationContext
	{
		public delegate bool IsNullHandler(ValidationContext context);

		private object _object;
		public  object  Object
		{
			get { return _object; }
			set 
			{
				_object     = value;
				_descriptor = Map.Descriptor(value.GetType());
			}
		}

		private MapDescriptor _descriptor;
		public  MapDescriptor  Descriptor
		{
			get { return _descriptor;  }
		}

		private IsNullHandler _isNull;
		public  IsNullHandler  IsNull
		{
			get { return _isNull;  }
			set { _isNull = value; }
		}

		private object _value;
		public  object  Value
		{
			get { return _value;  }
			set { _value = value; }
		}

		private IMemberMapper _memberMapper;
		public  IMemberMapper  MemberMapper
		{
			get { return _memberMapper; }
			set 
			{
				_memberMapper = value;
				_memberInfo   = value.MemberInfo;
			}
		}

		private MemberInfo _memberInfo;
		public  MemberInfo  MemberInfo
		{
			get { return _memberInfo;  }
		}
	}
}
