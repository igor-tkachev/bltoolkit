using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field |
		AttributeTargets.Class | AttributeTargets.Interface)]
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

		public static readonly TrimmableAttribute Yes     = new TrimmableAttribute(true);
		public static readonly TrimmableAttribute No      = new TrimmableAttribute(false);
		public static readonly TrimmableAttribute Default = new TrimmableAttribute(false);
	}
}
