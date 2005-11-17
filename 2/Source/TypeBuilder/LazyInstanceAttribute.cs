using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.TypeBuilder
{
	[SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments")]
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class LazyInstanceAttribute : Attribute
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
