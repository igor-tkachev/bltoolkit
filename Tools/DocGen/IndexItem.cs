using System;
using System.Collections.Generic;

namespace DocGen
{
	class IndexItem
	{
		public IndexItem(string name)
		{
			Name = name;

			string[] ss = name.Split(' ');

			if (ss.Length > 1)
			{
				switch (ss[1])
				{
					case "attribute" :
					case "aspect"    : Text.Add("[" + ss[0]);
					                   Text.Add(", " + ss[0]);      break;
					case "method"    : Text.Add("." + ss[0] + "(");
					                   Text.Add("." + ss[0] + "<");
					                   Text.Add(" " + ss[0] + "(");
					                   Text.Add(" " + ss[0] + "<"); break;
					case "class"     :
					case "enum"      :
					case "component" : Text.Add("new " + ss[0]);
					                   Text.Add(": " + ss[0]);
					                   Text.Add("<" + ss[0] + ">");
					                   Text.Add("<" + ss[0] + ",");
					                   Text.Add(" " + ss[0] + ".");
					                   Text.Add(" " + ss[0] + "<");
					                   Text.Add("(" + ss[0] + ".");
					                   Text.Add("(" + ss[0] + "<"); break;
					case "property"  : Text.Add(ss[0] + " =");      break;
					default          : Text.Add(ss[0]);             break;
				}
			}
			else
			{
				Text.Add(ss[0]);
			}
		}

		public IndexItem(string name, params string[] text)
		{
			Name = name;
			Array.ForEach(text, Text.Add);
		}

		public string         Name;
		public List<string>   Text  = new List<string>();
		public List<FileItem> Files = new List<FileItem>();

		public static List<IndexItem> Index;

		static IndexItem()
		{
			Index = new List<IndexItem>
			{
				new IndexItem("Async aspect"),
				new IndexItem("Cache aspect"),
				new IndexItem("ClearCache aspect"),
				new IndexItem("NoCache attribute"),
				new IndexItem("Counter aspect"),
				new IndexItem("Logging aspect",         "[Log"),
				new IndexItem("Mixin aspect"),
				new IndexItem("NotNull attribute"),

				new IndexItem("ObjectBinder component", "ObjectBinder"),
				new IndexItem("DbManager component"),
				new IndexItem("DataProvider class",     "DataProvider"),
				new IndexItem("MapResultSet class"),
				new IndexItem("ScalarSourceType enum"),
				new IndexItem("AddRelation method"),
				new IndexItem("AddConnectionString method"),
				new IndexItem("AddDataProvider method"),
				new IndexItem("AssignParameterValues method"),
				new IndexItem("CreateParameters method"),
				new IndexItem("SetCommand method"),
				new IndexItem("SetSpCommand method"),
				new IndexItem("Parameter method"),
				new IndexItem("OutputParameter method"),
				new IndexItem("Prepare method"),
				new IndexItem("Close method"),
				new IndexItem("ExecuteDataSet method"),
				new IndexItem("ExecuteDataTable method"),
				new IndexItem("ExecuteDictionary method"),
				new IndexItem("ExecuteForEach method"),
				new IndexItem("ExecuteList method"),
				new IndexItem("ExecuteNonQuery method"),
				new IndexItem("ExecuteObject method"),
				new IndexItem("ExecuteReader method"),
				new IndexItem("ExecuteResultSet method"),
				new IndexItem("ExecuteScalar method"),
				new IndexItem("ExecuteScalarDictionary method"),
				new IndexItem("ExecuteScalarList method"),

				new IndexItem("DataAccessor class"),
				new IndexItem("Abstract accessors", "DataAccessor"),
				new IndexItem("SqlQuery class"),
				new IndexItem("SprocQuery class"),
				new IndexItem("BeginTransaction method"),
				new IndexItem("CommitTransaction method"),
				new IndexItem("CreateDbManager method"),
				new IndexItem("DataAccessor class"),
				new IndexItem("GetDefaultSpName method"),
				new IndexItem("GetTableName method"),
				new IndexItem("Insert method"),
				new IndexItem("PrepareSqlQuery method"),
				new IndexItem("SelectByKey method"),
				new IndexItem("SelectAll method"),
				new IndexItem("TableName attribute"),
				new IndexItem("ActionName attribute"),
				new IndexItem("ActionSprocName attribute"),
				new IndexItem("ActualType attribute"),
				new IndexItem("CommandBehavior attribute"),
				new IndexItem("Destination attribute"),
				new IndexItem("Direction attribute"),
				new IndexItem("DiscoverParameters attribute"),
				new IndexItem("DataSetTable  attribute"),
				new IndexItem("Format attribute"),
				new IndexItem("Index attribute"),
				new IndexItem("ObjectType attribute"),
				new IndexItem("NonUpdatable attribute"),
				new IndexItem("ParamDbType attribute"),
				new IndexItem("ParamName attribute"),
				new IndexItem("ParamNullValue attribute"),
				new IndexItem("ParamSize attribute"),
				new IndexItem("PrimaryKey attribute"),
				new IndexItem("ScalarFieldName attribute"),
				new IndexItem("ScalarSource attribute"),
				new IndexItem("SprocName attribute"),
				new IndexItem("TableName attribute"),

				new IndexItem("EditableObject class"),

				new IndexItem("IsDynamic property"),
				new IndexItem("Extensions property"),

				new IndexItem("CompoundValue class"),
				new IndexItem("TypeAccessor class"),
				new IndexItem("TypeExtension class"),
				new IndexItem("CreateInstance method"),
				new IndexItem("GetExtensions method")
			};
		}
	}
}
