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

		private Mapper _mapper;
		public  Mapper  Mapper
		{
			get { return _mapper;  }
			set { _mapper = value; }
		}
	}
}
