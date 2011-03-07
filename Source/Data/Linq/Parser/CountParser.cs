using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class CountParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Count", "LongCount");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var returnType = methodCall.Method.ReturnType;
			var sequence   = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);

			if (methodCall.Arguments.Count == 2)
			{
				var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();

				sequence = parser.ParseWhere(parent, sequence, condition, true);
				sequence.SetAlias(condition.Parameters[0].Name);
			}

			if (sequence.SqlQuery != sqlQuery)
			{
				if (sequence is JoinParser.GroupJoinSubQueryContext)
				{
					var ctx = new CountConext(parent, sequence, returnType);

					ctx.SqlQuery   = ((JoinParser.GroupJoinSubQueryContext)sequence).GetCounter(methodCall);
					ctx.Sql        = ctx.SqlQuery;
					ctx.FieldIndex = ctx.SqlQuery.Select.Add(SqlFunction.CreateCount(returnType, ctx.SqlQuery), "cnt");

					return ctx;
				}

				if (sequence is GroupByParser.GroupByContext)
				{
					var groupBy = (GroupByParser.GroupByContext)sequence;
					var sql     = groupBy.SqlQuery.Clone(o => !(o is SqlParameter));

					groupBy.SqlQuery.Where.SearchCondition.Conditions.RemoveAt(groupBy.SqlQuery.Where.SearchCondition.Conditions.Count - 1);

					sql.Select.Columns.Clear();

					if (parser.SqlProvider.IsSubQueryColumnSupported && parser.SqlProvider.IsCountSubQuerySupported)
					{
						for (var i = 0; i < sql.GroupBy.Items.Count; i++)
						{
							var item1 = sql.GroupBy.Items[i];
							var item2 = groupBy.SqlQuery.GroupBy.Items[i];
							var pr    = parser.Convert(sequence, new SqlQuery.Predicate.ExprExpr(item1, SqlQuery.Predicate.Operator.Equal, item2));

							sql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, pr));
						}

						sql.GroupBy.Items.Clear();
						sql.ParentSql = groupBy.SqlQuery;

						var ctx = new CountConext(parent, sequence, returnType);

						ctx.SqlQuery   = sql;
						ctx.Sql        = sql;
						ctx.FieldIndex = sql.Select.Add(SqlFunction.CreateCount(returnType, sql), "cnt");

						return ctx;
					}
					else
					{
						var join = sql.WeakLeftJoin();

						groupBy.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);

						for (var i = 0; i < sql.GroupBy.Items.Count; i++)
						{
							var item1 = sql.GroupBy.Items[i];
							var item2 = groupBy.SqlQuery.GroupBy.Items[i];
							var col   = sql.Select.Columns[sql.Select.Add(item1)];
							var pr    = parser.Convert(sequence, new SqlQuery.Predicate.ExprExpr(col, SqlQuery.Predicate.Operator.Equal, item2));

							join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, pr));
						}

						sql.ParentSql = groupBy.SqlQuery;

						var ctx = new CountConext(parent, sequence, returnType);

						ctx.SqlQuery   = sql;
						ctx.Sql        = new SqlFunction(returnType, "Count", sql.Select.Columns[0]);
						ctx.FieldIndex = -1;

						return ctx;
					}
				}

				throw new NotImplementedException();
			}

			if (sequence.SqlQuery.Select.IsDistinct || sequence.SqlQuery.Select.TakeValue != null || sequence.SqlQuery.Select.SkipValue != null)
			{
				sequence.ConvertToIndex(null, 0, ConvertFlags.Key);
				sequence = new SubQueryContext(sequence);
			}

			if (!sequence.SqlQuery.GroupBy.IsEmpty)
			{
				//sequence.SqlQuery.Select.Columns.Clear();

				//foreach (var item in sequence.SqlQuery.GroupBy.Items)
				//	sequence.SqlQuery.Select.Add(item);

				sequence.ConvertToIndex(null, 0, ConvertFlags.Key);
				sequence = new SubQueryContext(sequence);
			}

			if (sequence.SqlQuery.OrderBy.Items.Count > 0)
			{
				if (sequence.SqlQuery.Select.TakeValue == null && sequence.SqlQuery.Select.SkipValue == null)
					sequence.SqlQuery.OrderBy.Items.Clear();
				else
					sequence = new SubQueryContext(sequence);
			}

			var context = new CountConext(parent, sequence, returnType);

			//context.SqlQuery.Select.Columns.Clear();

			context.Sql        = context.SqlQuery;
			context.FieldIndex = context.SqlQuery.Select.Add(SqlFunction.CreateCount(returnType, context.SqlQuery), "cnt");

			return context;
		}

		static bool IsParent(IParseContext sequence, IParseContext parent)
		{
			for (; sequence != null; sequence = sequence.Parent)
				if (sequence == parent)
					return true;

			return false;
		}

		class CountConext : SequenceContextBase
		{
			public CountConext(IParseContext parent, IParseContext sequence, Type returnType)
				: base(parent, sequence, null)
			{
				_returnType = returnType;
			}

			private  SqlInfo[] _index;
			readonly Type      _returnType;

			public int            FieldIndex;
			public ISqlExpression Sql;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = Expression.Convert(Parser.BuildSql(_returnType, FieldIndex), typeof(object));

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],object>>(
					expr, new []
					{
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				query.SetElementQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				return Parser.BuildSql(_returnType, ConvertToIndex(expression, level, ConvertFlags.Field)[0].Index);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field : return new[] { new SqlInfo { Query = Parent.SqlQuery, Sql = Sql } };
				}

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
						return _index ?? (_index = new[]
						{
							new SqlInfo { Query = Parent.SqlQuery, Index = Parent.SqlQuery.Select.Add(Sql), Sql = Sql, }
						});
				}

				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression : return true;
				}

				return false;
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				return Sequence.GetContext(expression, level, currentSql);
			}
		}
	}
}
