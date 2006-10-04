using System;
using System.Collections;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapMetadataProviderList : MapMetadataProvider, ICollection
	{
		#region Init

		public MapMetadataProviderList()
		{
			AddProvider(new MapExtensionMetadataProvider());
			AddProvider(new MapAttributeMetadataProvider());
		}

		private ArrayList _list = new ArrayList();

		#endregion

		#region Provider Support

		public override void AddProvider(MapMetadataProvider provider)
		{
			_list.Add(provider);
		}

		public override void InsertProvider(int index, MapMetadataProvider provider)
		{
			_list.Insert(index, provider);
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			foreach (MapMetadataProvider p in _list)
			{
				string name = p.GetFieldName(mapper, member, out isSet);

				if (isSet)
					return name;
			}

			return base.GetFieldName(mapper, member, out isSet);
		}

		#endregion

		#region EnsureMapper

		public override void EnsureMapper(ObjectMapper mapper, EnsureMapperHandler handler)
		{
			foreach (MapMetadataProvider p in _list)
				p.EnsureMapper(mapper, handler);
		}

		#endregion

		#region GetIgnore

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			foreach (MapMetadataProvider p in _list)
			{
				bool ignore = p.GetIgnore(mapper, member, out isSet);

				if (isSet)
					return ignore;
			}

			return base.GetIgnore(mapper, member, out isSet);
		}

		#endregion

		#region GetTrimmable

		public override bool GetTrimmable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				foreach (MapMetadataProvider p in _list)
				{
					bool trimmable = p.GetTrimmable(mapper, member, out isSet);

					if (isSet)
						return trimmable;
				}
			}

			return base.GetTrimmable(mapper, member, out isSet);
		}

		#endregion

		#region GetMapValues

		public override MapValue[] GetMapValues(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			foreach (MapMetadataProvider p in _list)
			{
				MapValue[] value = p.GetMapValues(mapper, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetMapValues(mapper, member, out isSet);
		}

		public override MapValue[] GetMapValues(TypeExtension typeExt, Type type, out bool isSet)
		{
			foreach (MapMetadataProvider p in _list)
			{
				MapValue[] value = p.GetMapValues(typeExt, type, out isSet);

				if (isSet)
					return value;
			}

			return base.GetMapValues(typeExt, type, out isSet);
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			foreach (MapMetadataProvider p in _list)
			{
				object value = p.GetDefaultValue(mapper, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetDefaultValue(mapper, member, out isSet);
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_list.CopyTo(array, index);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsSynchronized
		{
			get { return _list.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _list.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		#endregion
	}
}
