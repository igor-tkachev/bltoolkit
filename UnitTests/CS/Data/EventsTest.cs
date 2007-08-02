using System;
using BLToolkit.Data;
using NUnit.Framework;

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
				db.BeforeOperation += new OperationTypeEventHandler(HandleBeforeOperation);
				db.AfterOperation  += new OperationTypeEventHandler(HandleAfterOperation);
				db.InitCommand     += new InitCommandEventHandler  (HandleInitCommand);

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
				db.OperationException += new OperationExceptionEventHandler(HandleOperationException);

				db
					.SetCommand("SELECT * FROM NoSuchTableEverExist")
					.ExecuteDataSet();
			}
		}

		private static void HandleOperationException(object sender, OperationExceptionEventArgs ea)
		{
			Console.WriteLine("Operation:  " + ea.Operation);
			Console.WriteLine("Exception:  " + ea.Exception.GetBaseException().Message);
		}
	}
}
