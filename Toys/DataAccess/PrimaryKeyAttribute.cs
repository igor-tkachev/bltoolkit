using System;

namespace Rsdn.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PrimaryKeyAttribute : Attribute
	{
		public PrimaryKeyAttribute()
		{
			_index = -1;
		}

		public PrimaryKeyAttribute(int index)
		{
			_index = index;
		}

		private int _index;
		public  int  Index
		{
			get { return _index; }
		}
	}
}
