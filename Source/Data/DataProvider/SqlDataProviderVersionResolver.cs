using System;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	public class SqlDataProviderVersionResolver : DataProviderVersionResolverBase
	{
		private static SqlDataProviderVersionResolver _instance;

		protected SqlDataProviderVersionResolver()
			: base(typeof (SqlDataProviderBase))
		{
			DefaultProviderType = typeof (Sql2005DataProvider);
		}

		public static SqlDataProviderVersionResolver Instance
		{
			get { return _instance ?? (_instance = new SqlDataProviderVersionResolver()); }
		}

		public Type DefaultProviderType { get; set; }

		protected override DataProviderBase InvalidateDataProviderInternal(DataProviderBase dataProvider,
		                                                                   string connectionString)
		{
			Type type = DefaultProviderType;
			try
			{
				using (var connection =
					(SqlConnection) dataProvider.CreateConnectionObject())
				{
					connection.ConnectionString = connectionString;
					connection.Open();

					string serverVersion = connection.ServerVersion;
					if (serverVersion == null)
						return null;

					string[] serverVersionDetails = serverVersion.Split(new[] {"."},
					                                                    StringSplitOptions.RemoveEmptyEntries);

					int versionNumber = int.Parse(serverVersionDetails[0]);


					if (versionNumber >= 11)
						type = typeof (Sql2012DataProvider);
					else if (versionNumber == 10)
						type = typeof (Sql2008DataProvider);
					else if (versionNumber == 9)
						type = typeof (Sql2005DataProvider);
					else if (versionNumber == 8)
						type = typeof (Sql2000DataProvider);
				}
			}
			catch
			{
				return null;
			}

			return GetProviderByType(type);
		}
	}
}