using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ExecuteList
	{
		public abstract class /*[a]*/PersonAccessor/*[/a]*/ : /*[a]*/DataAccessor/*[/a]*/
		{
			// This method reads a list of Person objects.
			//
			[ActionName("SelectAll")]
			public abstract /*[a]*/List<Person>/*[/a]*/ GetPersonList1();

			// Here we help the method to get object type information.
			// /*[a]*/ObjectTypeAttribute/*[/a]*/ can be applied to the class itself.
			// In this case there is no need to specify object type for each method.
			// Another way to specify object type is a generic parameter
			// of the DataAccessor<T> class.
			//
			[SqlQuery("SELECT * FROM Person")]
			[/*[a]*/ObjectType(typeof(Person))/*[/a]*/]
			public abstract /*[a]*/ArrayList/*[/a]*/ GetPersonList2();

			// This method reads a list of scalar values.
			//
			[SqlQuery("SELECT PersonID FROM Person")]
			public abstract /*[a]*/List<int>/*[/a]*/ GetPersonIDList();
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// ExecuteList.
			//
			IList list;
			
			list = pa.GetPersonList1();
			list = pa.GetPersonList2();

			foreach (Person p in list)
				Console.WriteLine("{0}: {1} {2}", p.ID, p.FirstName, p.LastName);

			// ExecuteScalarList.
			//
			List<int> slist = pa.GetPersonIDList();

			foreach (int id in slist)
				Console.WriteLine("{0}", id);
		}
	}
}
