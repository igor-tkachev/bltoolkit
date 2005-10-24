using System;

namespace BLToolkit.TypeBuilder
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public class LazyInstancesAttribute : Attribute
	{
		public LazyInstancesAttribute()
		{
			_isLazy = true;
		}

		public LazyInstancesAttribute(bool isLazy)
		{
			_isLazy = isLazy;
		}

		private bool _isLazy;
		public bool IsLazy
		{
			get { return _isLazy; }
			set { _isLazy = value; }
		}
	}
}
