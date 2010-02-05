using System;
using System.Collections;
using System.Collections.Generic;
using BLToolkit.DataAccess;

namespace BLToolkit.Reflection.MetadataProvider
{
	using Extension;
	using Mapping;

	public class MetadataProviderList : MetadataProviderBase, ICollection
	{
		#region Init

		public MetadataProviderList()
		{
			AddProvider(new ExtensionMetadataProvider());
			AddProvider(new AttributeMetadataProvider());
#if FW3
			AddProvider(new LinqMetadataProvider());
#endif
		}

		private readonly ArrayList _list = new ArrayList();

		#endregion

		#region Provider Support

		public override void AddProvider(MetadataProviderBase provider)
		{
			_list.Add(provider);
		}

		public override void InsertProvider(int index, MetadataProviderBase provider)
		{
			_list.Insert(index, provider);
		}

		public override MetadataProviderBase[] GetProviders()
		{
			return (MetadataProviderBase[])_list.ToArray(typeof(MetadataProviderBase));
		}

		#endregion

		#region GetFieldName

		public override string GetFieldName(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				string name = p.GetFieldName(typeExtension, member, out isSet);

				if (isSet)
					return name;
			}

			return base.GetFieldName(typeExtension, member, out isSet);
		}

		#endregion

		#region GetFieldStorage

		public override string GetFieldStorage(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				string name = p.GetFieldStorage(typeExtension, member, out isSet);

				if (isSet)
					return name;
			}

			return base.GetFieldStorage(typeExtension, member, out isSet);
		}

		#endregion

		#region GetInheritanceDiscriminator

		public override bool GetInheritanceDiscriminator(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				bool value = p.GetInheritanceDiscriminator(typeExtension, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetInheritanceDiscriminator(typeExtension, member, out isSet);
		}

		#endregion

		#region EnsureMapper

		public override void EnsureMapper(TypeAccessor typeAccessor, MappingSchema mappingSchema, EnsureMapperHandler handler)
		{
			foreach (MetadataProviderBase p in _list)
				p.EnsureMapper(typeAccessor, mappingSchema, handler);
		}

		#endregion

		#region GetMapIgnore

		public override bool GetMapIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				bool ignore = p.GetMapIgnore(typeExtension, member, out isSet);

				if (isSet)
					return ignore;
			}

			return base.GetMapIgnore(typeExtension, member, out isSet);
		}

		#endregion

		#region GetTrimmable

		public override bool GetTrimmable(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			if (member.Type == typeof(string))
			{
				foreach (MetadataProviderBase p in _list)
				{
					bool trimmable = p.GetTrimmable(typeExtension, member, out isSet);

					if (isSet)
						return trimmable;
				}
			}

			return base.GetTrimmable(typeExtension, member, out isSet);
		}

		#endregion

		#region GetMapValues

		public override MapValue[] GetMapValues(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				MapValue[] value = p.GetMapValues(typeExtension, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetMapValues(typeExtension, member, out isSet);
		}

		public override MapValue[] GetMapValues(TypeExtension typeExt, Type type, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				MapValue[] value = p.GetMapValues(typeExt, type, out isSet);

				if (isSet)
					return value;
			}

			return base.GetMapValues(typeExt, type, out isSet);
		}

		#endregion

		#region GetDefaultValue

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				object value = p.GetDefaultValue(mappingSchema, typeExtension, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetDefaultValue(mappingSchema, typeExtension, member, out isSet);
		}

		public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, Type type, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				object value = p.GetDefaultValue(mappingSchema, typeExtension, type, out isSet);

				if (isSet)
					return value;
			}

			return base.GetDefaultValue(mappingSchema, typeExtension, type, out isSet);
		}

		#endregion

		#region GetNullable

		public override bool GetNullable(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				bool value = p.GetNullable(mappingSchema, typeExtension, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetNullable(mappingSchema, typeExtension, member, out isSet);
		}

		#endregion

		#region GetNullValue

		public override object GetNullValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				object value = p.GetNullValue(mappingSchema, typeExtension, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetNullValue(mappingSchema, typeExtension, member, out isSet);
		}

		#endregion

		#region GetDbName

		public override string GetDatabaseName(Type type, ExtensionList extensions, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				string value = p.GetDatabaseName(type, extensions, out isSet);

				if (isSet)
					return value;
			}

			return base.GetDatabaseName(type, extensions, out isSet);
		}

		#endregion

		#region GetOwnerName

		public override string GetOwnerName(Type type, ExtensionList extensions, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				string value = p.GetOwnerName(type, extensions, out isSet);

				if (isSet)
					return value;
			}

			return base.GetOwnerName(type, extensions, out isSet);
		}

		#endregion

		#region GetTableName

		public override string GetTableName(Type type, ExtensionList extensions, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				string value = p.GetTableName(type, extensions, out isSet);

				if (isSet)
					return value;
			}

			return base.GetTableName(type, extensions, out isSet);
		}

		#endregion

		#region GetPrimaryKeyOrder

		public override int GetPrimaryKeyOrder(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				int value = p.GetPrimaryKeyOrder(type, typeExt, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetPrimaryKeyOrder(type, typeExt, member, out isSet);
		}

		#endregion

		#region GetNonUpdatableFlag

		public override NonUpdatableAttribute GetNonUpdatableAttribute(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				NonUpdatableAttribute value = p.GetNonUpdatableAttribute(type, typeExt, member, out isSet);

				if (isSet)
					return value;
			}

			return base.GetNonUpdatableAttribute(type, typeExt, member, out isSet);
		}

		#endregion

		#region GetSqlIgnore

		public override bool GetSqlIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				bool ignore = p.GetSqlIgnore(typeExtension, member, out isSet);

				if (isSet)
					return ignore;
			}

			return base.GetSqlIgnore(typeExtension, member, out isSet);
		}

		#endregion

		#region GetRelations

		public override List<MapRelationBase> GetRelations(MappingSchema schema, ExtensionList typeExt, Type master, Type slave, out bool isSet)
		{
			foreach (MetadataProviderBase p in _list)
			{
				List<MapRelationBase> relations = p.GetRelations(schema, typeExt, master, slave, out isSet);

				if (isSet)
					return relations;
			}

			return base.GetRelations(schema, typeExt, master, slave, out isSet);
		}

		#endregion

		#region GetAssociation

		public override Association GetAssociation(TypeExtension typeExtension, MemberAccessor member)
		{
			foreach (MetadataProviderBase p in _list)
			{
				Association attr = p.GetAssociation(typeExtension, member);

				if (attr != null)
					return attr;
			}

			return base.GetAssociation(typeExtension, member);
		}

		#endregion

		#region GetInheritanceMapping

		public override InheritanceMappingAttribute[] GetInheritanceMapping(Type type, TypeExtension typeExtension)
		{
			foreach (MetadataProviderBase p in _list)
			{
				InheritanceMappingAttribute[] attrs = p.GetInheritanceMapping(type, typeExtension);

				if (attrs.Length > 0)
					return attrs;
			}

			return base.GetInheritanceMapping(type, typeExtension);
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
