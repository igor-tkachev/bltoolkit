using System;

namespace BLToolkit.Data.Linq
{
	using Data.Sql.SqlProvider;

	public static class Settings
	{
		public static string                    GetDefaultContextID         = DbManager.GetDataProvider(DbManager.DefaultConfiguration).Name;
		public static Func<ISqlProvider>        GetDefaultCreateSqlProvider = DbManager.GetDataProvider(DbManager.DefaultConfiguration).CreateSqlProvider;
		public static Func<string,IDataContext> CreateDefaultDataContext    = config => new DbManager(config ?? DbManager.DefaultConfiguration);
	}
}
