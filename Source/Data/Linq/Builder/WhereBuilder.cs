using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class WhereBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Where");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence  = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
			var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var result    = builder.BuildWhere(buildInfo.Parent, sequence, condition, true);

			result.SetAlias(condition.Parameters[0].Name);

			return result;
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			var predicate = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var info      = builder.ConvertSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]), predicate.Parameters[0]);

			if (info != null)
			{
				info.Expression = methodCall.Convert(ex => ConvertMethod(methodCall, 0, info, predicate.Parameters[0], ex));

				if (param.Type != info.Parameter.Type)
					param = Expression.Parameter(info.Parameter.Type, param.Name);

				var q =
					from ex in info.ExpressionsToReplace
					select new
					{
						Key   = ex.Key.  Convert(e => e == info.Parameter ? param : e),
						Value = ex.Value.Convert(e => e == info.Parameter ? param : e)
					};

				info.Parameter = param;
				info.ExpressionsToReplace = q.ToDictionary(e => e.Key, e => e.Value);

				return info;
			}

			return null;
		}
	}
}
