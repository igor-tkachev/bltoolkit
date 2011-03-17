using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class InsertBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Insert", "InsertWithIdentity");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			return new InsertContext(buildInfo.Parent, sequence, null);
		}
	}

	class InsertContext : SequenceContextBase
	{
		public InsertContext(IBuildContext parent, IBuildContext sequence, LambdaExpression lambda)
			: base(parent, sequence, lambda)
		{
		}

		public override Expression BuildExpression(Expression expression, int level)
		{
			throw new NotImplementedException();
		}

		public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}

		public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			throw new NotImplementedException();
		}

		public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
		{
			throw new NotImplementedException();
		}

		public override IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
		{
			throw new NotImplementedException();
		}
	}
}
