using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Reflection;

	class OfTypeBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("OfType");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			var table = sequence as TableBuilder.TableContext;

			if (table != null && table.InheritanceMapping.Count > 0)
			{
				var objectType = methodCall.Type.GetGenericArguments()[0];

				if (TypeHelper.IsSameOrParent(table.ObjectType, objectType))
				{
					var predicate = builder.MakeIsPredicate(table, objectType);

					if (predicate.GetType() != typeof(SqlQuery.Predicate.Expr))
						sequence.SqlQuery.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));
				}
			}

			return sequence;
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}
	}
}
