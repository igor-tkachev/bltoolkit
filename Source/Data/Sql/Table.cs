using System;
using System.Collections.Generic;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.Data.Sql
{
	public class Table : ITableSource
	{
		#region Init

		public Table() : this(Map.DefaultSchema)
		{
		}

		public Table(MappingSchema mappingSchema)
		{
			if (mappingSchema == null) throw new ArgumentNullException("mappingSchema");

			_mappingSchema = mappingSchema;

			_fields = new ChildContainer<Table,Field>(this);
			_all    = new Field("*", "*");

			((IChild<Table>)_all).Parent = this;
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

			foreach (Join join in table.Joins)
				Joins.Add(join.Clone());
		}

		public Table(Table table)
			: this(table.MappingSchema, table)
		{
		}

		#endregion

		#region Init from XML

		public Table(ExtensionList extensions, string name)
			: this(Map.DefaultSchema, extensions, name)
		{
		}

		public Table(MappingSchema mappingSchema, ExtensionList extensions, string name)
			: this(mappingSchema)
		{
			if (extensions == null) throw new ArgumentNullException("extensions");
			if (name       == null) throw new ArgumentNullException("name");

			TypeExtension te = extensions[name];

			if (te == TypeExtension.Null)
				throw new ArgumentException(string.Format("Table '{0}' not found.", name));

			_name         = te.Name;
			_alias        = (string)te.Attributes["Alias"].       Value;
			_database     = (string)te.Attributes["Database"].    Value;
			_owner        = (string)te.Attributes["Owner"].       Value;
			_physicalName = (string)te.Attributes["PhysicalName"].Value;

			foreach (MemberExtension me in te.Members)
				Fields.Add(new Field(me.Name, (string)me["MapField"].Value ?? (string)me["PhysicalName"].Value));

			foreach (AttributeExtension ae in te.Attributes["Join"])
				Joins.Add(new Join(ae));

			string baseExtension = (string)te.Attributes["BaseExtension"].Value;

			if (!string.IsNullOrEmpty(baseExtension))
				InitFromBase(new Table(mappingSchema, extensions, baseExtension));

			string baseTypeName = (string)te.Attributes["BaseType"].Value;

			if (!string.IsNullOrEmpty(baseTypeName))
				InitFromBase(new Table(mappingSchema, Type.GetType(baseTypeName, true, true)));
		}

		void InitFromBase(Table baseTable)
		{
			if (_alias        == null) _alias        = baseTable._alias;
			if (_database     == null) _database     = baseTable._database;
			if (_owner        == null) _owner        = baseTable._owner;
			if (_physicalName == null) _physicalName = baseTable._physicalName;

			foreach (Field field in baseTable.Fields.Values)
				if (!Fields.ContainsKey(field.Name))
					Fields.Add(new Field(field.Name, field.PhysicalName));

			foreach (Join join in baseTable.Joins)
				if (Joins.Find(delegate(Join j) { return j.TableName == join.TableName; }) == null)
					Joins.Add(join);
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

		private List<Join> _joins = new List<Join>();
		public  List<Join>  Joins { get { return _joins; } }

		private Field _all;
		public  Field  All { get { return _all; } }

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
