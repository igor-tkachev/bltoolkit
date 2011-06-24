using System;
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
			ExpressionBuilder builder, MethodCallExpression originalMethodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			var methodCall = originalMethodCall;
			var predicate  = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var info       = builder.ConvertSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]), predicate.Parameters[0]);

			if (info != null)
			{
				methodCall = (MethodCallExpression)methodCall.Convert(ex =>
				{
					if (ex == methodCall.Arguments[0])
						return info.Expression;

					switch (ex.NodeType)
					{
						case ExpressionType.Parameter :

							foreach (var item in info.ExpressionsToReplace)
								if (ex == item.Key)
									return item.Value;

							break;

						case ExpressionType.MemberAccess :

							foreach (var item in info.ExpressionsToReplace)
							{
								var ex1 = ex;
								var ex2 = item.Key;

								while (ex1.NodeType == ex2.NodeType)
								{
									if (ex1.NodeType == ExpressionType.Parameter)
										return ex1 == ex2 ? item.Value : ex;

									if (ex2.NodeType != ExpressionType.MemberAccess)
										return ex;

									var ma1 = (MemberExpression)ex1;
									var ma2 = (MemberExpression)ex2;

									if (ma1.Member != ma2.Member)
										return ex;

									ex1 = ma1.Expression;
									ex2 = ma2.Expression;
								}
							}

							break;
					}
					return ex;
				});

				predicate = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			}

			if (methodCall != originalMethodCall)
				return new SequenceConvertInfo
				{
					Parameter  = param,
					Expression = methodCall,
				};

			return null;
		}
	}
}
