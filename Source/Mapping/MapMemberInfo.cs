using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping
{
	[DebuggerStepThrough]
	public class MapMemberInfo
	{
		public MapMemberInfo()
		{
			DbType = DbType.Object;
		}

		public MemberAccessor  MemberAccessor             { get; set; }
		public MemberAccessor  ComplexMemberAccessor      { get; set; }
		public string          Name                       { get; set; }
		public string          MemberName                 { get; set; }
		public string          Storage                    { get; set; }
		public bool            IsInheritanceDiscriminator { get; set; }
		public bool            Trimmable                  { get; set; }
		public bool            SqlIgnore                  { get; set; }
		public bool            Nullable                   { get; set; }
		public object          NullValue                  { get; set; }
		public object          DefaultValue               { get; set; }
		public Type            Type                       { get; set; }
		public int             DbSize                     { get; set; }
		public bool            IsDbTypeSet                { get; set; }
		public bool            IsDbSizeSet                { get; set; }
		public MappingSchema   MappingSchema              { get; set; }
		public MemberExtension MemberExtension            { get; set; }
		public DbType          DbType                     { get; set; }
		public KeyGenerator    KeyGenerator               { get; set; }

		private MapValue[] _mapValues;
		public  MapValue[]  MapValues
		{
			get { return _mapValues; }
			set
			{
				_mapValues = value;
				if (value != null)
					CacheMapValues();
				else
				{
					_mapValueCache  = new Dictionary<object, object>();
					_origValueCache = new Dictionary<object, object>();
				}
			}
		}

		private Dictionary<object,object> _mapValueCache;

		public bool TryGetOrigValue(object mapedValue, out object origValue)
		{
			return _mapValueCache.TryGetValue(mapedValue, out origValue);
		}

		private Dictionary<object,object> _origValueCache;

		public bool TryGetMapValue(object origValue, out object mapValue)
		{
			return _origValueCache.TryGetValue(origValue, out mapValue);
		}

		private void CacheMapValues()
		{
			_mapValueCache = new Dictionary<object,object>();

			foreach (var mv in MapValues)
			foreach (var mapValue in mv.MapValues)
			{
				_mapValueCache[mapValue] = mv.OrigValue;

				// this fixes spesial case for char
				if (mapValue is char)
				{
					var str = new string(new[] { (char)mapValue });
					_mapValueCache[str] = mv.OrigValue;
				}
			}

			_origValueCache = new Dictionary<object, object>();

			foreach (var mv in MapValues)
			{
				// previous behaviour - first wins!
				// yah, no...
				// any wins - attributes order is not specified
				// and memberInfo.GetCustomAttributes(...) order and can differ
				if (!_origValueCache.ContainsKey(mv.OrigValue))
					_origValueCache[mv.OrigValue] = mv.MapValues[0];
			}
		}
	}
}
