using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class SelectBuilder : MethodCallBuilder
	{
		#region SelectBuilder

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			if (methodCall.IsQueryable("Select"))
			{
				switch (((LambdaExpression)methodCall.Arguments[1].Unwrap()).Parameters.Count)
				{
					case 1 :
					case 2 : return true;
					default: break;
				}
			}

			return false;
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var selector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			sequence.SetAlias(selector.Parameters[0].Name);

			var body = selector.Body.Unwrap();

			// .Select(p => p)
			//
			//if (body == selector.Parameters[0])
			//	return sequence;

			switch (body.NodeType)
			{
				case ExpressionType.Parameter : break;
				default                       :
					sequence = CheckSubQueryForSelect(sequence);
					break;
			}

			var context = selector.Parameters.Count == 1 ?
				new SelectContext (buildInfo.Parent, selector, sequence) :
				new SelectContext2(buildInfo.Parent, selector, sequence);

#if DEBUG
			context.MethodCall = methodCall;
#endif

			return context;
		}

		static IBuildContext CheckSubQueryForSelect(IBuildContext context)
		{
			if (/*_parsingMethod[0] != ParsingMethod.OrderBy &&*/ context.SqlQuery.Select.IsDistinct)
				return new SubQueryContext(context);

			return context;
		}

		#endregion

		#region SelectContext2

		class SelectContext2 : SelectContext
		{
			public SelectContext2(IBuildContext parent, LambdaExpression lambda, IBuildContext sequence)
				: base(parent, lambda, sequence)
			{
			}

			static readonly ParameterExpression _counterParam = Expression.Parameter(typeof(int), "counter");

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<int,QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						_counterParam,
						ExpressionBuilder.ContextParam,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						ExpressionBuilder.ExpressionParam,
						ExpressionBuilder.ParametersParam,
					});

				var func    = mapper.Compile();
				var counter = 0;

				Func<QueryContext,IDataContext,IDataReader,Expression,object[],T> map = (ctx,db,rd,e,ps) => func(counter++, ctx, db, rd, e, ps);

				query.SetQuery(map);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression :
					case RequestFor.Root       :
						if (expression == Lambda.Parameters[1])
							return true;
						break;
				}

				return base.IsExpression(expression, level, requestFlag);
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == Lambda.Parameters[1])
					return _counterParam;

				return base.BuildExpression(expression, level);
			}
		}

		#endregion

		#region Convert

		[DebuggerDisplay("Path = {Path}; Expr = {Expr}")]
		class ExprInfo
		{
			public Expression Path;
			public Expression Expr;
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression originalMethodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			var methodCall = originalMethodCall;
			var selector   = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var info       = builder.ConvertSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]), selector.Parameters[0]);

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

				selector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			}

			if (param != builder.SequenceParameter)
			{
				var list = GetExpressions(param, selector.Body.Unwrap()).ToList();

				if (list.Count > 0)
				{
					var p = list.FirstOrDefault(e => e.Expr == selector.Parameters[0]);

					if (p == null)
					{
						return null;
					}
					else if (list.Count > 1)
					{
						return new SequenceConvertInfo
						{
							Expression = methodCall,
							ExpressionsToReplace = list
								.Where (e => e != p)
								.Select(ei =>
								{
									ei.Expr = ei.Expr.Convert(e => e == p.Expr ? p.Path : e);
									return ei;
								})
								.ToDictionary(e => e.Path, e => e.Expr)
						};
					}
				}
			}

			if (methodCall != originalMethodCall)
				return new SequenceConvertInfo { Expression = methodCall, };

			return null;
		}

		static IEnumerable<ExprInfo> GetExpressions(Expression path, Expression expression)
		{
			switch (expression.NodeType)
			{
				// new { ... }
				//
				case ExpressionType.New        :
					{
						var expr = (NewExpression)expression;

						if (expr.Members != null) for (var i = 0; i < expr.Members.Count; i++)
						{
							var q = GetExpressions(Expression.MakeMemberAccess(path, expr.Members[i]), expr.Arguments[i]);
							foreach (var e in q)
								yield return e;
						}

						break;
					}

				// new MyObject { ... }
				//
				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)expression;
						var dic  = TypeAccessor.GetAccessor(expr.Type)
							.Select((m,i) => new { m, i })
							.ToDictionary(_ => _.m.MemberInfo.Name, _ => _.i);

						foreach (var binding in expr.Bindings.Cast<MemberAssignment>().OrderBy(b => dic[b.Member.Name]))
						{
							var q = GetExpressions(Expression.MakeMemberAccess(path, binding.Member), binding.Expression);
							foreach (var e in q)
								yield return e;
						}

						break;
					}

				// parameter
				//
				case ExpressionType.Parameter  :
					yield return new ExprInfo { Path = path, Expr = expression };
					break;

				// everything else
				//
				default                        :
					{
						if (TypeHelper.IsSameOrParent(typeof(IEnumerable), expression.Type) ||
						    TypeHelper.IsSameOrParent(typeof(IQueryable),  expression.Type))
							yield return new ExprInfo { Path = path, Expr = expression };

						break;
					}
			}
		}

		#endregion
	}
}
