using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public interface IParseContext
	{
		ExpressionParser Parser     { get; }
		Expression       Expression { get; }
		SqlQuery         SqlQuery   { get; }
		IParseContext    Parent     { get; set; }

		void             BuildQuery<T>       (Query<T> query, ParameterExpression queryParameter);
		Expression       BuildExpression     (Expression expression, int level);
		ISqlExpression[] ConvertToSql        (Expression expression, int level, ConvertFlags flags);
		int[]            ConvertToIndex      (Expression expression, int level, ConvertFlags flags);
		bool             IsExpression        (Expression expression, int level, RequestFor requestFlag);
		IParseContext    GetContext          (Expression expression, int level, SqlQuery currentSql);
		int              ConvertToParentIndex(int index, IParseContext context);
		void             SetAlias            (string alias);
	}

	public static class ParserContextExtensions
	{
		public static Expression BuildExpression(this IParseContext context)
		{
			return context.BuildExpression(null, 0);
		}

		public static Expression BuildExpression(this IParseContext context, Expression expression)
		{
			return context.BuildExpression(expression, 0);
		}

		public static ISqlExpression[] ConvertToSql(this IParseContext context, ConvertFlags flags)
		{
			return context.ConvertToSql(null, 0, flags);
		}

		public static ISqlExpression[] ConvertToSql(this IParseContext context, Expression expression, ConvertFlags flags)
		{
			return context.ConvertToSql(expression, 0, flags);
		}

		public static int[] ConvertToIndex(this IParseContext context, ConvertFlags flags)
		{
			return context.ConvertToIndex(null, 0, flags);
		}

		public static int[] ConvertToIndex(this IParseContext context, Expression expression, ConvertFlags flags)
		{
			return context.ConvertToIndex(expression, 0, flags);
		}

		public static bool IsExpression(this IParseContext context, RequestFor requestFlag)
		{
			return context.IsExpression(null, 0, requestFlag);
		}

		public static bool IsExpression(this IParseContext context, Expression expression, RequestFor requestFlag)
		{
			return context.IsExpression(expression, 0, requestFlag);
		}

		public static IParseContext GetContext(this IParseContext context, SqlQuery currentSql)
		{
			return context.GetContext(null, 0, currentSql);
		}

		public static IParseContext GetContext(this IParseContext context, Expression expression, SqlQuery currentSql)
		{
			return context.GetContext(expression, 0, currentSql);
		}
	}
}
