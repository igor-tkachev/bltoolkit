using System;

namespace BLToolkit.DataAccess
{
	[AttributeUsage(AttributeTargets.Parameter)]
	public class NoMapAttribute : Attribute
	{
		public NoMapAttribute()
		{
			_noMap = true;
		}

		public NoMapAttribute(bool noMap)
		{
			_noMap = noMap;
		}

		private bool _noMap;
		public  bool  NoMap
		{
			get { return _noMap;  }
			set { _noMap = value; }
		}
	}
}

