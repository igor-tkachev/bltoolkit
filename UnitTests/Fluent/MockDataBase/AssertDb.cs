using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	public class AssertDb
	{
		private readonly MockDb _db;

		public AssertDb(MockDb db)
		{
			_db = db;
		}

		public void AreAll(string message = null)
		{
			if (_db.Commands.Any(c => !c.IsUsing))
			{
				Assert.Fail(message ?? "Fail all queries");
			}
		}
	}
}