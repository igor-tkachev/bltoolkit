using System;
using System.Data;
using System.Data.Common;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;

namespace BLToolkit.Fluent.Test.MockDataBase
{
	/// <summary>
	/// For BLToolkit
	/// </summary>
	public class MockDataProvider : DataProviderBase
	{
		public override IDbConnection CreateConnectionObject()
		{
			throw new NotImplementedException();
		}

		public override DbDataAdapter CreateDataAdapterObject()
		{
			throw new NotImplementedException();
		}

		public override bool DeriveParameters(IDbCommand command)
		{
			throw new NotImplementedException();
		}

		public override ISqlProvider CreateSqlProvider()
		{
			return new MockSqlProvider();
		}

		public override Type ConnectionType
		{
			get { return typeof(MockDb); }
		}

		public override string Name
		{
			get { return "Mock"; }
		}
	}
}