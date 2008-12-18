using System;
using System.Collections.Generic;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Sql
{
	public class Table
	{
		#region Init

		public Table()
			: this(Map.DefaultSchema)
		{
		}

		public Table(MappingSchema mappingSchema)
		{
			if (mappingSchema == null) throw new ArgumentNullException("mappingSchema");

			_mappingSchema = mappingSchema;

			_fields = new ChildContainer<Table,Field>(this);
		}

		#endregion

		#region Init from type

		public Table(MappingSchema mappingSchema, Type objectType)
			: this(mappingSchema)
		{
			bool isSet;
			Name = _mappingSchema.MetadataProvider.GetTableName(objectType, _mappingSchema.Extensions, out isSet);

			foreach (MemberMapper mm in MappingSchema.GetObjectMapper(objectType))
				Fields.Add(new Field(mm.MemberName, mm.Name));
		}

		public Table(Type objectType)
			: this(Map.DefaultSchema, objectType)
		{
		}

		#endregion

		#region Init from Table

		public Table(MappingSchema mappingSchema, Table table)
			: this(mappingSchema)
		{
			_alias        = table._alias;
			_database     = table._database;
			_owner        = table._owner;
			_name         = table._name;
			_physicalName = table._physicalName;

			foreach (Field field in table.Fields.Values)
				Fields.Add(new Field(field.Name, field.PhysicalName));
		}

		public Table(Table table)
			: this(table.MappingSchema, table)
		{
		}

		#endregion

		#region Init from XML

		public Table(MappingSchema mappingSchema, TypeExtension extension)
			: this(mappingSchema)
		{
			if (extension == null)               throw new ArgumentNullException("extension");
			if (extension == TypeExtension.Null) throw new InvalidOperationException("Table not found.");

			_name         = extension.Name;
			_alias        = (string)extension.Attributes["Alias"].       Value;
			_database     = (string)extension.Attributes["Database"].    Value;
			_owner        = (string)extension.Attributes["Owner"].       Value;
			_physicalName = (string)extension.Attributes["PhysicalName"].Value;

			foreach (MemberExtension me in extension.Members)
				Fields.Add(new Field(me.Name, (string)me["MapField"].Value ?? (string)me["PhysicalName"].Value));
		}

		public Table(TypeExtension extension)
			: this(Map.DefaultSchema, extension)
		{
		}

		#endregion

		#region Public Members

		public Field this[string fieldName]
		{
			get { return Fields[fieldName]; }
		}

		private string _name;
		public  string  Name { get { return _name; } set { _name = value; } }

		private string _alias;
		public  string  Alias { get { return _alias; } set { _alias = value; } }

		private string _database;
		public  string  Database { get { return _database; } set { _database = value; } }

		private string _owner;
		public  string  Owner { get { return _owner; } set { _owner = value; } }

		private string _physicalName;
		public  string  PhysicalName { get { return _physicalName ?? _name; } set { _physicalName = value; } }

		private ChildContainer<Table,Field> _fields;
		public  ChildContainer<Table,Field>  Fields { get { return _fields; } }

		#endregion

		#region Protected Members

		private MappingSchema _mappingSchema;
		public  MappingSchema  MappingSchema
		{
			get { return _mappingSchema; }
		}

		#endregion
	}
}
