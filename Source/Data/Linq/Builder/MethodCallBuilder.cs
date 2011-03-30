using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	abstract class MethodCallBuilder : ISequenceBuilder
	{
		public int BuildCounter { get; set; }

		public bool CanBuild(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			if (buildInfo.Expression.NodeType == ExpressionType.Call)
				return CanBuildMethodCall(builder, (MethodCallExpression)buildInfo.Expression, buildInfo);
			return false;
		}

		public IBuildContext BuildSequence(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			return BuildMethodCall(builder, (MethodCallExpression)buildInfo.Expression, buildInfo);
		}

		protected abstract bool            CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo);
		protected abstract IBuildContext BuildMethodCall   (ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo);
	}
}
