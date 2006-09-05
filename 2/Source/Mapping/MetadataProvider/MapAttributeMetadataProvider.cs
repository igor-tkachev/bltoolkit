using System;

using BLToolkit.Reflection;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapAttributeMetadataProvider : MapMetadataProvider
	{
		private object[] _mapFieldAttributes;

		private object[] GetMapFieldAttributes(ObjectMapper mapper)
		{
			if (_mapFieldAttributes == null)
				_mapFieldAttributes = TypeHelper.GetAttributes(mapper.TypeAccessor.Type, typeof(MapFieldAttribute));

			return _mapFieldAttributes;
		}

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			MapFieldAttribute a = (MapFieldAttribute)member.GetAttribute(typeof(MapFieldAttribute));

			if (a != null)
			{
				isSet = true;
				return a.MapName;
			}

			string name = member.Name.ToLower();

			foreach (MapFieldAttribute attr in GetMapFieldAttributes(mapper))
			{
				if (attr.OrigName.ToLower() == name)
				{
					isSet = true;
					return attr.MapName;
				}
			}

			return base.GetFieldName(mapper, member, out isSet);
		}

		public override void EnsureMapper(ObjectMapper mapper, EnsureMapperHandler handler)
		{
			foreach (MapFieldAttribute attr in GetMapFieldAttributes(mapper))
				handler(attr.MapName, attr.OrigName);
		}

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			MapIgnoreAttribute attr = 
				(MapIgnoreAttribute)member.GetAttribute(typeof(MapIgnoreAttribute));

			if (attr != null)
			{
				isSet = true;
				return attr.Ignore;
			}

			return base.GetIgnore(mapper, member, out isSet);
		}

		public override bool GetTrimmable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				TrimmableAttribute attr =
					(TrimmableAttribute)member.GetAttribute(typeof(TrimmableAttribute));

				if (attr != null)
				{
					isSet = true;
					return attr.IsTrimmable;
				}

				attr = (TrimmableAttribute)TypeHelper.GetFirstAttribute(
					member.MemberInfo.DeclaringType, typeof(TrimmableAttribute));

				if (attr != null)
				{
					isSet = true;
					return attr.IsTrimmable;
				}
			}

			return base.GetTrimmable(mapper, member, out isSet);
		}
	}
}
