using System;
using System.Linq.Expressions;
using System.Reflection;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class UpdateBuilder : MethodCallBuilder
	{
		#region Update

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Update");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			switch (methodCall.Arguments.Count)
			{
				case 1 : // int Update<T>(this IUpdateable<T> source)
					break;

				case 2 : // int Update<T>(this IQueryable<T> source, Expression<Func<T,T>> setter)
					{
						BuildSetter(
							builder, buildInfo, (LambdaExpression)methodCall.Arguments[1].Unwrap(), sequence, sequence);
						break;
					}

				case 3 :
					{
						var expr = methodCall.Arguments[1].Unwrap();

						if (expr is LambdaExpression)
						{
							// int Update<T>(this IQueryable<T> source, Expression<Func<T,bool>> predicate, Expression<Func<T,T>> setter)
							//
							sequence = builder.BuildWhere(buildInfo.Parent, sequence, (LambdaExpression)methodCall.Arguments[1].Unwrap(), false);

							BuildSetter(builder, buildInfo, (LambdaExpression)methodCall.Arguments[2].Unwrap(), sequence, sequence);
						}
						else
						{
							// static int Update<TSource,TTarget>(this IQueryable<TSource> source, Table<TTarget> target, Expression<Func<TSource,TTarget>> setter)
							//
							var into = builder.BuildSequence(new BuildInfo(buildInfo, expr, new SqlQuery()));

							BuildSetter(builder, buildInfo, (LambdaExpression)methodCall.Arguments[2].Unwrap(), into, sequence);

							var sql = sequence.SqlQuery;

							sql.Select.Columns.Clear();

							foreach (var item in sql.Set.Items)
								sql.Select.Columns.Add(new SqlQuery.Column(sql, item.Expression));

							sql.Set.Into = ((TableBuilder.TableContext)into).SqlTable;
						}

						break;
					}
			}

			sequence.SqlQuery.QueryType = QueryType.Update;

			return new UpdateContext(buildInfo.Parent, sequence);
		}

		#endregion

		#region Helpers

		internal static void BuildSetter(
			ExpressionBuilder builder,
			BuildInfo         buildInfo,
			LambdaExpression  setter,
			IBuildContext     into,
			IBuildContext     sequence)
		{
			if (setter.Body.NodeType != ExpressionType.MemberInit)
				throw new LinqException("Object initializer expected for insert statement.");

			var body = (MemberInitExpression)setter.Body;
			var ex   = body;
			var ctx  = new ExpressionContext(buildInfo.Parent, sequence, setter);

			for (var i = 0; i < ex.Bindings.Count; i++)
			{
				var binding = ex.Bindings[i];
				var member  = binding.Member;

				if (member is MethodInfo)
					member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

				if (binding is MemberAssignment)
				{
					var ma     = binding as MemberAssignment;
					var column = into.ConvertToSql(
						Expression.MakeMemberAccess(Expression.Parameter(member.DeclaringType, "p"), member), 1, ConvertFlags.Field);
					var expr   = builder.ConvertToSql(ctx, ma.Expression);

					if (expr is SqlParameter && ma.Expression.Type.IsEnum)
						((SqlParameter)expr).SetEnumConverter(ma.Expression.Type, builder.MappingSchema);

					sequence.SqlQuery.Set.Items.Add(new SqlQuery.SetExpression(column[0].Sql, expr));
				}
				else
					throw new InvalidOperationException();
			}
		}

		internal static void ParseSet(
			ExpressionBuilder builder,
			BuildInfo         buildInfo,
			LambdaExpression  extract, 
			LambdaExpression  update, 
			IBuildContext     select)
		{
			var pi = extract.Body;

			while (pi.NodeType == ExpressionType.Convert || pi.NodeType == ExpressionType.ConvertChecked)
				pi = ((UnaryExpression)pi).Operand;

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for the 'Set' statement.");

			var body   = (MemberExpression)pi;
			var member = body.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var sql    = select.SqlQuery;
			var column = sql.Set.Into != null ?
				sql.Set.Into.Fields[member.Name] :
				select.ConvertToSql(
					Expression.MakeMemberAccess(Expression.Parameter(member.DeclaringType, "p"), member), 1, ConvertFlags.Field)[0].Sql;
			var ctx    = new ExpressionContext(buildInfo.Parent, select, update);
			var expr   = builder.ConvertToSql(ctx, update.Body);

			if (expr is SqlParameter && update.Body.Type.IsEnum)
				((SqlParameter)expr).SetEnumConverter(update.Body.Type, builder.MappingSchema);

			sql.Set.Items.Add(new SqlQuery.SetExpression(column, expr));
		}

		internal static void ParseSet(
			ExpressionBuilder builder,
			BuildInfo         buildInfo,
			LambdaExpression  extract,
			Expression        update,
			IBuildContext     select)
		{
			var pi = extract.Body;

			if (!ExpressionHelper.IsConstant(update.Type) && !builder.AsParameters.Contains(update))
				builder.AsParameters.Add(update);

			while (pi.NodeType == ExpressionType.Convert || pi.NodeType == ExpressionType.ConvertChecked)
				pi = ((UnaryExpression)pi).Operand;

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for the 'Set' statement.");

			var body   = (MemberExpression)pi;
			var member = body.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var column = select.ConvertToSql(
				Expression.MakeMemberAccess(Expression.Parameter(member.DeclaringType, "p"), member), 1, ConvertFlags.Field);

			if (column.Length == 0)
				throw new LinqException("Member '{0}.{1}' is not a table column.", member.DeclaringType.Name, member.Name);

			var expr   = builder.ConvertToSql(select, update);

			if (expr is SqlParameter && update.Type.IsEnum)
				((SqlParameter)expr).SetEnumConverter(update.Type, builder.MappingSchema);

			select.SqlQuery.Set.Items.Add(new SqlQuery.SetExpression(column[0].Sql, expr));
		}

		#endregion

		#region UpdateContext

		class UpdateContext : SequenceContextBase
		{
			public UpdateContext(IBuildContext parent, IBuildContext sequence)
				: base(parent, sequence, null)
			{
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				query.SetNonQueryQuery();
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

		#endregion

		#region Set

		internal class Set : MethodCallBuilder
		{
			protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				return methodCall.IsQueryable("Set");
			}

			protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
				var extract  = (LambdaExpression)methodCall.Arguments[1].Unwrap();
				var update   =                   methodCall.Arguments[2].Unwrap();

				if (update.NodeType == ExpressionType.Lambda)
					ParseSet(builder, buildInfo, extract, (LambdaExpression)update, sequence);
				else
					ParseSet(builder, buildInfo, extract, update, sequence);

				return sequence;
			}
		}

		#endregion
	}
}
