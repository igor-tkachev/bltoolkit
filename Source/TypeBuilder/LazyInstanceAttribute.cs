using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Property)]
	public class LazyInstanceAttribute : Attribute
	{
		public LazyInstanceAttribute()
		{
			_isLazy = true;
		}

		public LazyInstanceAttribute(bool isLazy)
		{
			_isLazy = isLazy;
		}

		private bool _isLazy;
		public  bool  IsLazy
		{
			get { return _isLazy;  }
			set { _isLazy = value; }
		}
	}
}
