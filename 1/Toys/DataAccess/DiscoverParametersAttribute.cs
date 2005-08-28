using System;

namespace Rsdn.Framework.DataAccess
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DiscoverParametersAttribute : Attribute
	{
		public DiscoverParametersAttribute()
		{
			_discover = true;
		}

		public DiscoverParametersAttribute(bool discover)
		{
			_discover = discover;
		}

		private bool _discover;
		public  bool  Discover
		{
			get { return _discover; }
		}
	}
}
