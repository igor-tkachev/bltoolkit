using System;
using System.Diagnostics;

namespace BLToolkit.Mapping
{
	[AttributeUsage(
		AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Interface,
		AllowMultiple=true)]
	public sealed class MapFieldAttribute : MapIgnoreAttribute
	{
		public MapFieldAttribute()
			: base(false)
		{
		}

		public MapFieldAttribute(string mapName)
			: base(false)
		{
			_mapName = mapName;
		}

		public MapFieldAttribute(string mapName, string origName)
			: base(false)
		{
			_mapName  = mapName;
			_origName = origName;
		}

		private string _mapName;
		public  string  MapName
		{
			[DebuggerStepThrough]
			get { return _mapName;  }
			set { _mapName = value; }
		}

		private string _origName;
		public  string  OrigName
		{
			[DebuggerStepThrough]
			get { return _origName;  }
			set { _origName = value; }
		}

		private string _format;
		public  string  Format
		{
			[DebuggerStepThrough]
			get { return _format;  }
			set { _format = value; }
		}
	}
}
