using System;
using System.Collections;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapExtensionMetadataProvider : MapMetadataProvider
	{
		private object GetValue(ObjectMapper mapper, MemberAccessor member, string elemName, out bool isSet)
		{
			object value = mapper.Extension[member.Name][elemName].Value;

			isSet = value != null;

			return value;
		}

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapField", out isSet);

			if (value != null)
				return value.ToString();

			return base.GetFieldName(mapper, member, out isSet);
		}

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapIgnore", out isSet);

			if (value != null)
				return TypeExtension.ToBoolean(value);

			return base.GetIgnore(mapper, member, out isSet);
		}

		public override bool GetTrimmable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				object value = GetValue(mapper, member, "Trimmable", out isSet);

				if (value != null)
					return TypeExtension.ToBoolean(value);
			}

			return base.GetTrimmable(mapper, member, out isSet);
		}

		public override MapValue[] GetMapValues(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			AttributeExtensionCollection extList = mapper.Extension[member.Name]["MapValue"];

			ArrayList list = null;

			if (extList == AttributeExtensionCollection.Null)
			{
				if (member.Type.IsEnum)
					list = MappingSchema.GetEnumMapValuesFromExtension(mapper.Extension, member.Type);

				if (list == null)
					list = MappingSchema.GetExtensionMapValues(mapper.Extension, member.Type);

				isSet = list != null;

				return isSet? (MapValue[])list.ToArray(typeof(MapValue)): null;
			}

			list = new ArrayList(extList.Count);

			foreach (AttributeExtension ext in extList)
			{
				object origValue = ext["OrigValue"];

				if (origValue != null)
				{
					origValue = TypeExtension.ChangeType(origValue, member.Type);
					list.Add(new MapValue(origValue, ext.Value));
				}
			}

			isSet = true;

			return (MapValue[])list.ToArray(typeof(MapValue));
		}
	}
}
