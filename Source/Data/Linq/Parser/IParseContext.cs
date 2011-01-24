using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	interface IParseContext
	{
		ExpressionParser Parser     { get; }
		Expression       Expression { get; }
		SqlQuery         SqlQuery   { get; }
		IParseContext    Root       { get; set; }

		Expression       BuildQuery     ();
		Expression       BuildExpression(Expression expression, int level);
		ISqlExpression[] ConvertToSql   (Expression expression, int level, ConvertFlags flags);
		int[]            ConvertToIndex (Expression expression, int level, ConvertFlags flags);
		bool             IsExpression   (Expression expression, int level, RequestFor requestFlag);
		IParseContext    GetContext     (Expression expression, int level, SqlQuery currentSql);
		void             SetAlias       (string alias);
	}
}
