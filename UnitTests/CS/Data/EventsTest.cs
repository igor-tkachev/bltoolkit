using System;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class EventsTest
	{
		[Test]
		public void BeforeAfterInitTest()
		{
			using (DbManager db = new DbManager())
			{
				db.BeforeOperation += HandleBeforeOperation;
				db.AfterOperation  += HandleAfterOperation;
				db.InitCommand     += HandleInitCommand;

				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDataSet();
			}
		}

		private static void HandleBeforeOperation(object sender, OperationTypeEventArgs ea)
		{
			Console.WriteLine("Before operation: " + ea.Operation);
		}

		private static void HandleAfterOperation(object sender, OperationTypeEventArgs ea)
		{
			Console.WriteLine("After operation:  " + ea.Operation);
		}

		private static void HandleInitCommand(object sender, InitCommandEventArgs ea)
		{
			Console.WriteLine("Init command:  " + ea.Command.CommandText);
		}

		[Test, ExpectedException(typeof(DataException))]
		public void ExceptionTest()
		{
			using (DbManager db = new DbManager())
			{
				db.OperationException += HandleOperationException;

				db
					.SetCommand("SELECT * FROM NoSuchTableEverExist")
					.ExecuteDataSet();
			}
		}

		private static void HandleOperationException(object sender, OperationExceptionEventArgs ea)
		{
			Assert.That(ea, Is.Not.Null);
			Assert.That(ea.Exception, Is.Not.Null);
			Assert.That(ea.Exception.Number, Is.Not.Null);

			Console.WriteLine("Operation:    " + ea.Operation);
			Console.WriteLine("Error number: " + ea.Exception.Number);
			Console.WriteLine("Exception:    " + ea.Exception.GetBaseException().Message);
		}
	}
}
