using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BLToolkit.Reflection.MetadataProvider
{
	using DataAccess;
	using Extension;
	using Mapping;

	public class AttributeMetadataProvider : MetadataProviderBase
	{
		#region Helpers

		private  TypeAccessor _typeAccessor;
		private  object[]     _mapFieldAttributes;
		readonly object       _sync = new object();

		void EnsureMapper(TypeAccessor typeAccessor)
		{
			if (_typeAccessor != typeAccessor)
			{
				_typeAccessor       = typeAccessor;
				_mapFieldAttributes = null;
			}
		}

		object[] GetMapFieldAttributes(TypeAccessor typeAccessor)
		{
			lock (_sync)
			{
				EnsureMapper(typeAccessor);

				if (_mapFieldAttributes == null)
					_mapFieldAttributes = TypeHelper.GetAttributes(typeAccessor.Type, typeof(MapFieldAttribute));

				return _mapFieldAttributes;
			}
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			MapFieldAttribute a = member.GetAttribute<MapFieldAttribute>();

			if (a != null && a.MapName != null)
			{
				isSet = true;
				return a.MapName;
			}

			foreach (MapFieldAttribute attr in GetMapFieldAttributes(member.TypeAccessor))
			{
				if (attr.MapName != null && string.Equals(attr.OrigName, member.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					isSet = true;
					return attr.MapName;
				}
			}

			return base.GetFieldName(typeExtension, member, out isSet);
		}

		#endregion

		#region GetFieldStorage

		public override string GetFieldStorage(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			MapFieldAttribute a = member.GetAttribute<MapFieldAttribute>();

			if (a != null)
			{
				isSet = true;
				return a.Storage;
			}

			foreach (MapFieldAttribute attr in GetMapFieldAttributes(member.TypeAccessor))
			{
				if (string.Equals(attr.OrigName, member.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					isSet = true;
					return attr.Storage;
				}
			}

			return base.GetFieldStorage(typeExtension, member, out isSet);
		}

		#endregion

		#region GetInheritanceDiscriminator

		public override bool GetInheritanceDiscriminator(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			MapFieldAttribute a = member.GetAttribute<MapFieldAttribute>();

			if (a != null)
			{
				isSet = true;
				return a.IsInheritanceDiscriminator;
			}

			foreach (MapFieldAttribute attr in GetMapFieldAttributes(member.TypeAccessor))
			{
				if (string.Equals(attr.OrigName, member.Name, StringComparison.InvariantCultureIgnoreCase))
				{
					isSet = true;
					return attr.IsInheritanceDiscriminator;
				}
			}

			return base.GetInheritanceDiscriminator(typeExtension, member, out isSet);
		}

		#endregion

		#region EnsureMapper

		public override void EnsureMapper(TypeAccessor typeAccessor, MappingSchema mappingSchema, EnsureMapperHandler handler)
		{
			foreach (MapFieldAttribute attr in GetMapFieldAttributes(typeAccessor))
			{
				if (attr.OrigName != null)
					handler(attr.MapName, attr.OrigName);
				else
				{
					MemberAccessor ma = typeAccessor[attr.MapName];

					foreach (MemberMapper inner in mappingSchema.GetObjectMapper(ma.Type))
						handler(string.Format(attr.Format, inner.Name), ma.Name + "." + inner.MemberName);
				}
			}
		}

		#endregion

		#region GetMapIgnore

		public override bool GetMapIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			MapIgnoreAttribute attr = member.GetAttribute<MapIgnoreAttribute>();

			if (attr == null)
				attr = (MapIgnoreAttribute)TypeHelper.GetFirstAttribute(member.Type, typeof(MapIgnoreAttribute));

			if (attr != null)
			{
				isSet = true;
				return attr.Ignore;
			}

			if (member.GetAttribute<MapFieldAttribute>()    != null ||
				member.GetAttribute<MapImplicitAttribute>() != null ||
				TypeHelper.GetFirstAttribute(member.Type, typeof(MapImplicitAttribute)) != null)
			{
				isSet = true;
				return false;
			}

			return base.GetMapIgnore(typeExtension, member, out isSet) || member.GetAttribute<AssociationAttribute>() != null;
		}

		#endregion

		#region GetTrimmable

		public override bool GetTrimmable(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				TrimmableAttribute attr = member.GetAttribute<TrimmableAttribute>();

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

			return base.GetTrimmable(typeExtension, member, out isSet);
		}

		#endregion

		#region GetMapValues

		public override MapValue[] GetMapValues(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			List<MapValue> list = null;

			object[] attrs = member.GetAttributes<MapValueAttribute>();

			if (attrs != null)
			{
				list = new List<MapValue>(attrs.Length);

				foreach (MapValueAttribute a in attrs)
					list.Add(new MapValue(a.OrigValue, a.Values));
			}

			attrs = member.GetTypeAttributes(typeof(MapValueAttribute));

			if (attrs != null && attrs.Length > 0)
			{
				if (list == null)
					list = new List<MapValue>(attrs.Length);

				foreach (MapValueAttribute a in attrs)
					if (a.Type == null && a.OrigValue != null && a.OrigValue.GetType() == member.Type ||
						a.Type is Type && (Type)a.Type == member.Type)
						list.Add(new MapValue(a.OrigValue, a.Values));
			}

			MapValue[] typeMapValues = GetMapValues(typeExtension, member.Type, out isSet);

			if (list == null)
				return typeMapValues;

			if (typeMapValues != null)
				list.AddRange(typeMapValues);

			isSet = true;

			return list.ToArray();
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		static List<MapValue> GetEnumMapValues(Type type)
		{
			List<MapValue> list   = null;
			FieldInfo[]    fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] enumAttributes = Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

					foreach (MapValueAttribute attr in enumAttributes)
					{
						if (list == null)
							list = new List<MapValue>(fields.Length);

						object origValue = Enum.Parse(type, fi.Name);

						list.Add(new MapValue(origValue, attr.Values));
					}
				}
			}

			return list;
		}

		public override MapValue[] GetMapValues(TypeExtension typeExtension, Type type, out bool isSet)
		{
			List<MapValue> list = null;

			if (TypeHelper.IsNullable(type))
				type = type.GetGenericArguments()[0];

			if (type.IsEnum)
				list = GetEnumMapValues(type);

			object[] attrs = TypeHelper.GetAttributes(type, typeof(MapValueAttribute));

			if (attrs != null && attrs.Length != 0)
			{
				if (list == null)
					list = new List<MapValue>(attrs.Length);

				for (int i = 0; i < attrs.Length; i++)
				{
					MapValueAttribute a = (MapValueAttribute)attrs[i];
					list.Add(new MapValue(a.OrigValue, a.Values));
				}
			}

			isSet = list != null;

			return isSet? list.ToArray(): null;
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			// Check member [DefaultValue(0)]
			//
			DefaultValueAttribute attr = member.GetAttribute<DefaultValueAttribute>();

			if (attr != null)
			{
				isSet = true;
				return attr.Value;
			}

			// Check type [DefaultValues(typeof(int), 0)]
			//
			object[] attrs = member.GetTypeAttributes(typeof(DefaultValueAttribute));

			foreach (DefaultValueAttribute a in attrs)
				if (a.Type == null && a.Value != null && a.Value.GetType() == member.Type ||
					a.Type != null && a.Type == member.Type)
				{
					isSet = true;
					return a.Value;
				}

			return GetDefaultValue(mappingSchema, typeExtension, member.Type, out isSet);
		}

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, Type type, out bool isSet)
		{
			object value = null;

			if (type.IsEnum)
				value = GetEnumDefaultValueFromType(type);

			if (value == null)
			{
				object[] attrs = TypeHelper.GetAttributes(type, typeof(DefaultValueAttribute));

				if (attrs != null && attrs.Length != 0)
					value = ((DefaultValueAttribute)attrs[0]).Value;
			}

			isSet = value != null;

			return TypeExtension.ChangeType(value, type);
		}

		private static object GetEnumDefaultValueFromType(Type type)
		{
			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] attrs = Attribute.GetCustomAttributes(fi, typeof(DefaultValueAttribute));

					if (attrs.Length > 0)
						return Enum.Parse(type, fi.Name);
				}
			}

			return null;
		}

		#endregion

		#region GetNullable

		public override bool GetNullable(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			// Check member [Nullable(true | false)]
			//
			NullableAttribute attr1 = member.GetAttribute<NullableAttribute>();

			if (attr1 != null)
			{
				isSet = true;
				return attr1.IsNullable;
			}

			// Check member [NullValue(0)]
			//
			NullValueAttribute attr2 = member.GetAttribute<NullValueAttribute>();

			if (attr2 != null)
				return isSet = true;

			// Check type [Nullable(true || false)]
			//
			attr1 = (NullableAttribute)TypeHelper.GetFirstAttribute(
				member.MemberInfo.DeclaringType, typeof(NullableAttribute));

			if (attr1 != null)
			{
				isSet = true;
				return attr1.IsNullable;
			}

			// Check type [NullValues(typeof(int), 0)]
			//
			object[] attrs = member.GetTypeAttributes(typeof(NullValueAttribute));

			foreach (NullValueAttribute a in attrs)
				if (a.Type == null && a.Value != null && a.Value.GetType() == member.Type ||
					a.Type != null && a.Type == member.Type)
					return isSet = true;

			if (member.Type.IsEnum)
				return isSet = mappingSchema.GetNullValue(member.Type) != null;

			return base.GetNullable(mappingSchema, typeExtension, member, out isSet);
		}

		#endregion

		#region GetNullValue

		private static object CheckNullValue(object value, MemberAccessor member)
		{
			if (value is Type && (Type)value == typeof(DBNull))
			{
				value = DBNull.Value;

				if (member.Type == typeof(string))
					value = null;
			}

			return value;
		}

		public override object GetNullValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			// Check member [NullValue(0)]
			//
			NullValueAttribute attr = member.GetAttribute<NullValueAttribute>();

			if (attr != null)
			{
				isSet = true;
				return CheckNullValue(attr.Value, member);
			}

			// Check type [NullValues(typeof(int), 0)]
			//
			object[] attrs = member.GetTypeAttributes(typeof(NullValueAttribute));

			foreach (NullValueAttribute a in attrs)
			{
				if (a.Type == null && a.Value != null && a.Value.GetType() == member.Type ||
					a.Type != null && a.Type == member.Type)
				{
					isSet = true;
					return CheckNullValue(a.Value, member);
				}
			}

			if (member.Type.IsEnum)
			{
				object value = CheckNullValue(mappingSchema.GetNullValue(member.Type), member);

				if (value != null)
				{
					isSet = true;
					return value;
				}
			}

			isSet = false;
			return null;
		}

		#endregion

		#region GetDbName

		public override string GetDatabaseName(Type type, ExtensionList extensions, out bool isSet)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);

			if (attrs.Length > 0)
			{
				string name = ((TableNameAttribute)attrs[0]).Database;
				isSet = name != null;
				return name;
			}

			return base.GetDatabaseName(type, extensions, out isSet);
		}

		#endregion

		#region GetTableName

		public override string GetOwnerName(Type type, ExtensionList extensions, out bool isSet)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);

			if (attrs.Length > 0)
			{
				string name = ((TableNameAttribute)attrs[0]).Owner;
				isSet = name != null;
				return name;
			}

			return base.GetOwnerName(type, extensions, out isSet);
		}

		#endregion

		#region GetTableName

		public override string GetTableName(Type type, ExtensionList extensions, out bool isSet)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TableNameAttribute), true);

			if (attrs.Length > 0)
			{
				string name = ((TableNameAttribute)attrs[0]).Name;
				isSet = name != null;
				return name;
			}

			return base.GetTableName(type, extensions, out isSet);
		}

		#endregion

		#region GetPrimaryKeyOrder

		public override int GetPrimaryKeyOrder(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			PrimaryKeyAttribute attr = member.GetAttribute<PrimaryKeyAttribute>();

			if (attr != null)
			{
				isSet = true;
				return attr.Order;
			}

			return base.GetPrimaryKeyOrder(type, typeExt, member, out isSet);
		}

		#endregion

		#region GetNonUpdatableFlag

		public override NonUpdatableAttribute GetNonUpdatableAttribute(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			NonUpdatableAttribute attr = member.GetAttribute<NonUpdatableAttribute>();

			if (attr != null)
			{
				isSet = true;
				return attr;
			}

			return base.GetNonUpdatableAttribute(type, typeExt, member, out isSet);
		}

		#endregion

		#region GetSqlIgnore

		public override bool GetSqlIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			SqlIgnoreAttribute attr = member.GetAttribute<SqlIgnoreAttribute>();

			if (attr == null)
				attr = (SqlIgnoreAttribute)TypeHelper.GetFirstAttribute(member.Type, typeof(SqlIgnoreAttribute));

			if (attr != null)
			{
				isSet = true;
				return attr.Ignore;
			}

			return base.GetSqlIgnore(typeExtension, member, out isSet);
		}

		#endregion

		#region GetRelations

		public override List<MapRelationBase> GetRelations(MappingSchema schema, ExtensionList typeExt, Type master, Type slave, out bool isSet)
		{
			TypeAccessor masterAccessor = TypeAccessor.GetAccessor(master);
			TypeAccessor slaveAccessor  = slave != null ? TypeAccessor.GetAccessor(slave) : null;

			List<MapRelationBase> relations = new List<MapRelationBase>();

			foreach (MemberAccessor ma in masterAccessor)
			{
				RelationAttribute attr = ma.GetAttribute<RelationAttribute>();

				if (attr == null || (slave != null && attr.Destination != slave && ma.Type != slave))
					continue;

				if (slave == null)
					slaveAccessor = TypeAccessor.GetAccessor(attr.Destination ?? ma.Type);


				bool toMany = TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Type);
				if (toMany && attr.Destination == null)
					throw new InvalidOperationException("Destination type should be set for enumerable relations: " + ma.Type.FullName + "." + ma.Name);

				MapIndex masterIndex = attr.MasterIndex;
				MapIndex slaveIndex  = attr.SlaveIndex;

				if (slaveIndex == null)
				{
					TypeAccessor  accessor = toMany ? masterAccessor : slaveAccessor;
					TypeExtension tex      = TypeExtension.GetTypeExtension(accessor.Type, typeExt);

					List<string> keys = GetPrimaryKeyFields(schema, accessor, tex);

					if (keys.Count > 0)
						slaveIndex = new MapIndex(keys.ToArray());
				}

				if (slaveIndex == null)
					throw new InvalidOperationException("Slave index is not set for relation: " + ma.Type.FullName + "." + ma.Name);

				if (masterIndex == null)
					masterIndex = slaveIndex;

				MapRelationBase relation = new MapRelationBase(attr.Destination ?? ma.Type, slaveIndex, masterIndex, ma.Name);

				relations.Add(relation);
			}

			isSet = true;
			return relations;
		}
		
		#endregion

		#region GetAssociation

		public override Association GetAssociation(TypeExtension typeExtension, MemberAccessor member)
		{
			AssociationAttribute aa = member.GetAttribute<AssociationAttribute>();

			if (aa == null)
				return base.GetAssociation(typeExtension, member);

			return new Association(
				member,
				aa.GetThisKeys(),
				aa.GetOtherKeys(),
				aa.Storage,
				aa.CanBeNull);
		}

		#endregion

		#region GetInheritanceMapping

		public override InheritanceMappingAttribute[] GetInheritanceMapping(Type type, TypeExtension typeExtension)
		{
			object[] attrs = type.GetCustomAttributes(typeof(InheritanceMappingAttribute), true);

			if (attrs.Length > 0)
			{
				InheritanceMappingAttribute[] maps = new InheritanceMappingAttribute[attrs.Length];

				for (int i = 0; i < attrs.Length; i++)
					maps[i] = (InheritanceMappingAttribute)attrs[i];

				return maps;
			}

			return base.GetInheritanceMapping(type, typeExtension);
		}

		#endregion
	}
}
