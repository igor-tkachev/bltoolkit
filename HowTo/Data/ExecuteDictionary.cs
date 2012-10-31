using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Mapping;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ExecuteDictionary
	{
		// The dictionary key is built from an object field/property.
		//
		Dictionary<int, Person> GetPersonDictionary1()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteDictionary/*[/a]*/<int, Person>(/*[a]*/"ID"/*[/a]*/);
			}
		}

		[Test]
		public void Test1()
		{
			Dictionary<int, Person> dic = GetPersonDictionary1();

			Assert.AreNotEqual(0, dic.Count);
		}

		// The dictionary key is built from a recordset field value ('@' prefix).
		//
		Dictionary<int, Person> GetPersonDictionary2()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteDictionary/*[/a]*/<int, Person>(/*[a]*/"@PersonID"/*[/a]*/);
			}
		}

		[Test]
		public void Test2()
		{
			Dictionary<int, Person> dic = GetPersonDictionary2();

			Assert.AreNotEqual(0, dic.Count);
		}

		// Complex dictionary key.
		//
		Dictionary</*[a]*/CompoundValue/*[/a]*/, Person> GetPersonDictionary3()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteDictionary/*[/a]*/<Person>(new /*[a]*/MapIndex("FirstName", "LastName")/*[/a]*/);
			}
		}

		[Test]
		public void Test3()
		{
			Dictionary</*[a]*/CompoundValue/*[/a]*/, Person> dic = GetPersonDictionary3();

			Assert.AreNotEqual(0, dic.Count);
		}
	}
}
