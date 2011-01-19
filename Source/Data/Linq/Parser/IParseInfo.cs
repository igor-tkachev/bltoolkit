using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	interface IParseInfo
	{
		SqlQuery         SqlQuery   { get; }
		ExpressionParser Parser     { get; }

		Expression                  BuildExpression(IParseInfo rootParse, Expression expression, int level);
		IEnumerable<ISqlExpression> ConvertToSql   (Expression expression, int level, ConvertFlags flags);
		IEnumerable<int>            ConvertToIndex (Expression expression, int level, ConvertFlags flags);
		void                        SetAlias       (string alias);
	}
}
