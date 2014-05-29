using System;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	public sealed class SqlDataProvider : SqlDataProviderBase
	{
		public static SqlBulkCopyOptions SqlBulkCopyOptions { get; set; }
	}
}
