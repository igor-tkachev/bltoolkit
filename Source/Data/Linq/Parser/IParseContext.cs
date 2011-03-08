using System;
using System.Linq.Expressions;

#if DEBUG
#pragma warning disable 3010
#endif

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public interface IParseContext
	{
#if DEBUG
// ReSharper disable InconsistentNaming
		[CLSCompliant(false)]
		string _sqlQueryText { get; }
// ReSharper restore InconsistentNaming
#endif

		ExpressionParser Parser     { get; }
		Expression       Expression { get; }
		SqlQuery         SqlQuery   { get; set; }
		IParseContext    Parent     { get; set; }

		void             BuildQuery<T>       (Query<T> query, ParameterExpression queryParameter);
		Expression       BuildExpression     (Expression expression, int level);
		SqlInfo[]        ConvertToSql        (Expression expression, int level, ConvertFlags flags);
		SqlInfo[]        ConvertToIndex      (Expression expression, int level, ConvertFlags flags);
		bool             IsExpression        (Expression expression, int level, RequestFor requestFlag);
		IParseContext    GetContext          (Expression expression, int level, ParseInfo parseInfo);
		int              ConvertToParentIndex(int index, IParseContext context);
		void             SetAlias            (string alias);
	}
}
