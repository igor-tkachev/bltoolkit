using System;

namespace BLToolkit.ServiceModel
{
	using Data.Sql;

	public class LinqServiceQuery
	{
		public SqlQuery       Query      { get; set; }
		public SqlParameter[] Parameters { get; set; }
	}
}
