using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	abstract class ParseInfoBase : IParseInfo
	{
		protected ParseInfoBase(IParseInfo parseInfo, Expression expression)
		{
			SqlQuery   = parseInfo.SqlQuery;
			Parser     = parseInfo.Parser;
			Expression = expression;
		}

		public SqlQuery         SqlQuery   { get; set; }
		public ExpressionParser Parser     { get; set; }
		public Expression       Expression { get; set; }

		public abstract Expression                  BuildExpression(IParseInfo rootParse, Expression expression, int level);
		public abstract IEnumerable<ISqlExpression> ConvertToSql   (Expression expression, int level, ConvertFlags flags);
		public abstract IEnumerable<int>            ConvertToIndex (Expression expression, int level, ConvertFlags flags);

		public virtual void SetAlias(string alias)
		{
		}
	}
}
