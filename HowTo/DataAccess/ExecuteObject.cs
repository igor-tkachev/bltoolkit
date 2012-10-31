using System;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ExecuteObject
	{
		public abstract class /*[a]*/PersonAccessor/*[/a]*/ : /*[a]*/DataAccessor/*[/a]*/<Person>
		{
			// Here we explicitly specify a stored procedure name.
			//
			[SprocName("Person_SelectByKey")]
			public abstract /*[a]*/Person/*[/a]*/ GetByID(int @id);

			// SQL query text.
			//
			[SqlQuery("SELECT * FROM Person WHERE PersonID = @id")]
			public abstract /*[a]*/Person/*[/a]*/ GetPersonByID(int @id);

			// Specify action name.
			// Stored procedure name is generated based on convention
			// defined by DataAccessor.GetDefaultSpName method.
			//
			[ActionName("SelectByName")]
			public abstract /*[a]*/Person/*[/a]*/ GetPersonByName(string @firstName, string @lastName);

			// By default method name defines an action name
			// which is converted to a stored procedure name.
			// Default conversion rule is ObjectName_MethodName.
			// This method calls the Person_SelectByName stored procedure.
			//
			public abstract /*[a]*/Person/*[/a]*/ SelectByName(string @firstName, string @lastName);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// ExecuteObject.
			//
			Assert.IsNotNull(pa.GetByID        (1));
			Assert.IsNotNull(pa.GetPersonByID  (2));
			Assert.IsNotNull(pa.GetPersonByName("Tester", "Testerson"));
			Assert.IsNotNull(pa.SelectByName   ("Tester", "Testerson"));
		}
	}
}
