using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ExecuteDictionary1
	{
		public class Person
		{
			[MapField("PersonID"), /*[a]*/PrimaryKey/*[/a]*/]
			public int    /*[a]*/ID/*[/a]*/;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public abstract class /*[a]*/PersonAccessor/*[/a]*/ : /*[a]*/DataAccessor/*[/a]*/<Person>
		{
			// This method uses Person class primary key information.
			//
			[ActionName("SelectAll")]
			public abstract /*[a]*/Dictionary<int,Person>/*[/a]*/ GetPersonDictionary1();

			// Define index field explicitly. "ID" is a field name of the Person class.
			//
			[ActionName("SelectAll")]
			[/*[a]*/Index("ID")/*[/a]*/]
			public abstract /*[a]*/Dictionary<int,Person>/*[/a]*/ GetPersonDictionary2();

			// Define index field explicitly. "@PersonID" is a recordset field.
			// Note that the '@' symbol enforces the library to read index value
			// from recordset (not from object).
			//
			[ActionName("SelectAll")]
			[/*[a]*/Index("@PersonID")/*[/a]*/]
			public abstract /*[a]*/Dictionary<int,Person>/*[/a]*/ GetPersonDictionary3();

			// This method reads a dictionary containing scalar values.
			//
			[SqlQuery("SELECT PersonID, FirstName FROM Person")]
			[/*[a]*/Index("PersonID")/*[/a]*/]
			[/*[a]*/ScalarFieldName("FirstName")/*[/a]*/]
			public abstract /*[a]*/Dictionary<int,string>/*[/a]*/ GetPersonNameDictionary();
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// ExecuteDictionary.
			//
			Dictionary<int,Person> dic;

			dic = pa.GetPersonDictionary1();
			dic = pa.GetPersonDictionary2();
			dic = pa.GetPersonDictionary3();

			foreach (int id in dic.Keys)
				Console.WriteLine("{0}: {1} {2}", id, dic[id].FirstName, dic[id].LastName);

			// ExecuteScalarDictionary.
			//
			Dictionary<int,string> sdic = pa.GetPersonNameDictionary();

			foreach (int id in dic.Keys)
				Console.WriteLine("{0}: {1}", id, sdic[id]);
		}
	}

	[TestFixture]
	public class ExecuteDictionary2
	{
		// This example demonstrates how to use a multiple field key.
		//
		public class Person
		{
			[/*[a]*/PrimaryKey(1)/*[/a]*/, MapField("PersonID")]
			public int    ID;
			[/*[a]*/PrimaryKey(2)/*[/a]*/]
			public string LastName;

			public string FirstName;
			public string MiddleName;
		}

		public abstract class /*[a]*/PersonAccessor/*[/a]*/ : /*[a]*/DataAccessor/*[/a]*/<Person>
		{
			// This method uses Person class primary key information.
			// Note that the key type of the dictionary must be of /*[a]*/IndexValue/*[/a]*/ type.
			// It is required if the index consists of more than one element.
			//
			[ActionName("SelectAll")]
			public abstract /*[a]*/Dictionary<CompoundValue,Person>/*[/a]*/ GetPersonDictionary();

			// This method reads a dictionary containing scalar values.
			//
			[SqlQuery("SELECT PersonID, LastName, FirstName FROM Person")]
			[/*[a]*/Index("PersonID", "LastName")/*[/a]*/]
			[/*[a]*/ScalarFieldName("FirstName")/*[/a]*/]
			public abstract /*[a]*/Dictionary<CompoundValue,string>/*[/a]*/ GetPersonNameDictionary();
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// ExecuteDictionary.
			//
			Dictionary<CompoundValue,Person> dic = pa.GetPersonDictionary();

			foreach (CompoundValue idx in dic.Keys)
				Console.WriteLine("{0}: {1} {2}", dic[idx].ID, dic[idx].FirstName, dic[idx].LastName);

			// ExecuteScalarDictionary.
			//
			Dictionary<CompoundValue,string> sdic = pa.GetPersonNameDictionary();

			string firstName = sdic[new CompoundValue(2, "Testerson")];

			Assert.AreEqual("Tester", firstName);
		}
	}
}
