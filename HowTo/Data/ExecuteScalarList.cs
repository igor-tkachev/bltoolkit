using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ExecuteScalarList
	{
		List<string> GetNameList1()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT FirstName FROM Person")
					./*[a]*/ExecuteScalarList/*[/a]*/<string>();
			}
		}

		[Test]
		public void Test1()
		{
			List<string> list = GetNameList1();

			Assert.AreNotEqual(0, list.Count);
			Assert.IsNotNull(list[0]);
		}

		List<string> GetNameList2()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteScalarList/*[/a]*/<string>(/*[a]*/"FirstName"/*[/a]*/);
			}
		}

		[Test]
		public void Test2()
		{
			List<string> list = GetNameList2();

			Assert.AreNotEqual(0, list.Count);
			Assert.IsNotNull(list[0]);
		}
	}
}
