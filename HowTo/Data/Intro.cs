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
			using (/*[b]*/DbManager db = new DbManager()/*[/b]*/)
			{
				string name = db
					./*[b]*/SetCommand/*[/b]*/(
						"SELECT FirstName FROM Person WHERE PersonID = @id",
						db./*[b]*/Parameter/*[/b]*/("@id", 1))
					./*[b]*/ExecuteScalar/*[/b]*/<string>();

				Console.WriteLine(name);
			}
		}
	}
}
