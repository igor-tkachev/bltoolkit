using System;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	public class SqlDataProviderVersionResolver : DataProviderVersionResolverBase
	{
		const int Sql2000 = 80;
		const int Sql2005 = 90;
		const int Sql2008 = 100;
		const int Sql2012 = 110;
		const int Sql2014 = 120;

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

		protected override DataProviderBase InvalidateDataProviderInternal(DataProviderBase dataProvider, string configuration, string connectionString)
		{
			Type type = DefaultProviderType;
			try
			{
				int? configVersion      = GetVersionFromConfiguration(configuration);
				int? serverVersion      = null;
				int? compatibilityLevel = null;
				int? targetVersion      = null;

				using (var connection = (SqlConnection) dataProvider.CreateConnectionObject())
				{
					connection.ConnectionString = connectionString;
					connection.Open();

					serverVersion = GetVersionFromConnection(connection);
					if (serverVersion >= Sql2005)
						compatibilityLevel = GetDatabaseCompatibilityLevel(connection);

				}

				targetVersion = compatibilityLevel ?? configVersion ?? serverVersion;

				if (targetVersion >= Sql2012)
					type = typeof(Sql2012DataProvider);
				else if (targetVersion == Sql2008)
					type = typeof(Sql2008DataProvider);
				else if (targetVersion == Sql2005)
					type = typeof(Sql2005DataProvider);
				else if (targetVersion == Sql2000)
					type = typeof(Sql2000DataProvider);
				
			}
			catch
			{
				return null;
			}

			return GetProviderByType(type);
		}

		private static int? GetDatabaseCompatibilityLevel(SqlConnection connection)
		{
			try
			{
				var cmd = connection.CreateCommand();
				cmd.CommandText = "select [compatibility_level] from sys.databases where [name] = @dbName";
				cmd.Parameters.AddWithValue("@dbName", connection.Database);
				return Common.Convert.ToNullableInt32(cmd.ExecuteScalar());
			}
			catch { return null; }
		}

		private static int? GetVersionFromConnection(SqlConnection connection)
		{
			string serverVersion = connection.ServerVersion;
			if (serverVersion == null)
				return null;

			string[] serverVersionDetails = serverVersion.Split(new[] { "." },
																StringSplitOptions.RemoveEmptyEntries);

			int versionNumber;
			if (serverVersionDetails.Length > 0 && int.TryParse(serverVersionDetails[0], out versionNumber))
				return versionNumber * 10;

			return null;
		}

		private static int? GetVersionFromConfiguration(string configuration)
		{
			if (configuration.Contains("2000"))
				return Sql2000;

			if (configuration.Contains("2005"))
				return Sql2005;

			if (configuration.Contains("2008"))
				return Sql2008;

			if (configuration.Contains("2012"))
				return Sql2012;

			if (configuration.Contains("2014"))
				return Sql2014;

			return null;
		}
	}
}