using System;

namespace BLToolkit.Reflection
{
	public class InitContext
	{
		public InitContext()
		{
		}

		private object [] _memberParameters;
		public  object []  MemberParameters
		{
			get { return _memberParameters;  }
			set { _memberParameters = value; }
		}

		private bool _isInternal;
		public  bool  IsInternal
		{
			get { return _isInternal;  }
			set { _isInternal = value; }
		}

		private bool _isLazyInstance;
		public  bool  IsLazyInstance
		{
			get { return _isLazyInstance;  }
			set { _isLazyInstance = value; }
		}
	}
}
