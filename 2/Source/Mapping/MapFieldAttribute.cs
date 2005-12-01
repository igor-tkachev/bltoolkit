using System;

namespace BLToolkit.Mapping
{
	[AttributeUsage(
		AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Interface,
		AllowMultiple=true)]
	public sealed class MapFieldAttribute : Attribute
	{
		public MapFieldAttribute(string mapName)
		{
			_mapName = mapName;
		}

		public MapFieldAttribute(string mapName, string originalName)
		{
			_mapName      = mapName;
			_origName = originalName;
		}

		private string _mapName;
		public  string  MapName
		{
			get { return _mapName; }
		}

		private string _origName;
		public  string  OrigName
		{
			get { return _origName; }
		}
	}
}
