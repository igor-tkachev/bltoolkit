using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DirectionTest
	{
		public abstract class PersonAccessor : DataAccessor
		{
			[SprocName("Person_Insert_OutputParameter")]
			public abstract void Insert_OutputParameter([/*[a]*/Direction.Output/*[/a]*/("PERSONID")] Person p);

			[SprocName("Scalar_ReturnParameter")]
			public abstract void Insert_ReturnParameter(
				[/*[a]*/Direction.ReturnValue/*[/a]*/("@PersonID"),
				 /*[a]*/Direction.Ignore/*[/a]*/("PersonID", "FirstName", "LastName", "MiddleName", "Gender")] Person p);

			[SprocName("Scalar_ReturnParameter")]
			public abstract void Insert_ReturnParameter2(
				[/*[a]*/Direction.ReturnValue/*[/a]*/("ID"),
				 /*[a]*/Direction.Ignore/*[/a]*/("PersonID", "FirstName", "LastName", "MiddleName", "Gender")] Person p);
		}

		PersonAccessor Accessor
		{
			get { return DataAccessor.CreateInstance<PersonAccessor>(); }
		}

		[Test]
		public void TestOutputParameter()
		{
			Person p = new Person { FirstName = "Crazy", LastName = "Frog", Gender = Gender.Other };

			Accessor.Insert_OutputParameter(p);

			Assert.IsTrue(p.ID > 0);

			new SprocQuery().Delete(p);
		}

		[Test]
		public void TestReturnParameter()
		{
			Person p = new Person();

			Accessor.Insert_ReturnParameter(p);

			Assert.AreEqual(12345, p.ID);
		}

		[Test]
		public void TestReturnParameter2()
		{
			Person p = new Person();

			Accessor.Insert_ReturnParameter2(p);

			Assert.AreEqual(12345, p.ID);
		}
	}
}
