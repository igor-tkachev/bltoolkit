using System;
using System.Collections;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapMetadataProviderList : MapMetadataProvider, ICollection
	{
		public MapMetadataProviderList()
		{
			AddProvider(new MapExtensionMetadataProvider());
			AddProvider(new MapAttributeMetadataProvider());
		}

		private ArrayList _list = new ArrayList();

		public override void AddProvider(MapMetadataProvider provider)
		{
			_list.Add(provider);
		}

		public override void InsertProvider(int index, MapMetadataProvider provider)
		{
			_list.Insert(index, provider);
		}

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

		public override void EnsureMapper(ObjectMapper mapper, EnsureMapperHandler handler)
		{
			foreach (MapMetadataProvider p in _list)
				p.EnsureMapper(mapper, handler);
		}

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
