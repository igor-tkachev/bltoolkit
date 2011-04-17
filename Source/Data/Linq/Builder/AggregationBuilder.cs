using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class AggregationBuilder : MethodCallBuilder
	{
		public static string[] MethodNames = new[] { "Average", "Min", "Max", "Sum" };

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable(MethodNames);
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			if (sequence.SqlQuery != buildInfo.SqlQuery)
			{
				throw new NotImplementedException();
			}

			if (sequence.SqlQuery.Select.IsDistinct        ||
			    sequence.SqlQuery.Select.TakeValue != null ||
			    sequence.SqlQuery.Select.SkipValue != null ||
			   !sequence.SqlQuery.GroupBy.IsEmpty)
			{
				sequence = new SubQueryContext(sequence);
			}

			if (sequence.SqlQuery.OrderBy.Items.Count > 0)
			{
				if (sequence.SqlQuery.Select.TakeValue == null && sequence.SqlQuery.Select.SkipValue == null)
					sequence.SqlQuery.OrderBy.Items.Clear();
				else
					sequence = new SubQueryContext(sequence);
			}

			//var index = sequence.ConvertToIndex(null, 0, ConvertFlags.Field);

			var context = new AggregationContext(buildInfo.Parent, sequence, null, methodCall.Method.ReturnType);

			context.FieldIndex = context.SqlQuery.Select.Add(
				new SqlFunction(
					methodCall.Type,
					methodCall.Method.Name,
					sequence.ConvertToSql(null, 0, ConvertFlags.Field).Select(_ => _.Sql).ToArray()));

			return context;
		}

		class AggregationContext : SequenceContextBase
		{
			public AggregationContext(IBuildContext parent, IBuildContext sequence, LambdaExpression lambda, Type returnType)
				: base(parent, sequence, lambda)
			{
				_returnType = returnType;
			}

			readonly Type _returnType;

			public int FieldIndex;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = Expression.Convert(Builder.BuildSql(_returnType, FieldIndex), typeof(object));

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],object>>(
					expr, new []
					{
						ExpressionBuilder.ContextParam,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						ExpressionBuilder.ExpressionParam,
						ExpressionBuilder.ParametersParam,
					});

				query.SetElementQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				return Builder.BuildSql(_returnType, FieldIndex);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.All   :
					case ConvertFlags.Key   :
					case ConvertFlags.Field : return Sequence.ConvertToSql(expression, level + 1, flags);
				}

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Root       : return Lambda != null && expression == Lambda.Parameters[0];
					case RequestFor.Expression : return true;
				}

				return false;
			}

			public override IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				throw new NotImplementedException();
			}

			public override ISqlExpression GetSubQuery(IBuildContext context)
			{
				return base.GetSubQuery(context);
			}
		}
	}
}
