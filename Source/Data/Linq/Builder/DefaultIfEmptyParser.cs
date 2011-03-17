using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class DefaultIfEmptyParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("DefaultIfEmpty");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence     = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));
			var defaultValue = methodCall.Arguments.Count == 1 ? null : methodCall.Arguments[1].Unwrap();

			if (parseInfo.Parent is SelectManyParser.SelectManyContext)
			{
				var groupJoin = ((SelectManyParser.SelectManyContext)parseInfo.Parent).Sequence[0] as JoinParser.GroupJoinContext;

				if (groupJoin != null)
				{
					groupJoin.SqlQuery.From.Tables[0].Joins[0].JoinType = SqlQuery.JoinType.Left;
					groupJoin.SqlQuery.From.Tables[0].Joins[0].IsWeak   = false;
				}
			}

			return new DefaultIfEmptyContext(parseInfo.Parent, sequence, defaultValue);
		}

		public class DefaultIfEmptyContext : SequenceContextBase
		{
			public DefaultIfEmptyContext(IParseContext parent, IParseContext sequence, Expression defaultValue) 
				: base(parent, sequence, null)
			{
				_defaultValue = defaultValue;
			}

			private readonly Expression _defaultValue;

			public override Expression BuildExpression(Expression expression, int level)
			{
				var expr = Sequence.BuildExpression(expression, level);

				if (expression == null)
				{
					var q =
						from col in SqlQuery.Select.Columns
						where !col.CanBeNull()
						select SqlQuery.Select.Columns.IndexOf(col);

					var idx = q.DefaultIfEmpty(-1).First();

					if (idx == -1)
						idx = SqlQuery.Select.Add(new SqlValue((int?) 1));

					var n = ConvertToParentIndex(idx, this);

					var e = Expression.Call(
						ExpressionParser.DataReaderParam,
						ReflectionHelper.DataReader.IsDBNull,
						Expression.Constant(n)) as Expression;

					var defaultValue = _defaultValue ?? Expression.Constant(null, expr.Type);

					expr = Expression.Condition(e, defaultValue, expr);
				}

				return expr;
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToSql(expression, level, flags);
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToIndex(expression, level, flags);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				return Sequence.IsExpression(expression, level, requestFlag);
			}

			public override IParseContext GetContext(Expression expression, int level, ParseInfo parseInfo)
			{
				return Sequence.GetContext(expression, level, parseInfo);
			}
		}
	}
}
