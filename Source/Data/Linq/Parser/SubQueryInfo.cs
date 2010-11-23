using System;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SubQueryInfo : ParseInfo
	{
		ParseInfo _baseQuery;

		public SubQueryInfo(ParseInfo baseQuery) : base(baseQuery.Parser)
		{
			_baseQuery = baseQuery;

			SqlQuery = baseQuery.SqlQuery = new SqlQuery { ParentSql = SqlQuery.ParentSql };
		}

		public override Query<T> BuildQuery<T>()
		{
			throw new NotImplementedException();
		}
	}
}
