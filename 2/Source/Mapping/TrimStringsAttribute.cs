using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class TrimStringsAttribute : Attribute
	{
		public TrimStringsAttribute()
		{
			_isTrimmable = true;
		}

		public TrimStringsAttribute(bool isTrimmable)
		{
			_isTrimmable = isTrimmable;
		}

		private bool _isTrimmable;
		public  bool  IsTrimmable
		{
			get { return _isTrimmable;  }
			set { _isTrimmable = value; }
		}
	}
}
