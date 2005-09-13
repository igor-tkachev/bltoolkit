using System;
using System.Collections;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace CS.DbManagerTest
{
	[TestFixture]
	public class SqlServer : CS.DbManagerTest.Test
	{
		public override string ConfigurationString 
		{
			get { return "SqlServer"; }
		}

		public override void SetCommand_CommandType_TableDirect_ExecuteNonQuery() {}

		public class testID
		{
			public int ID;

			public testID(int ID)
			{
				this.ID = ID;
			}
		}

		[Test]
		public void CreateParameters()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				IDbDataParameter[] pars = db.CreateParameters(new testID(51));

				Console.WriteLine(pars[0].Value);

				Assert.IsTrue((int)pars[0].Value == 51);
			}
		}

		public class TestGuid
		{
			public Guid Guid;
		}

		[Test]
		public void GuidTest()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				TestGuid guid = (TestGuid)db
					.SetCommand("SELECT NewID() as Guid")
					.ExecuteObject(typeof(TestGuid));

				Console.WriteLine(guid.Guid);

				Assert.IsTrue(guid.Guid != Guid.Empty);
			}
		}

		[Test]
		public void OutputParameter()
		{
			using (DbManager db = new DbManager(ConfigurationString))
			{
				db.SetCommand(@"
					if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Test]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
					begin
						drop procedure [dbo].[Test]
					end").ExecuteNonQuery();

				db.SetCommand(@"
					CREATE PROCEDURE dbo.Test
						@p1 int,
						@p2 int,
						@po int output
					AS

					SET @po = @p1 * @p2").ExecuteNonQuery();

				db.SetCommand(@"
					GRANT EXEC ON dbo.Test TO PUBLIC").ExecuteNonQuery();

				db
					.SetSpCommand(
						"Test",
						db.Parameter("@p1", 2),
						db.Parameter("@p2", 2),
						db.OutputParameter("@po", 0))
					.ExecuteNonQuery();

				object o = db.Parameter("@po").Value;

                Console.WriteLine(o);

				Assert.AreEqual(4, o);
			}
		}

		#region Execute

		public class ExecuteTestObject
		{
			public ExecuteTestObject(string s)
			{
				Len = s.Length;
				Str = s;
			}

			public int    Len;
			public string Str;
		}

		[Test]
		public virtual void Execute_IList()
		{
			ArrayList list = new ArrayList(2);

			list.Add(new ExecuteTestObject("12345"));
			list.Add(new ExecuteTestObject("1234567890"));

			using (DbManager db = new DbManager(ConfigurationString))
			{
				db
					.SetSpCommand("Length_Test")
					.Execute(list);
			}
		}

		#endregion
	}
}
