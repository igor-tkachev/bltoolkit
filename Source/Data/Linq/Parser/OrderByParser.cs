using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class OrderByParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			if (!methodCall.IsQueryable("OrderBy", "OrderByDescending", "ThenBy", "ThenByDescending"))
				return false;

			var body = ((LambdaExpression)methodCall.Arguments[1].Unwrap()).Body.Unwrap();

			if (body.NodeType == ExpressionType	.MemberInit)
			{
				var mi = (MemberInitExpression)body;
				bool throwExpr;

				if (mi.NewExpression.Arguments.Count > 0 || mi.Bindings.Count == 0)
					throwExpr = true;
				else
					throwExpr = mi.Bindings.Any(b => b.BindingType != MemberBindingType.Assignment);

				if (throwExpr)
					throw new NotSupportedException(string.Format("Explicit construction of entity type '{0}' in order by is not allowed.", body.Type));
			}

			return true;
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);

			if (sequence.SqlQuery.Select.TakeValue != null || sequence.SqlQuery.Select.SkipValue != null)
				sequence = new SubQueryContext(sequence);

			var lambda = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var order  = new PathThroughContext(sequence, lambda);
			var body   = lambda.Body.Unwrap();
			var sql    = parser.ParseExpressions(order, body, ConvertFlags.Key);

			if (!methodCall.Method.Name.StartsWith("Then"))
				sequence.SqlQuery.OrderBy.Items.Clear();

			foreach (var expr in sql)
			{
				var e = expr.Sql;

				if (e is SqlQuery.SearchCondition)
				{
					if (e.CanBeNull())
					{
						var notExpr = new SqlQuery.SearchCondition
						{
							Conditions = { new SqlQuery.Condition(true, new SqlQuery.Predicate.Expr(e, e.Precedence)) }
						};

						e = parser.Convert(sequence, new SqlFunction(e.SystemType, "CASE", e, new SqlValue(1), notExpr, new SqlValue(0), new SqlValue(null)));
					}
					else
						e = parser.Convert(sequence, new SqlFunction(e.SystemType, "CASE", e, new SqlValue(1), new SqlValue(0)));
				}

				sequence.SqlQuery.OrderBy.Expr(e, methodCall.Method.Name.EndsWith("Descending"));
			}

			return sequence;
		}
	}
}
