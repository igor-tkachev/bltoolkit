using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ExecuteScalarDictionary
	{
		Dictionary<int, string> GetNameDictionary()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteScalarDictionary/*[/a]*/<int, string>(/*[a]*/"PersonID"/*[/a]*/, /*[a]*/"FirstName"/*[/a]*/);
			}
		}

		[Test]
		public void Test()
		{
			Dictionary<int, string> dic = GetNameDictionary();

			Assert.AreNotEqual(0, dic.Count);
			Assert.IsNotNull(dic[1]);
		}
	}
}
