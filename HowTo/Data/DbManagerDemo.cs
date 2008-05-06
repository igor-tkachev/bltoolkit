using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace HowTo.Data
{
	[TestFixture]
	public class DbManagerDemo
	{
		// The MapValue attribute is used by BLToolkit.
		//
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		// Business object. Here we use C# 3.0 automatic properties,
		// however it can be public fields, regular or abstract properties.
		// The MapField attribute is used by BLToolkit to associate a database field
		// with a business object property if they have different names.
		//
		public class Person
		{
			[MapField("PersonID")]
			public int    ID         { get; set; }
			public string FirstName  { get; set; }
			public string MiddleName { get; set; }
			public string LastName   { get; set; }
			public /*[a]*/Gender/*[/a]*/ Gender     { get; set; }
		}

		// BLToolkit data access method.
		//
		public List<Person> /*[a]*/GetList/*[/a]*/(Gender gender)
		{
			/*[a]*/using/*[/a]*/ (/*[a]*/DbManager/*[/a]*/ db = new DbManager(/*[a]*/"DemoConnection"/*[/a]*/))
			{
				return db
					./*[a]*/SetCommand/*[/a]*/(
						"SELECT * FROM Person WHERE Gender = @gender",
						db./*[a]*/Parameter/*[/a]*/("@gender", /*[a]*/Map.EnumToValue/*[/a]*/(gender)))
					./*[a]*/ExecuteList/*[/a]*/<Person>();
			}
		}

		[Test]
		public void Test()
		{
			List<Person> list = GetList(Gender.Male);
			Assert.Greater(list.Count, 0);
		}
	}
}
