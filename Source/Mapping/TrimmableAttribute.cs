using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class TrimmableAttribute : Attribute
	{
		public TrimmableAttribute()
		{
			_isTrimmable = true;
		}

		public TrimmableAttribute(bool isTrimmable)
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
