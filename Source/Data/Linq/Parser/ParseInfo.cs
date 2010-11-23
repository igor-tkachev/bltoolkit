using System;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	abstract class ParseInfo
	{
		public SqlQuery         SqlQuery;
		public ExpressionParser Parser;

		protected ParseInfo(ExpressionParser parser)
		{
			Parser   = parser;
			SqlQuery = parser.SqlQuery;
		}

		public abstract Query<T> BuildQuery<T>();

		public virtual void SetAlias(string alias)
		{
		}
	}
}
