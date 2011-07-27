using System;
using System.Linq;
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
				case 3 : // static int InsertOrUpdate<T>(
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

			var table = sequence.SqlQuery.Insert.Into;
			var keys  = table.GetKeys(false);

			if (keys.Count == 0)
				throw new LinqException("InsertOrUpdate method requires the '{0}' table to have a primary key.", table.Name);

			var missedKey = keys.Except(
				from k in keys
					join i in sequence.SqlQuery.Insert.Items
					on k equals i.Column
				select k).FirstOrDefault();

			if (missedKey != null)
				throw new LinqException("InsertOrUpdate method requires the '{0}.{1}' field to be included in the insert setter.",
					table.Name,
					((SqlField)missedKey).Name);

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
