using System;
using System.Text;

namespace BLToolkit.Data.Sql.SqlProvider
{
	public class BasicSqlProvider : ISqlProvider
	{
		#region ISqlProvider Members

		public string BuildSql(SqlBuilder sqlBuilder)
		{
			StringBuilder sb = new StringBuilder();



			return sb.ToString();
		}

		#endregion
	}
}
