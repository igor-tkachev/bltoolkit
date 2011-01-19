using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SubQueryInfo : ParseInfoBase
	{
		IParseInfo _baseQuery;

		public SubQueryInfo(IParseInfo baseQuery, Expression expression) : base(baseQuery, expression)
		{
			_baseQuery = baseQuery;

			SqlQuery = new SqlQuery { ParentSql = SqlQuery.ParentSql };
		}

		public override Expression BuildExpression(IParseInfo rootParse, Expression expression, int level)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<ISqlExpression> ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<int> ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}
	}
}
