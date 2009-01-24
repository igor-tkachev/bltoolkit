using System;
using System.Text;

namespace BLToolkit.Data.SqlBuilder.SqlProvider
{
	public class BasicSqlProvider : ISqlProvider
	{
		#region ISqlProvider Members

		public string BuildSql(Sql sql)
		{
			StringBuilder sb = new StringBuilder();



			return sb.ToString();
		}

		#endregion
	}
}
