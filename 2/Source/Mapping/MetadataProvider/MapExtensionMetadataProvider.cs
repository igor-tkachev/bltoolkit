using System;
using System.Collections;
using System.Reflection;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Mapping.MetadataProvider
{
	public class MapExtensionMetadataProvider : MapMetadataProvider
	{
		#region Helpers

		private static object GetValue(ObjectMapper mapper, MemberAccessor member, string elemName, out bool isSet)
		{
			object value = mapper.Extension[member.Name][elemName].Value;

			isSet = value != null;

			return value;
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapField", out isSet);

			if (value != null)
				return value.ToString();

			return base.GetFieldName(mapper, member, out isSet);
		}

		#endregion

		#region GetIgnore

		public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "MapIgnore", out isSet);

			if (value != null)
				return TypeExtension.ToBoolean(value);

			return base.GetIgnore(mapper, member, out isSet);
		}

		#endregion

		#region GetTrimmable

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

		#endregion

		#region GetMapValues

		public override MapValue[] GetMapValues(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			AttributeExtensionCollection extList = mapper.Extension[member.Name]["MapValue"];

			if (extList == AttributeExtensionCollection.Null)
				return GetMapValues(mapper.Extension, member.Type, out isSet);

			ArrayList list = new ArrayList(extList.Count);

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

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		static ArrayList GetEnumMapValues(TypeExtension typeExt, Type type)
		{
			ArrayList   mapValues = null;
			FieldInfo[] fields    = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					AttributeExtensionCollection attrExt = typeExt[fi.Name]["MapValue"];

					if (attrExt.Count == 0)
						continue;

					ArrayList list      = new ArrayList(attrExt.Count);
					object    origValue = Enum.Parse(type, fi.Name);

					foreach (AttributeExtension ae in attrExt)
						if (ae.Value != null)
							list.Add(ae.Value);

					if (list.Count > 0)
					{
						if (mapValues == null)
							mapValues = new ArrayList(fields.Length);

						mapValues.Add(new MapValue(origValue, list.ToArray()));
					}
				}
			}

			return mapValues;
		}

		static ArrayList GetTypeMapValues(TypeExtension typeExt, Type type)
		{
			AttributeExtensionCollection extList = typeExt.Attributes["MapValue"];

			if (extList == AttributeExtensionCollection.Null)
				return null;

			ArrayList attrs = new ArrayList(extList.Count);

			foreach (AttributeExtension ext in extList)
			{
				object origValue = ext["OrigValue"];

				if (origValue != null)
				{
					origValue = TypeExtension.ChangeType(origValue, type);
					attrs.Add(new MapValue(origValue, ext.Value));
				}
			}

			return attrs;
		}

		public override MapValue[] GetMapValues(TypeExtension typeExt, Type type, out bool isSet)
		{
			ArrayList list = null;

			if (type.IsEnum)
				list = GetEnumMapValues(typeExt, type);

			if (list == null)
				list = GetTypeMapValues(typeExt, type);

			isSet = list != null;

			return isSet? (MapValue[])list.ToArray(typeof(MapValue)): null;
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(mapper, member, "DefaultValue", out isSet);

			if (value != null)
				return TypeExtension.ChangeType(value, member.Type);

			return null;
		}

		#endregion
	}
}
