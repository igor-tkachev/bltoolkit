using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class InsertOrUpdateBuilder : MethodCallBuilder
	{
		#region InsertOrUpdateBuilder

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("InsertOrUpdate");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			switch (methodCall.Arguments.Count)
			{
				case 2 : // static int InsertOrUpdate<T>(
					{    //     this Table<T> target, Expression<Func<T>> insertSetter, Expression<Func<T>> onDuplicateKeyUpdateSetter)
						UpdateBuilder.BuildSetter(
							builder,
							buildInfo,
							(LambdaExpression)methodCall.Arguments[1].Unwrap(),
							sequence,
							sequence.SqlQuery.Insert.Items,
							sequence);

						UpdateBuilder.BuildSetter(
							builder,
							buildInfo,
							(LambdaExpression)methodCall.Arguments[2].Unwrap(),
							sequence,
							sequence.SqlQuery.Update.Items,
							sequence);

						sequence.SqlQuery.Insert.Into  = ((TableBuilder.TableContext)sequence).SqlTable;
						sequence.SqlQuery.Update.Table = ((TableBuilder.TableContext)sequence).SqlTable;
						sequence.SqlQuery.From.Tables.Clear();

						break;
					}
			}

			sequence.SqlQuery.QueryType = QueryType.InsertOrUpdate;

			return new UpdateBuilder.UpdateContext(buildInfo.Parent, sequence);
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}

		#endregion
	}
}
