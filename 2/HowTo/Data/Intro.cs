using System;
using NUnit.Framework;
using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class Intro
	{
		[Test]
		public void Test()
		{
			using (/*[a]*/DbManager db = new DbManager()/*[/a]*/)
			{
				string name = db
					./*[a]*/SetCommand/*[/a]*/(
						"SELECT FirstName FROM Person WHERE PersonID = @id",
						db./*[a]*/Parameter/*[/a]*/("@id", 1))
					./*[a]*/ExecuteScalar/*[/a]*/<string>();

				Console.WriteLine(name);
			}
		}
	}
}
