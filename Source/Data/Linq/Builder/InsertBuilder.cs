using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;
	using Reflection;

	class InsertBuilder : MethodCallBuilder
	{
		#region InsertBuilder

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Insert", "InsertWithIdentity");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			var isSubQuery = sequence.SqlQuery.Select.IsDistinct;

			if (isSubQuery)
				sequence = new SubQueryContext(sequence);

			switch (methodCall.Arguments.Count)
			{
				case 1 : 
					// static int Insert<T>              (this IValueInsertable<T> source)
					// static int Insert<TSource,TTarget>(this ISelectInsertable<TSource,TTarget> source)
					{
						foreach (var item in sequence.SqlQuery.Set.Items)
							sequence.SqlQuery.Select.Expr(item.Expression);
						break;
					}

				case 2 : // static int Insert<T>(this Table<T> target, Expression<Func<T>> setter)
					{
						BuildSetter(builder, buildInfo, (LambdaExpression)methodCall.Arguments[1].Unwrap(), sequence, sequence);

						sequence.SqlQuery.Set.Into  = ((TableBuilder.TableContext)sequence).SqlTable;
						sequence.SqlQuery.From.Tables.Clear();

						break;
					}

				case 3 : // static int Insert<TSource,TTarget>(this IQueryable<TSource> source, Table<TTarget> target, Expression<Func<TSource,TTarget>> setter)
					{
						var into = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[1], new SqlQuery()));

						BuildSetter(builder, buildInfo, (LambdaExpression)methodCall.Arguments[2].Unwrap(), into, sequence);

						sequence.SqlQuery.Select.Columns.Clear();

						foreach (var item in sequence.SqlQuery.Set.Items)
							sequence.SqlQuery.Select.Columns.Add(new SqlQuery.Column(sequence.SqlQuery, item.Expression));

						sequence.SqlQuery.Set.Into = ((TableBuilder.TableContext)into).SqlTable;

						break;
					}
			}

			sequence.SqlQuery.QueryType        = QueryType.Insert;
			sequence.SqlQuery.Set.WithIdentity = methodCall.Method.Name == "InsertWithIdentity";

			return new InsertContext(buildInfo.Parent, sequence, sequence.SqlQuery.Set.WithIdentity);
		}

		static void BuildSetter(
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
						Expression.MakeMemberAccess(Expression.Parameter(member.DeclaringType), member), 1, ConvertFlags.Field);
					var expr   = builder.ConvertToSql(ctx, ma.Expression);

					if (expr is SqlParameter && ma.Expression.Type.IsEnum)
						((SqlParameter)expr).SetEnumConverter(ma.Expression.Type, builder.MappingSchema);

					sequence.SqlQuery.Set.Items.Add(new SqlQuery.SetExpression(column[0].Sql, expr));
				}
				else
					throw new InvalidOperationException();
			}
		}

		#endregion

		#region InsertContext

		class InsertContext : SequenceContextBase
		{
			public InsertContext(IBuildContext parent, IBuildContext sequence, bool insertWithIdentity)
				: base(parent, sequence, null)
			{
				_insertWithIdentity = insertWithIdentity;
			}

			readonly bool _insertWithIdentity;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				if (_insertWithIdentity) query.SetScalarQuery<object>();
				else					 query.SetNonQueryQuery();
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

		#region Into

		internal class Into : MethodCallBuilder
		{
			protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				return methodCall.IsQueryable("Into");
			}

			protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				var source = methodCall.Arguments[0].Unwrap();
				var into   = methodCall.Arguments[1].Unwrap();

				IBuildContext sequence;

				// static IValueInsertable<T> Into<T>(this IDataContext dataContext, Table<T> target)
				//
				if (source.NodeType == ExpressionType.Constant && ((ConstantExpression)source).Value == null)
				{
					sequence = builder.BuildSequence(new BuildInfo((IBuildContext)null, into, new SqlQuery()));
					sequence.SqlQuery.Set.Into = ((TableBuilder.TableContext)sequence).SqlTable;
					sequence.SqlQuery.From.Tables.Clear();
				}
				// static ISelectInsertable<TSource,TTarget> Into<TSource,TTarget>(this IQueryable<TSource> source, Table<TTarget> target)
				//
				else
				{
					sequence = builder.BuildSequence(new BuildInfo(buildInfo, source));
					var tbl = builder.BuildSequence(new BuildInfo((IBuildContext)null, into, new SqlQuery()));
					sequence.SqlQuery.Set.Into = ((TableBuilder.TableContext)tbl).SqlTable;
				}

				sequence.SqlQuery.Select.Columns.Clear();

				return sequence;
			}
		}

		#endregion

		#region Value

		internal class Value : MethodCallBuilder
		{
			protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				return methodCall.IsQueryable("Value");
			}

			protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
			{
				var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
				var extract  = (LambdaExpression)methodCall.Arguments[1].Unwrap();
				var update   =                   methodCall.Arguments[2].Unwrap();




				return sequence;
			}
		}

/*
		static IValueInsertable<T> Value<T,TV>(this Table<T>            source, Expression<Func<T,TV>> field, Expression<Func<TV>> value)
		static IValueInsertable<T> Value<T,TV>(this Table<T>            source, Expression<Func<T,TV>> field, TV                   value)
		static IValueInsertable<T> Value<T,TV>(this IValueInsertable<T> source, Expression<Func<T,TV>> field, Expression<Func<TV>> value)
		static IValueInsertable<T> Value<T,TV>(this IValueInsertable<T> source, Expression<Func<T,TV>> field, TV                   value)

		static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(this ISelectInsertable<TSource,TTarget> source, Expression<Func<TTarget,TValue>> field, Expression<Func<TSource,TValue>> value)
		static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(this ISelectInsertable<TSource,TTarget> source, Expression<Func<TTarget,TValue>> field, Expression<Func<TValue>>         value)
		static ISelectInsertable<TSource,TTarget> Value<TSource,TTarget,TValue>(this ISelectInsertable<TSource,TTarget> source, Expression<Func<TTarget,TValue>> field, TValue                           value)
 * 
		void ParseValue(LambdaInfo extract, Expression update, QuerySource select)
		{
			if (!ExpressionHelper.IsConstant(update.Type) && !_asParameters.Contains(update))
				_asParameters.Add(update);

			if (CurrentSql.Set.Into == null)
			{
				CurrentSql.Set.Into = (SqlTable)CurrentSql.From.Tables[0].Source;
				CurrentSql.From.Tables.Clear();
			}

			ParseSet(extract, update, select);
		}

		void ParseValue(LambdaInfo extract, LambdaInfo update, QuerySource select)
		{
			if (CurrentSql.Set.Into == null)
			{
				CurrentSql.Set.Into = (SqlTable)CurrentSql.From.Tables[0].Source;
				CurrentSql.From.Tables.Clear();
			}

			ParseSet(extract, update, select);
		}
 * 
 * 
 */

		#endregion
	}
}
