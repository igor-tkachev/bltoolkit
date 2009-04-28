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

		private static object GetValue(TypeExtension typeExtension, MemberAccessor member, string elemName, out bool isSet)
		{
			object value = typeExtension[member.Name][elemName].Value;

			isSet = value != null;

			return value;
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(typeExtension, member, "MapField", out isSet);

			if (value != null)
				return value.ToString();

			return base.GetFieldName(typeExtension, member, out isSet);
		}

		#endregion

		#region GetMapIgnore

		public override bool GetMapIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(typeExtension, member, "MapIgnore", out isSet);

			if (value != null)
				return TypeExtension.ToBoolean(value);

			return base.GetMapIgnore(typeExtension, member, out isSet);
		}

		#endregion

		#region GetTrimmable

		public override bool GetTrimmable(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				object value = GetValue(typeExtension, member, "Trimmable", out isSet);

				if (value != null)
					return TypeExtension.ToBoolean(value);
			}

			return base.GetTrimmable(typeExtension, member, out isSet);
		}

		#endregion

		#region GetMapValues

		public override MapValue[] GetMapValues(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			AttributeExtensionCollection extList = typeExtension[member.Name]["MapValue"];

			if (extList == AttributeExtensionCollection.Null)
				return GetMapValues(typeExtension, member.Type, out isSet);

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

			if (TypeHelper.IsNullable(type))
				type = type.GetGenericArguments()[0];

			if (type.IsEnum)
				list = GetEnumMapValues(typeExt, type);

			if (list == null)
				list = GetTypeMapValues(typeExt, type);

			isSet = list != null;

			return isSet? list.ToArray(): null;
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			object value = typeExtension[member.Name]["DefaultValue"].Value;

			if (value != null)
			{
				isSet = value != null;
				return TypeExtension.ChangeType(value, member.Type);
			}

			return GetDefaultValue(mappingSchema, typeExtension, member.Type, out isSet);
		}

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExt, Type type, out bool isSet)
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

		public override bool GetNullable(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			// Check extension <Member1 Nullable='true' />
			//
			object value = GetValue(typeExtension, member, "Nullable", out isSet);

			if (isSet)
				return TypeExtension.ToBoolean(value);

			// Check extension <Member1 NullValue='-1' />
			//
			if (GetValue(typeExtension, member, "NullValue", out isSet) != null)
				return true;

			return base.GetNullable(mappingSchema, typeExtension, member, out isSet);
		}

		#endregion

		#region GetNullable

		public override object GetNullValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			// Check extension <Member1 NullValue='-1' />
			//
			object value = GetValue(typeExtension, member, "NullValue", out isSet);

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

		#region GetSqlIgnore

		public override bool GetSqlIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			object value = GetValue(typeExtension, member, "SqlIgnore", out isSet);

			if (value != null)
				return TypeExtension.ToBoolean(value);

			return base.GetSqlIgnore(typeExtension, member, out isSet);
		}

		#endregion

		#region GetRelations

		public override List<MapRelationBase> GetRelations(MappingSchema schema, ExtensionList typeExt, Type master, Type slave, out bool isSet)
		{
			List<MapRelationBase> relations = new List<MapRelationBase>();
			TypeExtension         ext       = typeExt != null ? typeExt[master] : TypeExtension.Null;

			isSet = ext != TypeExtension.Null;

			if (!isSet)
				return relations;

			TypeAccessor ta = TypeAccessor.GetAccessor(master);

			foreach (MemberExtension mex in ext.Members)
			{
				AttributeExtensionCollection relationInfos = mex.Attributes[TypeExtension.NodeName.Relation];

				if (relationInfos == AttributeExtensionCollection.Null)
					continue;

				string         destinationTypeName = relationInfos[0][TypeExtension.AttrName.DestinationType, string.Empty].ToString();
				Type           destinationType     = slave;
				MemberAccessor ma                  = ta[mex.Name];
				bool           toMany              = TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Type);

				if (destinationTypeName == string.Empty)
				{
					if (toMany)
						throw new InvalidOperationException("Destination type should be set for enumerable relations: " + ma.Type.FullName + "." + ma.Name);

					destinationType = ma.Type;
				}
				else
				{
					if (!destinationTypeName.Contains(","))
						destinationTypeName += ", " + ta.OriginalType.Assembly.FullName;
					
					try
					{
						destinationType = Type.GetType(destinationTypeName, true);
					}
					catch (TypeLoadException ex)
					{
						throw new InvalidOperationException(
							"Unable to load type by name: " + destinationTypeName
							+ "\n may be assembly is not specefied, please see Type.GetType(string typeName) documentation",
							ex);
					}
				}

				if (slave != null && !TypeHelper.IsSameOrParent(slave, destinationType))
					continue;

				List<string> masterIndexFields = new List<string>();
				List<string> slaveIndexFields  = new List<string>();

				foreach (AttributeExtension ae in relationInfos[0].Attributes[TypeExtension.NodeName.MasterIndex])
					masterIndexFields.Add(ae[TypeExtension.AttrName.Name].ToString());

				foreach (AttributeExtension ae in relationInfos[0].Attributes[TypeExtension.NodeName.SlaveIndex])
					slaveIndexFields.Add(ae[TypeExtension.AttrName.Name].ToString());


				if (slaveIndexFields.Count == 0)
				{
					TypeAccessor  accessor = toMany ? ta : TypeAccessor.GetAccessor(destinationType);
					TypeExtension tex      = TypeExtension.GetTypeExtension(accessor.Type, typeExt);

					slaveIndexFields = GetPrimaryKeyFields(schema, accessor, tex);
				}

				if (slaveIndexFields.Count == 0)
					throw new InvalidOperationException("Slave index is not set for relation: " + ma.Type.FullName + "." + ma.Name);

				MapIndex slaveIndex  = new MapIndex(slaveIndexFields.ToArray());
				MapIndex masterIndex = masterIndexFields.Count > 0 ? new MapIndex(masterIndexFields.ToArray()) : slaveIndex;

				MapRelationBase mapRelation = new MapRelationBase(destinationType, slaveIndex, masterIndex, mex.Name);

				relations.Add(mapRelation);

			}

			isSet = relations.Count > 0;
			return relations;
		}

		#endregion
	}
}
