using System;

using BLToolkit.DataAccess;

namespace DataAccess
{
	public class TestQueryAttribute : SqlQueryAttribute
	{
		public TestQueryAttribute()
		{
			IsDynamic = true;
		}

		private string _oracleText;
		public  string  OracleText
		{
			get { return _oracleText;  }
			set { _oracleText = value; }
		}

		private string _fbText;
		public  string  FbText
		{
			get { return _fbText;  }
			set { _fbText = value; }
		}

		private string _SQLiteText;
		public  string  SQLiteText
		{
			get { return _SQLiteText;  }
			set { _SQLiteText = value; }
		}

		public override string GetSqlText(DataAccessor accessor, BLToolkit.Data.DbManager dbManager)
		{
			switch (dbManager.DataProvider.Name)
			{
				case "Sql"   :
				case "Access": return SqlText;
				case "Oracle":
				case "ODP"   : return OracleText ?? SqlText;
				case "Fdp"   : return FbText     ?? SqlText;
				case "SQLite": return SQLiteText ?? SqlText;
			}

			throw new ApplicationException(string.Format("Unknown data provider '{0}'", dbManager.DataProvider.Name));
		}
	}
}
