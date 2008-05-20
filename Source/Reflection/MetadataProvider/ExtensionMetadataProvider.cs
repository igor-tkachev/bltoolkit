using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using BLToolkit.Mapping;

namespace BLToolkit.Reflection.MetadataProvider
{
	using Extension;

	public class ExtensionMetadataProvider : MetadataProviderBase
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

			List<MapValue> list = new List<MapValue>(extList.Count);

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

			return list.ToArray();
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		static List<MapValue> GetEnumMapValues(TypeExtension typeExt, Type type)
		{
			List<MapValue> mapValues = null;
			FieldInfo[]    fields    = type.GetFields();

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
							mapValues = new List<MapValue>(fields.Length);

						mapValues.Add(new MapValue(origValue, list.ToArray()));
					}
				}
			}

			return mapValues;
		}

		static List<MapValue> GetTypeMapValues(TypeExtension typeExt, Type type)
		{
			AttributeExtensionCollection extList = typeExt.Attributes["MapValue"];

			if (extList == AttributeExtensionCollection.Null)
				return null;

			List<MapValue> attrs = new List<MapValue>(extList.Count);

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
			List<MapValue> list = null;

			if (type.IsEnum)
				list = GetEnumMapValues(typeExt, type);

			if (list == null)
				list = GetTypeMapValues(typeExt, type);

			isSet = list != null;

			return isSet? list.ToArray(): null;
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			object value = mapper.Extension[member.Name]["DefaultValue"].Value;

			if (value != null)
			{
				isSet = value != null;
				return TypeExtension.ChangeType(value, member.Type);
			}

			return GetDefaultValue(mapper.Extension, member.Type, out isSet);
		}

		public override object GetDefaultValue(TypeExtension typeExt, Type type, out bool isSet)
		{
			object value = null;

			if (type.IsEnum)
				value = GetEnumDefaultValueFromExtension(typeExt, type);

			if (value == null)
				value = typeExt.Attributes["DefaultValue"].Value;

			isSet = value != null;

			return TypeExtension.ChangeType(value, type);
		}

		private static object GetEnumDefaultValueFromExtension(TypeExtension typeExt, Type type)
		{
			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
				if ((fi.Attributes & EnumField) == EnumField)
					if (typeExt[fi.Name]["DefaultValue"].Value != null)
						return Enum.Parse(type, fi.Name);

			return null;
		}

		#endregion

		#region GetNullable

		public override bool GetNullable(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			// Check extension <Member1 Nullable='true' />
			//
			object value = GetValue(mapper, member, "Nullable", out isSet);

			if (isSet)
				return TypeExtension.ToBoolean(value);

			// Check extension <Member1 NullValue='-1' />
			//
			if (GetValue(mapper, member, "NullValue", out isSet) != null)
				return true;

			return base.GetNullable(mapper, member, out isSet);
		}

		#endregion

		#region GetNullable

		public override object GetNullValue(ObjectMapper mapper, MemberAccessor member, out bool isSet)
		{
			// Check extension <Member1 NullValue='-1' />
			//
			object value = GetValue(mapper, member, "NullValue", out isSet);

			return isSet? TypeExtension.ChangeType(value, member.Type): null;
		}

		#endregion

		#region GetTableName

		public override string GetTableName(Type type, ExtensionList extensions, out bool isSet)
		{
			TypeExtension typeExt = TypeExtension.GetTypeExtension(type, extensions);

			object value = typeExt.Attributes["TableName"].Value;

			if (value != null)
			{
				isSet = true;
				return value.ToString();
			}

			return base.GetTableName(type, extensions, out isSet);
		}

		#endregion

		#region GetPrimaryKeyOrder

		public override int GetPrimaryKeyOrder(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			object value = typeExt[member.Name]["PrimaryKey"].Value;

			if (value != null)
			{
				isSet = true;
				return (int)TypeExtension.ChangeType(value, typeof(int));
			}

			return base.GetPrimaryKeyOrder(type, typeExt, member, out isSet);
		}

		#endregion

		#region GetNonUpdatableFlag

		public override bool GetNonUpdatableFlag(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			object value = typeExt[member.Name]["NonUpdatable"].Value;

			if (value != null)
			{
				isSet = true;
				return (bool)TypeExtension.ChangeType(value, typeof(bool));
			}

			return base.GetNonUpdatableFlag(type, typeExt, member, out isSet);
		}

		#endregion
	}
}
