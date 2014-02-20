﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Common;
	using Data.Sql;
	using Mapping;
	using Reflection;

	partial class ExpressionBuilder
	{
		#region Build Where

		public IBuildContext BuildWhere(IBuildContext parent, IBuildContext sequence, LambdaExpression condition, bool checkForSubQuery)
		{
			var makeHaving = false;
			var prevParent = sequence.Parent;

			var ctx  = new ExpressionContext(parent, sequence, condition);
			var expr = ConvertExpression(condition.Body.Unwrap());

			if (checkForSubQuery && CheckSubQueryForWhere(ctx, expr, out makeHaving))
			{
				ReplaceParent(ctx, prevParent);

				sequence = new SubQueryContext(sequence);
				prevParent = sequence.Parent;

				ctx = new ExpressionContext(parent, sequence, condition);
			}

			BuildSearchCondition(
				ctx,
				expr,
				makeHaving ?
					ctx.SqlQuery.Having.SearchCondition.Conditions :
					ctx.SqlQuery.Where. SearchCondition.Conditions);

			ReplaceParent(ctx, prevParent);

			return sequence;
		}

		bool CheckSubQueryForWhere(IBuildContext context, Expression expression, out bool makeHaving)
		{
			var makeSubQuery = false;
			var isHaving     = false;
			var isWhere      = false;

			expression.Visit(expr =>
			{
				if (_subQueryExpressions != null && _subQueryExpressions.Contains(expr))
				{
					makeSubQuery = true;
					isWhere      = true;
					return false;
				}

				var stopWalking = false;

				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)expr;

							if (TypeHelper.IsNullableValueMember(ma.Member) ||
							    TypeHelper.IsNullableHasValueMember(ma.Member))
								break;

							if (SqlProvider.ConvertMember(ma.Member) == null)
							{
								var ctx = GetContext(context, expr);

								if (ctx != null)
								{
									if (ctx.IsExpression(expr, 0, RequestFor.Expression).Result)
										makeSubQuery = true;
									stopWalking = true;
								}
							}

							isWhere = true;

							break;
						}

					case ExpressionType.Call:
						{
							var e = (MethodCallExpression)expr;

							if (e.Method.DeclaringType == typeof(Enumerable) && e.Method.Name != "Contains")
								return isHaving = true;

							isWhere = true;

							break;
						}

					case ExpressionType.Parameter:
						{
							var ctx = GetContext(context, expr);

							if (ctx != null)
							{
								if (ctx.IsExpression(expr, 0, RequestFor.Expression).Result)
									makeSubQuery = true;
								stopWalking = true;
							}

							isWhere = true;

							break;
						}
				}

				return !stopWalking;
			});

			makeHaving = isHaving && !isWhere;
			return makeSubQuery || isHaving && isWhere;
		}

		#endregion

		#region BuildTake

		public void BuildTake(IBuildContext context, ISqlExpression expr)
		{
			var sql = context.SqlQuery;

			sql.Select.Take(expr);

			SqlProvider.SqlQuery = sql;

			if (sql.Select.SkipValue != null && SqlProvider.IsTakeSupported && !SqlProvider.IsSkipSupported)
			{
				if (context.SqlQuery.Select.SkipValue is SqlParameter && sql.Select.TakeValue is SqlValue)
				{
					var skip = (SqlParameter)sql.Select.SkipValue;
					var parm = (SqlParameter)sql.Select.SkipValue.Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);

					parm.SetTakeConverter((int)((SqlValue)sql.Select.TakeValue).Value);

					sql.Select.Take(parm);

					var ep = (from pm in CurrentSqlParameters where pm.SqlParameter == skip select pm).First();

					ep = new ParameterAccessor
					{
						Expression   = ep.Expression,
						Accessor     = ep.Accessor,
						SqlParameter = parm
					};

					CurrentSqlParameters.Add(ep);
				}
				else
					sql.Select.Take(Convert(
						context,
						new SqlBinaryExpression(typeof(int), sql.Select.SkipValue, "+", sql.Select.TakeValue, Precedence.Additive)));
			}

			if (!SqlProvider.TakeAcceptsParameter)
			{
				var p = sql.Select.TakeValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}

		#endregion

		#region SubQueryToSql

		public IBuildContext GetSubQuery(IBuildContext context, MethodCallExpression expr)
		{
			var info = new BuildInfo(context, expr, new SqlQuery { ParentSql = context.SqlQuery });
			var ctx  = BuildSequence(info);

			if (ctx.SqlQuery.Select.Columns.Count == 0 &&
				(ctx.IsExpression(null, 0, RequestFor.Expression).Result ||
				 ctx.IsExpression(null, 0, RequestFor.Field).     Result))
			{
				ctx.ConvertToIndex(null, 0, ConvertFlags.Field);
			}

			return ctx;
		}

		internal ISqlExpression SubQueryToSql(IBuildContext context, MethodCallExpression expression)
		{
			var sequence = GetSubQuery(context, expression);
			var subSql   = sequence.GetSubQuery(context);

			if (subSql != null)
				return subSql;

			var query    = context.SqlQuery;
			var subQuery = sequence.SqlQuery;

			// This code should be moved to context.
			//
			if (!query.GroupBy.IsEmpty && !subQuery.Where.IsEmpty)
			{
				var fromGroupBy = sequence.SqlQuery.Properties
					.OfType<System.Tuple<string,SqlQuery>>()
					.Where(p => p.Item1 == "from_group_by" && p.Item2 == context.SqlQuery)
					.Any();

				if (fromGroupBy)
				{
					if (subQuery.Select.Columns.Count == 1 &&
					    subQuery.Select.Columns[0].Expression.ElementType == QueryElementType.SqlFunction &&
					    subQuery.GroupBy.IsEmpty && !subQuery.Select.HasModifier && !subQuery.HasUnion &&
					    subQuery.Where.SearchCondition.Conditions.Count == 1)
					{
						var cond = subQuery.Where.SearchCondition.Conditions[0];

						if (cond.Predicate.ElementType == QueryElementType.ExprExprPredicate && query.GroupBy.Items.Count == 1 ||
						    cond.Predicate.ElementType == QueryElementType.SearchCondition &&
						    query.GroupBy.Items.Count == ((SqlQuery.SearchCondition)cond.Predicate).Conditions.Count)
						{
							var func = (SqlFunction)subQuery.Select.Columns[0].Expression;

							if (CountBuilder.MethodNames.Contains(func.Name))
								return SqlFunction.CreateCount(func.SystemType, query);
						}
					}
				}
			}

			return sequence.SqlQuery;
		}

		#endregion

		#region IsSubQuery

		bool IsSubQuery(IBuildContext context, MethodCallExpression call)
		{
			if (call.IsQueryable())
			{
				var info = new BuildInfo(context, call, new SqlQuery { ParentSql = context.SqlQuery });

				if (!IsSequence(info))
					return false;

				var arg = call.Arguments[0];

				if (AggregationBuilder.MethodNames.Contains(call.Method.Name))
					while (arg.NodeType == ExpressionType.Call && ((MethodCallExpression) arg).Method.Name == "Select")
						arg = ((MethodCallExpression)arg).Arguments[0];

				var mc = arg as MethodCallExpression;

				while (mc != null)
				{
					if (!mc.IsQueryable())
						return GetTableFunctionAttribute(mc.Method) != null;

					mc = mc.Arguments[0] as MethodCallExpression;
				}

				return arg.NodeType == ExpressionType.Call || IsSubQuerySource(context, arg);
			}

			return false;
		}

		bool IsSubQuerySource(IBuildContext context, Expression expr)
		{
			if (expr == null)
				return false;

			var ctx = GetContext(context, expr);

			if (ctx != null && ctx.IsExpression(expr, 0, RequestFor.Object).Result)
				return true;

			while (expr != null && expr.NodeType == ExpressionType.MemberAccess)
				expr = ((MemberExpression)expr).Expression;

			return expr != null && expr.NodeType == ExpressionType.Constant;
		}

		bool IsGroupJoinSource(IBuildContext context, MethodCallExpression call)
		{
			if (!call.IsQueryable() || CountBuilder.MethodNames.Contains(call.Method.Name))
				return false;

			Expression expr = call;

			while (expr.NodeType == ExpressionType.Call)
				expr = ((MethodCallExpression)expr).Arguments[0];

			var ctx = GetContext(context, expr);

			return ctx != null && ctx.IsExpression(expr, 0, RequestFor.GroupJoin).Result;
		}

		#endregion

		#region ConvertExpression

		interface IConvertHelper
		{
			Expression ConvertNull(MemberExpression expression);
		}

		class ConvertHelper<T> : IConvertHelper
			where T : struct 
		{
			public Expression ConvertNull(MemberExpression expression)
			{
				return Expression.Call(
					null,
					ReflectionHelper.Expressor<T?>.MethodExpressor(p => Sql.ConvertNullable(p)),
					expression.Expression);
			}
		}

		Expression ConvertExpression(Expression expression)
		{
			return expression.Convert2(e =>
			{
				if (CanBeConstant(e) || CanBeCompiled(e))
					return new ExpressionHelper.ConvertInfo(e, true);

				switch (e.NodeType)
				{
					case ExpressionType.New:
						{
							var ex = ConvertNew((NewExpression)e);
							if (ex != null)
								return new ExpressionHelper.ConvertInfo(ConvertExpression(ex));
							break;
						}

					case ExpressionType.Call:
						{
							var cm = ConvertMethod((MethodCallExpression)e);
							if (cm != null)
								return new ExpressionHelper.ConvertInfo(ConvertExpression(cm));
							break;
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)e;
							var l  = SqlProvider.ConvertMember(ma.Member);

							if (l != null)
							{
								var body = l.Body.Unwrap();
								var expr = body.Convert(wpi => wpi.NodeType == ExpressionType.Parameter ? ma.Expression : wpi);

								if (expr.Type != e.Type)
									expr = new ChangeTypeExpression(expr, e.Type);

								return new ExpressionHelper.ConvertInfo(ConvertExpression(expr));
							}

							if (TypeHelper.IsNullableValueMember(ma.Member))
							{
								var ntype  = typeof(ConvertHelper<>).MakeGenericType(ma.Type);
								var helper = (IConvertHelper)Activator.CreateInstance(ntype);
								var expr   = helper.ConvertNull(ma);

								return new ExpressionHelper.ConvertInfo(ConvertExpression(expr));
							}

							if (ma.Member.DeclaringType == typeof(TimeSpan))
							{
								switch (ma.Expression.NodeType)
								{
									case ExpressionType.Subtract       :
									case ExpressionType.SubtractChecked:

										Sql.DateParts datePart;

										switch (ma.Member.Name)
										{
											case "TotalMilliseconds" : datePart = Sql.DateParts.Millisecond; break;
											case "TotalSeconds"      : datePart = Sql.DateParts.Second;      break;
											case "TotalMinutes"      : datePart = Sql.DateParts.Minute;      break;
											case "TotalHours"        : datePart = Sql.DateParts.Hour;        break;
											case "TotalDays"         : datePart = Sql.DateParts.Day;         break;
											default                  : return new ExpressionHelper.ConvertInfo(e);
										}

										var ex     = (BinaryExpression)ma.Expression;
										var method = ReflectionHelper.Expressor<object>.MethodExpressor(
											_ => Sql.DateDiff(Sql.DateParts.Day, DateTime.MinValue, DateTime.MinValue));

										var call   =
											Expression.Convert(
												Expression.Call(
													null,
													method,
													Expression.Constant(datePart),
													Expression.Convert(ex.Right, typeof(DateTime?)),
													Expression.Convert(ex.Left,  typeof(DateTime?))),
												typeof(double));

										return new ExpressionHelper.ConvertInfo(ConvertExpression(call));
								}
							}

							break;
						}
				}

				return new ExpressionHelper.ConvertInfo(e);
			});
		}

		Expression ConvertMethod(MethodCallExpression pi)
		{
			var l = SqlProvider.ConvertMember(pi.Method);
			return l == null ? null : ConvertMethod(pi, l);
		}

		static Expression ConvertMethod(MethodCallExpression pi, LambdaExpression lambda)
		{
			var ef    = lambda.Body.Unwrap();
			var parms = new Dictionary<string,int>(lambda.Parameters.Count);
			var pn    = pi.Method.IsStatic ? 0 : -1;

			foreach (var p in lambda.Parameters)
				parms.Add(p.Name, pn++);

			var pie = ef.Convert(wpi =>
			{
				if (wpi.NodeType == ExpressionType.Parameter)
				{
					int n;
					if (parms.TryGetValue(((ParameterExpression)wpi).Name, out n))
						return n < 0 ? pi.Object : pi.Arguments[n];
				}

				return wpi;
			});

			if (pi.Method.ReturnType != pie.Type)
				pie = new ChangeTypeExpression(pie, pi.Method.ReturnType);

			return pie;
		}

		Expression ConvertNew(NewExpression pi)
		{
			var lambda = SqlProvider.ConvertMember(pi.Constructor);

			if (lambda != null)
			{
				var ef    = lambda.Body.Unwrap();
				var parms = new Dictionary<string,int>(lambda.Parameters.Count);
				var pn    = 0;

				foreach (var p in lambda.Parameters)
					parms.Add(p.Name, pn++);

				return ef.Convert(wpi =>
				{
					if (wpi.NodeType == ExpressionType.Parameter)
					{
						var pe   = (ParameterExpression)wpi;
						var n    = parms[pe.Name];
						return pi.Arguments[n];
					}

					return wpi;
				});
			}

			return null;
		}

		#endregion

		#region BuildExpression

		public SqlInfo[] ConvertExpressions(IBuildContext context, Expression expression, ConvertFlags queryConvertFlag)
		{
			expression = ConvertExpression(expression);

			switch (expression.NodeType)
			{
				case ExpressionType.New :
					{
						var expr = (NewExpression)expression;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
						if (expr.Members == null)
							return Array<SqlInfo>.Empty;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

						return expr.Arguments
							.Select((arg,i) =>
							{
								var mi = expr.Members[i];
								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return ConvertExpressions(context, arg, queryConvertFlag).Select(si => si.Clone(mi));
							})
							.SelectMany(si => si)
							.ToArray();
					}

				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)expression;
						var dic  = TypeAccessor.GetAccessor(expr.Type)
							.Select((m,i) => new { m, i })
							.ToDictionary(_ => _.m.MemberInfo, _ => _.i);

						return expr.Bindings
							.Where  (b => b is MemberAssignment)
							.Cast<MemberAssignment>()
							.OrderBy(b => dic[b.Member])
							.Select (a =>
							{
								var mi = a.Member;
								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return ConvertExpressions(context, a.Expression, queryConvertFlag).Select(si => si.Clone(mi));
							})
							.SelectMany(si => si)
							.ToArray();
					}
			}

			var ctx = GetContext(context, expression);

			if (ctx != null && ctx.IsExpression(expression, 0, RequestFor.Object).Result)
				return ctx.ConvertToSql(expression, 0, queryConvertFlag);

			return new[] { new SqlInfo { Sql = ConvertToSql(context, expression, false) } };
		}

		public ISqlExpression ConvertToSqlExpression(IBuildContext context, Expression expression, bool convertEnum)
		{
			var expr = ConvertExpression(expression);
			return ConvertToSql(context, expr, false, convertEnum);
		}

#if FW3
		public ISqlExpression ConvertToSql(IBuildContext context, Expression expression, bool unwrap)
		{
			return ConvertToSql(context, expression, unwrap, true);
		}
#endif

		public ISqlExpression ConvertToSql(IBuildContext context, Expression expression, bool unwrap, bool convertEnum
#if !FW3
			= true
#endif
			)
		{
			if (CanBeConstant(expression))
				return BuildConstant(expression, convertEnum);

			if (CanBeCompiled(expression))
				return BuildParameter(expression).SqlParameter;

			if (unwrap)
				expression = expression.Unwrap();

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso            :
				case ExpressionType.OrElse             :
				case ExpressionType.Not                :
				case ExpressionType.Equal              :
				case ExpressionType.NotEqual           :
				case ExpressionType.GreaterThan        :
				case ExpressionType.GreaterThanOrEqual :
				case ExpressionType.LessThan           :
				case ExpressionType.LessThanOrEqual    :
					{
						var condition = new SqlQuery.SearchCondition();
						BuildSearchCondition(context, expression, condition.Conditions);
						return condition;
					}

				case ExpressionType.And                :
				case ExpressionType.Or                 :
					{
						if (expression.Type == typeof(bool))
							goto case ExpressionType.AndAlso;
						goto case ExpressionType.Add;
					}

				case ExpressionType.Add                :
				case ExpressionType.AddChecked         :
				case ExpressionType.Divide             :
				case ExpressionType.ExclusiveOr        :
				case ExpressionType.Modulo             :
				case ExpressionType.Multiply           :
				case ExpressionType.MultiplyChecked    :
				case ExpressionType.Power              :
				case ExpressionType.Subtract           :
				case ExpressionType.SubtractChecked    :
				case ExpressionType.Coalesce           :
					{
						var e = (BinaryExpression)expression;
						var l = ConvertToSql(context, e.Left,  false);
						var r = ConvertToSql(context, e.Right, false);
						var t = e.Type;

						switch (expression.NodeType)
						{
							case ExpressionType.Add             :
							case ExpressionType.AddChecked      : return Convert(context, new SqlBinaryExpression(t, l, "+", r, Precedence.Additive));
							case ExpressionType.And             : return Convert(context, new SqlBinaryExpression(t, l, "&", r, Precedence.Bitwise));
							case ExpressionType.Divide          : return Convert(context, new SqlBinaryExpression(t, l, "/", r, Precedence.Multiplicative));
							case ExpressionType.ExclusiveOr     : return Convert(context, new SqlBinaryExpression(t, l, "^", r, Precedence.Bitwise));
							case ExpressionType.Modulo          : return Convert(context, new SqlBinaryExpression(t, l, "%", r, Precedence.Multiplicative));
							case ExpressionType.Multiply:
							case ExpressionType.MultiplyChecked : return Convert(context, new SqlBinaryExpression(t, l, "*", r, Precedence.Multiplicative));
							case ExpressionType.Or              : return Convert(context, new SqlBinaryExpression(t, l, "|", r, Precedence.Bitwise));
							case ExpressionType.Power           : return Convert(context, new SqlFunction(t, "Power", l, r));
							case ExpressionType.Subtract        :
							case ExpressionType.SubtractChecked : return Convert(context, new SqlBinaryExpression(t, l, "-", r, Precedence.Subtraction));
							case ExpressionType.Coalesce        :
								{
									if (r is SqlFunction)
									{
										var c = (SqlFunction)r;

										if (c.Name == "Coalesce")
										{
											var parms = new ISqlExpression[c.Parameters.Length + 1];

											parms[0] = l;
											c.Parameters.CopyTo(parms, 1);

											return Convert(context, new SqlFunction(t, "Coalesce", parms));
										}
									}

									return Convert(context, new SqlFunction(t, "Coalesce", l, r));
								}
						}

						break;
					}

				case ExpressionType.UnaryPlus      :
				case ExpressionType.Negate         :
				case ExpressionType.NegateChecked  :
					{
						var e = (UnaryExpression)expression;
						var o = ConvertToSql(context, e.Operand, false);
						var t = e.Type;

						switch (expression.NodeType)
						{
							case ExpressionType.UnaryPlus     : return o;
							case ExpressionType.Negate        :
							case ExpressionType.NegateChecked :
								return Convert(context, new SqlBinaryExpression(t, new SqlValue(-1), "*", o, Precedence.Multiplicative));
						}

						break;
					}

				case ExpressionType.Convert        :
				case ExpressionType.ConvertChecked :
					{
						var e = (UnaryExpression)expression;
						var o = ConvertToSql(context, e.Operand, false);

						if (e.Method == null && e.IsLifted)
							return o;

						var t = e.Operand.Type;
						var s = SqlDataType.GetDataType(t);

						if (o.SystemType != null && s.Type == typeof(object))
						{
							t = o.SystemType;
							s = SqlDataType.GetDataType(t);
						}

						if (e.Type == t ||
							t.IsEnum && Enum.GetUnderlyingType(t) == e.Type ||
							e.Type.IsEnum && Enum.GetUnderlyingType(e.Type) == t)
							return o;

						return Convert(
							context,
							new SqlFunction(e.Type, "$Convert$", SqlDataType.GetDataType(e.Type), s, o));
					}

				case ExpressionType.Conditional    :
					{
						var e = (ConditionalExpression)expression;
						var s = ConvertToSql(context, e.Test,    false);
						var t = ConvertToSql(context, e.IfTrue,  false);
						var f = ConvertToSql(context, e.IfFalse, false);

						if (f is SqlFunction)
						{
							var c = (SqlFunction)f;

							if (c.Name == "CASE")
							{
								var parms = new ISqlExpression[c.Parameters.Length + 2];

								parms[0] = s;
								parms[1] = t;
								c.Parameters.CopyTo(parms, 2);

								return Convert(context, new SqlFunction(e.Type, "CASE", parms));
							}
						}

						return Convert(context, new SqlFunction(e.Type, "CASE", s, t, f));
					}

				case ExpressionType.MemberAccess :
					{
						var ma   = (MemberExpression)expression;
						var attr = GetFunctionAttribute(ma.Member);

						if (attr != null)
							return Convert(context, attr.GetExpression(ma.Member));

						var ctx = GetContext(context, expression);

						if (ctx != null)
						{
							var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

							switch (sql.Length)
							{
								case 0  : break;
								case 1  : return sql[0].Sql;
								default : throw new InvalidOperationException();
							}
						}

						break;
					}

				case ExpressionType.Parameter   :
					{
						var ctx = GetContext(context, expression);

						if (ctx != null)
						{
							var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

							switch (sql.Length)
							{
								case 0  : break;
								case 1  : return sql[0].Sql;
								default : throw new InvalidOperationException();
							}
						}

						break;
					}

				case ExpressionType.Call        :
					{
						var e = (MethodCallExpression)expression;

						if (e.IsQueryable())
						{
							if (IsSubQuery(context, e))
								return SubQueryToSql(context, e);

							if (CountBuilder.MethodNames.Concat(AggregationBuilder.MethodNames).Contains(e.Method.Name))
							{
								var ctx = GetContext(context, expression);

								if (ctx != null)
								{
									var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

									if (sql.Length != 1)
										throw new InvalidOperationException();

									return sql[0].Sql;
								}

								break;
							}

							return SubQueryToSql(context, e);
						}

						var attr = GetFunctionAttribute(e.Method);

						if (attr != null)
						{
							var parms = new List<ISqlExpression>();

							if (e.Object != null)
								parms.Add(ConvertToSql(context, e.Object, false));

							parms.AddRange(e.Arguments.Select(t => ConvertToSql(context, t, false)));

							return Convert(context, attr.GetExpression(e.Method, parms.ToArray()));
						}

						break;
					}

				case ExpressionType.Invoke :
					{
						var pi = (InvocationExpression)expression;
						var ex = pi.Expression;

						if (ex.NodeType == ExpressionType.Quote)
							ex = ((UnaryExpression)ex).Operand;

						if (ex.NodeType == ExpressionType.Lambda)
						{
							var l   = (LambdaExpression)ex;
							var dic = new Dictionary<Expression,Expression>();

							for (var i = 0; i < l.Parameters.Count; i++)
								dic.Add(l.Parameters[i], pi.Arguments[i]);

							var pie = l.Body.Convert(wpi =>
							{
								Expression ppi;
								return dic.TryGetValue(wpi, out ppi) ? ppi : wpi;
							});

							return ConvertToSql(context, pie, false);
						}

						break;
					}

				case ExpressionType.TypeIs :
					{
						var condition = new SqlQuery.SearchCondition();
						BuildSearchCondition(context, expression, condition.Conditions);
						return condition;
					}

				case (ExpressionType)ChangeTypeExpression.ChangeTypeType :
					return ConvertToSql(context, ((ChangeTypeExpression)expression).Expression, false);
			}

			if (expression.Type == typeof(bool) && _convertedPredicates.Add(expression))
			{
				var predicate = ConvertPredicate(context, expression);
				_convertedPredicates.Remove(expression);
				if (predicate != null)
					return new SqlQuery.SearchCondition(new SqlQuery.Condition(false, predicate));
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", expression);
		}

		readonly HashSet<Expression> _convertedPredicates = new HashSet<Expression>();

		#endregion

		#region IsServerSideOnly

		bool IsServerSideOnly(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var ex = (MemberExpression)expr;
						var l  = SqlProvider.ConvertMember(ex.Member);

						if (l != null)
							return IsServerSideOnly(l.Body.Unwrap());

						var attr = GetFunctionAttribute(ex.Member);
						return attr != null && attr.ServerSideOnly;
					}

				case ExpressionType.Call:
					{
						var e = (MethodCallExpression)expr;

						if (e.Method.DeclaringType == typeof(Enumerable))
						{
							if (CountBuilder.MethodNames.Concat(AggregationBuilder.MethodNames).Contains(e.Method.Name))
								return IsQueryMember(e.Arguments[0]);
						}
						else if (e.Method.DeclaringType == typeof(Queryable))
						{
							switch (e.Method.Name)
							{
								case "Any"      :
								case "All"      :
								case "Contains" : return true;
							}
						}
						else
						{
							var l = SqlProvider.ConvertMember(e.Method);

							if (l != null)
								return l.Body.Unwrap().Find(IsServerSideOnly) != null;

							var attr = GetFunctionAttribute(e.Method);
							return attr != null && attr.ServerSideOnly;
						}

						break;
					}
			}

			return false;
		}

		static bool IsQueryMember(Expression expr)
		{
			if (expr != null) switch (expr.NodeType)
			{
				case ExpressionType.Parameter    : return true;
				case ExpressionType.MemberAccess : return IsQueryMember(((MemberExpression)expr).Expression);
				case ExpressionType.Call         :
					{
						var call = (MethodCallExpression)expr;

						if (call.Method.DeclaringType == typeof(Queryable))
							return true;

						if (call.Method.DeclaringType == typeof(Enumerable) && call.Arguments.Count > 0)
							return IsQueryMember(call.Arguments[0]);

						return IsQueryMember(call.Object);
					}
			}

			return false;
		}

		#endregion

		#region CanBeConstant

		bool CanBeConstant(Expression expr)
		{
			return null == expr.Find(ex =>
			{
				if (ex is BinaryExpression || ex is UnaryExpression /*|| ex.NodeType == ExpressionType.Convert*/)
					return false;

				switch (ex.NodeType)
				{
					case ExpressionType.Constant     :
						{
							var c = (ConstantExpression)ex;

							if (c.Value == null || ExpressionHelper.IsConstant(ex.Type))
								return false;

							break;
						}

					case ExpressionType.MemberAccess :
						{
							var ma = (MemberExpression)ex;

							if (ExpressionHelper.IsConstant(ma.Member.DeclaringType) || TypeHelper.IsNullableValueMember(ma.Member))
								return false;

							break;
						}

					case ExpressionType.Call         :
						{
							var mc = (MethodCallExpression)ex;

							if (ExpressionHelper.IsConstant(mc.Method.DeclaringType) || mc.Method.DeclaringType == typeof(object))
								return false;

							var attr = GetFunctionAttribute(mc.Method);

							if (attr != null && !attr.ServerSideOnly)
								return false;

							break;
						}
				}

				return true;
			});
		}

		#endregion

		#region CanBeCompiled

		bool CanBeCompiled(Expression expr)
		{
			return null == expr.Find(ex =>
			{
				if (IsServerSideOnly(ex))
					return true;

				switch (ex.NodeType)
				{
					case ExpressionType.Parameter    :
						return ex != ParametersParam;

					case ExpressionType.MemberAccess :
						{
							var attr = GetFunctionAttribute(((MemberExpression)ex).Member);
							return attr != null && attr.ServerSideOnly;
						}

					case ExpressionType.Call         :
						{
							var attr = GetFunctionAttribute(((MethodCallExpression)ex).Method);
							return attr != null && attr.ServerSideOnly;
						}
				}

				return false;
			});
		}

		#endregion

		#region Build Constant

		readonly Dictionary<Expression,SqlValue> _constants = new Dictionary<Expression,SqlValue>();

		SqlValue BuildConstant(Expression expr, bool convertEnum)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));
			var v      = lambda.Compile()();

			if (v != null && convertEnum && v.GetType().IsEnum)
			{
				var attrs = v.GetType().GetCustomAttributes(typeof(SqlEnumAttribute), true);

				v = Map.EnumToValue(v, attrs.Length == 0);
			}

			value = new SqlValue(v);

			_constants.Add(expr, value);

			return value;
		}

		#endregion

		#region Build Parameter

		readonly Dictionary<Expression,ParameterAccessor> _parameters = new Dictionary<Expression, ParameterAccessor>();

		public readonly HashSet<Expression> AsParameters = new HashSet<Expression>();

		ParameterAccessor BuildParameter(Expression expr)
		{
			ParameterAccessor p;

			if (_parameters.TryGetValue(expr, out p))
				return p;

			string name = null;

			var newExpr = ReplaceParameter(_expressionAccessors, expr, nm => name = nm);
			var mapper  = Expression.Lambda<Func<Expression,object[],object>>(
				Expression.Convert(newExpr, typeof(object)),
				new [] { ExpressionParam, ParametersParam });

			p = new ParameterAccessor
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, name, null, MappingSchema)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p;
		}

		Expression ReplaceParameter(IDictionary<Expression,Expression> expressionAccessors, Expression expression, Action<string> setName)
		{
			return expression.Convert(expr =>
			{
				if (expr.NodeType == ExpressionType.Constant)
				{
					var c = (ConstantExpression)expr;

					if (!ExpressionHelper.IsConstant(expr.Type) || AsParameters.Contains(c))
					{
						Expression val;
						
						if (expressionAccessors.TryGetValue(expr, out val))
						{
							expr = Expression.Convert(val, expr.Type);

							if (expression.NodeType == ExpressionType.MemberAccess)
							{
								var ma = (MemberExpression)expression;
								setName(ma.Member.Name);
							}
						}
					}
				}

				return expr;
			});
		}

		#endregion

		#region Predicate Converter

		ISqlPredicate ConvertPredicate(IBuildContext context, Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Equal              :
				case ExpressionType.NotEqual           :
				case ExpressionType.GreaterThan        :
				case ExpressionType.GreaterThanOrEqual :
				case ExpressionType.LessThan           :
				case ExpressionType.LessThanOrEqual    :
					{
						var e = (BinaryExpression)expression;
						return ConvertCompare(context, expression.NodeType, e.Left, e.Right);
					}

				case ExpressionType.Call               :
					{
						var e = (MethodCallExpression)expression;

						ISqlPredicate predicate = null;

						if (e.Method.Name == "Equals" && e.Object != null && e.Arguments.Count == 1)
							return ConvertCompare(context, ExpressionType.Equal, e.Object, e.Arguments[0]);

						if (e.Method.DeclaringType == typeof(string))
						{
							switch (e.Method.Name)
							{
								case "Contains"   : predicate = ConvertLikePredicate(context, e, "%", "%"); break;
								case "StartsWith" : predicate = ConvertLikePredicate(context, e, "",  "%"); break;
								case "EndsWith"   : predicate = ConvertLikePredicate(context, e, "%", "");  break;
							}
						}
						else if (e.Method.Name == "Contains")
						{
							if (e.Method.DeclaringType == typeof(Enumerable) ||
							    TypeHelper.IsSameOrParent(typeof(IList), e.Method.DeclaringType) ||
							    TypeHelper.IsSameOrParent(typeof(ICollection<>), e.Method.DeclaringType))
							{
								predicate = ConvertInPredicate(context, e);
							}
						}
						else if (e.Method.Name == "ContainsValue" && TypeHelper.IsSameOrParent(typeof(Dictionary<,>), e.Method.DeclaringType))
						{
							var args = TypeHelper.GetGenericArguments(e.Method.DeclaringType, typeof(Dictionary<,>));
							var minf = EnumerableMethods
								.First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
								.MakeGenericMethod(args[1]);

							var expr = Expression.Call(
								minf, 
								Expression.PropertyOrField(e.Object, "Values"),
								e.Arguments[0]);

							predicate = ConvertInPredicate(context, expr);
						}
						else if (e.Method.Name == "ContainsKey" && TypeHelper.IsSameOrParent(typeof(IDictionary<,>), e.Method.DeclaringType))
						{
							var args = TypeHelper.GetGenericArguments(e.Method.DeclaringType, typeof(IDictionary<,>));
							var minf = EnumerableMethods
								.First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
								.MakeGenericMethod(args[0]);

							var expr = Expression.Call(
								minf, 
								Expression.PropertyOrField(e.Object, "Keys"),
								e.Arguments[0]);

							predicate = ConvertInPredicate(context, expr);
						}
#if !SILVERLIGHT
						else if (e.Method == ReflectionHelper.Functions.String.Like11) predicate = ConvertLikePredicate(context, e);
						else if (e.Method == ReflectionHelper.Functions.String.Like12) predicate = ConvertLikePredicate(context, e);
#endif
						else if (e.Method == ReflectionHelper.Functions.String.Like21) predicate = ConvertLikePredicate(context, e);
						else if (e.Method == ReflectionHelper.Functions.String.Like22) predicate = ConvertLikePredicate(context, e);

						if (predicate != null)
							return Convert(context, predicate);

						break;
					}

				case ExpressionType.Conditional  :
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							ConvertToSql(context, expression, false),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(true)));

				case ExpressionType.MemberAccess :
					{
						var e = (MemberExpression)expression;

						if (e.Member.Name == "HasValue" && 
							e.Member.DeclaringType.IsGenericType && 
							e.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							var expr = ConvertToSql(context, e.Expression, false);
							return Convert(context, new SqlQuery.Predicate.IsNull(expr, true));
						}

						break;
					}

				case ExpressionType.TypeIs:
					{
						var e   = (TypeBinaryExpression)expression;
						var ctx = GetContext(context, e.Expression);

						if (ctx != null && ctx.IsExpression(e.Expression, 0, RequestFor.Table).Result)
							return MakeIsPredicate(ctx, e);

						break;
					}
			}

			var ex = ConvertToSql(context, expression, false);

			if (SqlExpression.NeedsEqual(ex))
				return Convert(context, new SqlQuery.Predicate.ExprExpr(ex, SqlQuery.Predicate.Operator.Equal, new SqlValue(true)));

			return Convert(context, new SqlQuery.Predicate.Expr(ex));
		}

		#region ConvertCompare

		ISqlPredicate ConvertCompare(IBuildContext context, ExpressionType nodeType, Expression left, Expression right)
		{
			if (left.NodeType == ExpressionType.Convert && left.Type == typeof(int) && right.NodeType == ExpressionType.Constant)
			{
				var conv = (UnaryExpression)left;

				if (conv.Operand.Type == typeof(char))
				{
					left  = conv.Operand;
					right = Expression.Constant(ConvertTo<char>.From(((ConstantExpression)right).Value));
				}
			}

			if (right.NodeType == ExpressionType.Convert && right.Type == typeof(int) && left.NodeType == ExpressionType.Constant)
			{
				var conv = (UnaryExpression)right;

				if (conv.Operand.Type == typeof(char))
				{
					right = conv.Operand;
					left  = Expression.Constant(ConvertTo<char>.From(((ConstantExpression)left).Value));
				}
			}

			#region special case for char?

//			if (left.NodeType == ExpressionType.Convert && left.Type == typeof(int?) && right.NodeType == ExpressionType.Convert)
//			{
//				var convLeft  = left  as UnaryExpression;
//				var convRight = right as UnaryExpression;
//
//				if (convLeft != null && convRight != null && convLeft.Operand.Type == typeof(char?))
//				{
//					left  = convLeft.Operand;
//					right = Expression.Constant(ConvertTo<char?>.From(((ConstantExpression)convRight.Operand).Value));
//				}
//			}

			#endregion

			switch (nodeType)
			{
				case ExpressionType.Equal    :
				case ExpressionType.NotEqual :

					var p = ConvertObjectComparison(nodeType, context, left, context, right);
					if (p != null)
						return p;

					p = ConvertObjectNullComparison(context, left, right, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					p = ConvertObjectNullComparison(context, right, left, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					if (left.NodeType == ExpressionType.New || right.NodeType == ExpressionType.New)
					{
						p = ConvertNewObjectComparison(context, nodeType, left, right);
						if (p != null)
							return p;
					}

					break;
			}

			SqlQuery.Predicate.Operator op;

			switch (nodeType)
			{
				case ExpressionType.Equal             : op = SqlQuery.Predicate.Operator.Equal;          break;
				case ExpressionType.NotEqual          : op = SqlQuery.Predicate.Operator.NotEqual;       break;
				case ExpressionType.GreaterThan       : op = SqlQuery.Predicate.Operator.Greater;        break;
				case ExpressionType.GreaterThanOrEqual: op = SqlQuery.Predicate.Operator.GreaterOrEqual; break;
				case ExpressionType.LessThan          : op = SqlQuery.Predicate.Operator.Less;           break;
				case ExpressionType.LessThanOrEqual   : op = SqlQuery.Predicate.Operator.LessOrEqual;    break;
				default: throw new InvalidOperationException();
			}

			if (left.NodeType == ExpressionType.Convert || right.NodeType == ExpressionType.Convert
				|| left.NodeType == ExpressionType.MemberAccess || right.NodeType == ExpressionType.MemberAccess)
			{
				var p = ConvertEnumConversion(context, left, op, right);
				if (p != null)
					return p;
			}

			var l = ConvertToSql(context, left,  false);
			var r = ConvertToSql(context, right, true);

		    if (l is SqlField && r is SqlParameter)
		    {
		        ((SqlParameter) r).DbType = ((SqlField) l).MemberMapper.DbType;
		    }

			switch (nodeType)
			{
				case ExpressionType.Equal   :
				case ExpressionType.NotEqual:

					if (!context.SqlQuery.IsParameterDependent && (l is SqlParameter || r is SqlParameter) && l.CanBeNull() && r.CanBeNull())
						context.SqlQuery.IsParameterDependent = true;

					// | (SqlQuery(Select([]) as q), SqlValue(null))
					// | (SqlValue(null), SqlQuery(Select([]) as q))  =>

					SqlQuery q =
						l.ElementType == QueryElementType.SqlQuery &&
						r.ElementType == QueryElementType.SqlValue &&
						((SqlValue)r).Value == null &&
						((SqlQuery)l).Select.Columns.Count == 0 ?
							(SqlQuery)l :
						r.ElementType == QueryElementType.SqlQuery &&
						l.ElementType == QueryElementType.SqlValue &&
						((SqlValue)l).Value == null &&
						((SqlQuery)r).Select.Columns.Count == 0 ?
							(SqlQuery)r :
							null;

					if (q != null)
					{
						q.Select.Columns.Add(new SqlQuery.Column(q, new SqlValue(1)));
					}

					break;
			}

			if (l is SqlQuery.SearchCondition)
				l = Convert(context, new SqlFunction(typeof(bool), "CASE", l, new SqlValue(true), new SqlValue(false)));

			if (r is SqlQuery.SearchCondition)
				r = Convert(context, new SqlFunction(typeof(bool), "CASE", r, new SqlValue(true), new SqlValue(false)));

			return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
		}

		#endregion

		#region ConvertEnumConversion

		ISqlPredicate ConvertEnumConversion(IBuildContext context, Expression left, SqlQuery.Predicate.Operator op, Expression right)
		{
			Expression value;
			Expression operand;

			if (left is MemberExpression)
			{
				operand = left;
				value   = right;
			}
			else if (left.NodeType == ExpressionType.Convert && ((UnaryExpression)left).Operand is MemberExpression)
			{
				operand = ((UnaryExpression)left).Operand;
				value   = right;
			}
			else if (right is MemberExpression)
			{
				operand = right;
				value   = left;
			}
			else if (right.NodeType == ExpressionType.Convert && ((UnaryExpression)right).Operand is MemberExpression)
			{
				operand = ((UnaryExpression)right).Operand;
				value   = left;
			}
			else if (left.NodeType == ExpressionType.Convert)
			{
				operand = ((UnaryExpression)left).Operand;
				value   = right;
			}
			else
			{
				operand = ((UnaryExpression)right).Operand;
				value = left;
			}

			var type = operand.Type;

			if (!TypeHelper.IsEnumOrNullableEnum(type))
				return null;

			var dic = new Dictionary<object,object>();

			var nullValue = MappingSchema.GetNullValue(type);

			if (nullValue != null)
				dic.Add(nullValue, null);

			var mapValues = MappingSchema.GetMapValues(type);

			if (mapValues != null)
				foreach (var mv in mapValues)
					if (!dic.ContainsKey(mv.OrigValue))
						dic.Add(mv.OrigValue, mv.MapValues[0]);

			switch (value.NodeType)
			{
				case ExpressionType.Constant:
					{
						var name = Enum.GetName(type, ((ConstantExpression)value).Value);

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
						if (name == null)
							return null;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

						var origValue = Enum.Parse(type, name, false);
						var mapValue  = origValue;

						if (!(operand is MemberExpression))
						{
							if (!dic.TryGetValue(origValue, out mapValue))
								return null;
						}

						ISqlExpression l, r;

						SqlValue sqlValue;

						if (left.NodeType == ExpressionType.Convert)
						{
							l = ConvertToSql(context, operand, false);
							r = sqlValue = new SqlValue(mapValue);
						}
						else
						{
							r = ConvertToSql(context, operand, false);
							l = sqlValue = new SqlValue(mapValue);
						}

						if (operand is MemberExpression)
						{
							var me = (MemberExpression)operand;
							var memberAccessor = TypeAccessor.GetAccessor(me.Member.DeclaringType)[me.Member.Name];
							sqlValue.SetEnumConverter(memberAccessor, MappingSchema);
						}


						return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
					}

				case ExpressionType.Convert:
					{
						value = ((UnaryExpression)value).Operand;

						var l = ConvertToSql(context, operand, false, false);
						var r = ConvertToSql(context, value,   false, false);

						MemberAccessor memberAccessor = null;

						if (operand is MemberExpression)
						{
							// is it even possible that operand is not MemberExpression?
							// if no, then we can remove this two last uses of SetEnumConverter(type, map)
							// and other depending code
							// At least currently there is no test coverage for this method and I didn't
							// manage to create such test
							var me = (MemberExpression)operand;
							memberAccessor = TypeAccessor.GetAccessor(me.Member.DeclaringType)[me.Member.Name];
						}

						if (l is SqlValueBase)
						{
							if (memberAccessor != null)
							{
								((SqlValueBase)l).SetEnumConverter(memberAccessor, MappingSchema);
							}
							else
							{
								((SqlValueBase)l).SetEnumConverter(type, MappingSchema);
							}
						}

						if (r is SqlValueBase)
						{
							if (memberAccessor != null)
							{
								((SqlValueBase)r).SetEnumConverter(memberAccessor, MappingSchema);
							}
							else
							{
								((SqlValueBase)r).SetEnumConverter(type, MappingSchema);
							}
						}

						return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
					}
			}

			return null;
		}

		#endregion

		#region ConvertObjectNullComparison

		ISqlPredicate ConvertObjectNullComparison(IBuildContext context, Expression left, Expression right, bool isEqual)
		{
			if (right.NodeType == ExpressionType.Constant && ((ConstantExpression)right).Value == null)
			{
				if (left.NodeType == ExpressionType.MemberAccess || left.NodeType == ExpressionType.Parameter)
				{
					var ctx = GetContext(context, left);

					if (ctx != null && ctx.IsExpression(left, 0, RequestFor.Object).Result ||
						left.NodeType == ExpressionType.Parameter && ctx.IsExpression(left, 0, RequestFor.Field).Result)
					{
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
					}
				}
			}

			return null;
		}

		#endregion

		#region ConvertObjectComparison

		public ISqlPredicate ConvertObjectComparison(
			ExpressionType nodeType,
			IBuildContext  leftContext,
			Expression     left,
			IBuildContext  rightContext,
			Expression     right)
		{
			var qsl = GetContext(leftContext,  left);
			var qsr = GetContext(rightContext, right);

			var sl = qsl != null && qsl.IsExpression(left,  0, RequestFor.Object).Result;
			var sr = qsr != null && qsr.IsExpression(right, 0, RequestFor.Object).Result;

			bool      isNull;
			SqlInfo[] lcols;

			var rmembers = new Dictionary<MemberInfo,Expression>(new MemberInfoComparer());

			if (sl == false && sr == false)
			{
				var lmembers = new Dictionary<MemberInfo,Expression>(new MemberInfoComparer());

				if (!ProcessProjection(lmembers, left) && !ProcessProjection(rmembers, right))
					return null;

				if (lmembers.Count == 0)
				{
					var r = right;
					right = left;
					left  = r;

					var c = rightContext;
					rightContext = leftContext;
					leftContext  = c;

					var q = qsr;
					qsl = q;

					sr = false;

					var lm = lmembers;
					lmembers = rmembers;
					rmembers = lm;
				}

				isNull = right is ConstantExpression && ((ConstantExpression)right).Value == null;
				lcols  = lmembers.Select(m => new SqlInfo(m.Key) { Sql = ConvertToSql(leftContext, m.Value, false) }).ToArray();
			}
			else
			{
				if (sl == false)
				{
					var r = right;
					right = left;
					left  = r;

					var c = rightContext;
					rightContext = leftContext;
					leftContext  = c;

					var q = qsr;
					qsl = q;

					sr = false;
				}

				isNull = right is ConstantExpression && ((ConstantExpression)right).Value == null;
				lcols  = qsl.ConvertToSql(left, 0, ConvertFlags.Key);

				if (!sr)
					ProcessProjection(rmembers, right);
			}

			if (lcols.Length == 0)
				return null;

			var condition = new SqlQuery.SearchCondition();

			foreach (var lcol in lcols)
			{
				if (lcol.Members.Count == 0)
					throw new InvalidOperationException();

				ISqlExpression rcol = null;

				var lmember = lcol.Members[lcol.Members.Count - 1];

				if (sr)
				{
					var info = rightContext.ConvertToSql(Expression.MakeMemberAccess(right, lmember), 0, ConvertFlags.Field).Single();
					rcol = info.Sql;
				}
				else
				{
					if (rmembers.Count != 0)
					{
						var info = rightContext.ConvertToSql(rmembers[lmember], 0, ConvertFlags.Field)[0];
						rcol = info.Sql;
					}
				}

				var rex =
					isNull ?
						new SqlValue(right.Type, null) :
						rcol ?? GetParameter(right, lmember);

				var predicate = Convert(leftContext, new SqlQuery.Predicate.ExprExpr(
					lcol.Sql,
					nodeType == ExpressionType.Equal ? SqlQuery.Predicate.Operator.Equal : SqlQuery.Predicate.Operator.NotEqual,
					rex));

				condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			if (nodeType == ExpressionType.NotEqual)
				foreach (var c in condition.Conditions)
					c.IsOr = true;

			return condition;
		}

		ISqlPredicate ConvertNewObjectComparison(IBuildContext context, ExpressionType nodeType, Expression left, Expression right)
		{
			left  = FindExpression(left);
			right = FindExpression(right);

			var condition = new SqlQuery.SearchCondition();

			if (left.NodeType != ExpressionType.New)
			{
				var temp = left;
				left  = right;
				right = temp;
			}

			var newRight = right as NewExpression;
			var newExpr  = (NewExpression)left;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
			if (newExpr.Members == null)
				return null;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

			for (var i = 0; i < newExpr.Arguments.Count; i++)
			{
				var lex = ConvertToSql(context, newExpr.Arguments[i], false);
				var rex =
					newRight != null ?
						ConvertToSql(context, newRight.Arguments[i], false) :
						GetParameter(right, newExpr.Members[i]);

				var predicate = Convert(context,
					new SqlQuery.Predicate.ExprExpr(
						lex,
						nodeType == ExpressionType.Equal ? SqlQuery.Predicate.Operator.Equal : SqlQuery.Predicate.Operator.NotEqual,
						rex));

				condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			if (nodeType == ExpressionType.NotEqual)
				foreach (var c in condition.Conditions)
					c.IsOr = true;

			return condition;
		}

		ISqlExpression GetParameter(Expression ex, MemberInfo member)
		{
			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var par    = ReplaceParameter(_expressionAccessors, ex, _ => {});
			var expr   = Expression.MakeMemberAccess(par.Type == typeof(object) ? Expression.Convert(par, member.DeclaringType) : par, member);
			var mapper = Expression.Lambda<Func<Expression,object[],object>>(
				Expression.Convert(expr, typeof(object)),
				new [] { ExpressionParam, ParametersParam });

			var p = new ParameterAccessor
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, member.Name, null, MappingSchema)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p.SqlParameter;
		}

		static Expression FindExpression(Expression expr)
		{
			var ret = expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.Convert      :
						{
							var e = (UnaryExpression)expr;

							return
								e.Operand.NodeType == ExpressionType.ArrayIndex &&
								((BinaryExpression)e.Operand).Left == ParametersParam;
						}

					case ExpressionType.MemberAccess :
					case ExpressionType.New          :
						return true;
				}

				return false;
			});

			if (ret == null)
				throw new InvalidOperationException();

			return ret;
		}

		#endregion

		#region ConvertInPredicate

		private ISqlPredicate ConvertInPredicate(IBuildContext context, MethodCallExpression expression)
		{
			var e        = expression;
			var argIndex = e.Object != null ? 0 : 1;
			var arr      = e.Object ?? e.Arguments[0];
			var arg      = e.Arguments[argIndex];

			ISqlExpression expr = null;

			var ctx = GetContext(context, arg);

			if (ctx is TableBuilder.TableContext &&
			    ctx.SqlQuery != context.SqlQuery &&
			    ctx.IsExpression(arg, 0, RequestFor.Object).Result)
			{
				expr = ctx.SqlQuery;
			}

			if (expr == null)
			{
				var sql = ConvertExpressions(context, arg, ConvertFlags.Key);

				if (sql.Length == 1 && sql[0].Members.Count == 0)
					expr = sql[0].Sql;
				else
					expr = new SqlExpression(
						'\x1' + string.Join(",", sql.Select(s => s.Members[s.Members.Count - 1].Name).ToArray()),
						sql.Select(s => s.Sql).ToArray());
			}

			MemberAccessor memberAccessor = null;

			if (arg is MemberExpression)
			{
				var me = (MemberExpression)arg;
				if (TypeHelper.IsEnumOrNullableEnum(me.Type))
				{
					memberAccessor = TypeAccessor.GetAccessor(me.Member.DeclaringType)[me.Member.Name];
				}
			}
			
			switch (arr.NodeType)
			{
				case ExpressionType.NewArrayInit :
					{
						var newArr = (NewArrayExpression)arr;

						if (newArr.Expressions.Count == 0)
							return new SqlQuery.Predicate.Expr(new SqlValue(false));

						var exprs  = new ISqlExpression[newArr.Expressions.Count];

						for (var i = 0; i < newArr.Expressions.Count; i++)
						{
							exprs[i] = ConvertToSql(context, newArr.Expressions[i], false, false);

							if (memberAccessor != null && exprs[i] is SqlValue)
							{
								((SqlValue)exprs[i]).SetEnumConverter(memberAccessor, MappingSchema);
							}
						}

						return new SqlQuery.Predicate.InList(expr, false, exprs);
					}

				default :

					if (CanBeCompiled(arr))
					{
						var p = BuildParameter(arr).SqlParameter;
						p.IsQueryParameter = false;
						if (memberAccessor != null)
						{
							p.SetEnumConverter(memberAccessor, MappingSchema);
						}
						return new SqlQuery.Predicate.InList(expr, false, p);
					}

					break;
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", expression);
		}

		#endregion

		#region LIKE predicate

		ISqlPredicate ConvertLikePredicate(IBuildContext context, MethodCallExpression expression, string start, string end)
		{
			var e = expression;
			var o = ConvertToSql(context, e.Object,       false);
			var a = ConvertToSql(context, e.Arguments[0], false);

			if (a is SqlValue)
			{
				var value = ((SqlValue)a).Value;

				if (value == null)
					throw new LinqException("NULL cannot be used as a LIKE predicate parameter.");

				return value.ToString().IndexOfAny(new[] { '%', '_' }) < 0?
					new SqlQuery.Predicate.Like(o, false, new SqlValue(start + value + end), null):
					new SqlQuery.Predicate.Like(o, false, new SqlValue(start + EscapeLikeText(value.ToString()) + end), new SqlValue('~'));
			}

			if (a is SqlParameter)
			{
				var p  = (SqlParameter)a;
				var ep = (from pm in CurrentSqlParameters where pm.SqlParameter == p select pm).First();

				ep = new ParameterAccessor
				{
					Expression   = ep.Expression,
					Accessor     = ep.Accessor,
					SqlParameter = new SqlParameter(ep.Expression.Type, p.Name, p.Value, GetLikeEscaper(start, end))
				};

				CurrentSqlParameters.Add(ep);

				return new SqlQuery.Predicate.Like(o, false, ep.SqlParameter, new SqlValue('~'));
			}

			var mi = ReflectionHelper.Expressor<string>.MethodExpressor(_ => _.Replace("", ""));
			var ex =
				Expression.Call(
				Expression.Call(
				Expression.Call(
					e.Arguments[0],
						mi, Expression.Constant("~"), Expression.Constant("~~")),
						mi, Expression.Constant("%"), Expression.Constant("~%")),
						mi, Expression.Constant("_"), Expression.Constant("~_"));

			var expr = ConvertToSql(context, ConvertExpression(ex), false);

			if (!string.IsNullOrEmpty(start))
				expr = new SqlBinaryExpression(typeof(string), new SqlValue("%"), "+", expr);

			if (!string.IsNullOrEmpty(end))
				expr = new SqlBinaryExpression(typeof(string), expr, "+", new SqlValue("%"));

			return new SqlQuery.Predicate.Like(o, false, expr, new SqlValue('~'));
		}

		ISqlPredicate ConvertLikePredicate(IBuildContext context, MethodCallExpression expression)
		{
			var e  = expression;
			var a1 = ConvertToSql(context, e.Arguments[0], false);
			var a2 = ConvertToSql(context, e.Arguments[1], false);

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ConvertToSql(context, e.Arguments[2], false);

			return new SqlQuery.Predicate.Like(a1, false, a2, a3);
		}

		static string EscapeLikeText(string text)
		{
			if (text.IndexOfAny(new[] { '%', '_' }) < 0)
				return text;

			var builder = new StringBuilder(text.Length);

			foreach (var ch in text)
			{
				switch (ch)
				{
					case '%':
					case '_':
					case '~':
						builder.Append('~');
						break;
				}

				builder.Append(ch);
			}

			return builder.ToString();
		}

		static Converter<object,object> GetLikeEscaper(string start, string end)
		{
			return value => value == null? null: start + EscapeLikeText(value.ToString()) + end;
		}

		#endregion

		#region MakeIsPredicate

		internal ISqlPredicate MakeIsPredicate(TableBuilder.TableContext table, Type typeOperand)
		{
			if (typeOperand == table.ObjectType && !table.InheritanceMapping.Any(m => m.Type == typeOperand))
				return Convert(table, new SqlQuery.Predicate.Expr(new SqlValue(true)));

			return MakeIsPredicate(
				table, table.InheritanceMapping, table.InheritanceDiscriminators, typeOperand,
				name => table.SqlTable.Fields.Values.First(f => f.Name == name));
		}

		internal ISqlPredicate MakeIsPredicate(
			IBuildContext                     context,
			List<InheritanceMappingAttribute> inheritanceMapping,
			List<string>                      inheritanceDiscriminators,
			Type                              toType,
			Func<string,ISqlExpression>       getSql)
		{
			var mapping = inheritanceMapping
				.Select((m,i) => new { m, i })
				.Where ( m => m.m.Type == toType && !m.m.IsDefault)
				.ToList();

			switch (mapping.Count)
			{
				case 0 :
					{
						var cond = new SqlQuery.SearchCondition();

						foreach (var m in inheritanceMapping.Select((m,i) => new { m, i }).Where(m => !m.m.IsDefault))
						{
							cond.Conditions.Add(
								new SqlQuery.Condition(
									false, 
									Convert(context,
										new SqlQuery.Predicate.ExprExpr(
											getSql(inheritanceDiscriminators[m.i]),
											SqlQuery.Predicate.Operator.NotEqual,
											new SqlValue(m.m.Code)))));
						}

						return cond;
					}

				case 1 :
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							getSql(inheritanceDiscriminators[mapping[0].i]),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(mapping[0].m.Code)));

				default:
					{
						var cond = new SqlQuery.SearchCondition();

						foreach (var m in mapping)
						{
							cond.Conditions.Add(
								new SqlQuery.Condition(
									false,
									Convert(context,
										new SqlQuery.Predicate.ExprExpr(
											getSql(inheritanceDiscriminators[m.i]),
											SqlQuery.Predicate.Operator.Equal,
											new SqlValue(m.m.Code))),
									true));
						}

						return cond;
					}
			}
		}

		ISqlPredicate MakeIsPredicate(IBuildContext context, TypeBinaryExpression expression)
		{
			var typeOperand = expression.TypeOperand;
			var table       = new TableBuilder.TableContext(this, new BuildInfo((IBuildContext)null, Expression.Constant(null), new SqlQuery()), typeOperand);

			if (typeOperand == table.ObjectType && !table.InheritanceMapping.Any(m => m.Type == typeOperand))
				return Convert(table, new SqlQuery.Predicate.Expr(new SqlValue(true)));

			var mapping = table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => m.m.Type == typeOperand && !m.m.IsDefault).ToList();
			var isEqual = true;

			if (mapping.Count == 0)
			{
				mapping = table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => !m.m.IsDefault).ToList();
				isEqual = false;
			}

			Expression expr = null;

			foreach (var m in mapping)
			{
				var field = table.SqlTable.Fields[table.InheritanceDiscriminators[m.i]];
				var ttype = field.MemberMapper.MemberAccessor.TypeAccessor.OriginalType;
				var obj   = expression.Expression;

				if (obj.Type != ttype)
					obj = Expression.Convert(expression.Expression, ttype);

				var left = Expression.PropertyOrField(obj, field.Name);
				var code = m.m.Code;

				if (code == null)
					code = TypeHelper.GetDefaultValue(left.Type);
				else if (left.Type != code.GetType())
					code = MappingSchema.ConvertChangeType(code, left.Type);

				Expression right = Expression.Constant(code, left.Type);

				var e = isEqual ? Expression.Equal(left, right) : Expression.NotEqual(left, right);

				expr = expr != null ? Expression.AndAlso(expr, e) : e;
			}

			return ConvertPredicate(context, expr);
		}

		#endregion

		#endregion

		#region Search Condition Builder

		void BuildSearchCondition(IBuildContext context, Expression expression, List<SqlQuery.Condition> conditions)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.And     :
				case ExpressionType.AndAlso :
					{
						var e = (BinaryExpression)expression;

						BuildSearchCondition(context, e.Left,  conditions);
						BuildSearchCondition(context, e.Right, conditions);

						break;
					}

				case ExpressionType.Or     :
				case ExpressionType.OrElse :
					{
						var e           = (BinaryExpression)expression;
						var orCondition = new SqlQuery.SearchCondition();

						BuildSearchCondition(context, e.Left,  orCondition.Conditions);
						orCondition.Conditions[orCondition.Conditions.Count - 1].IsOr = true;
						BuildSearchCondition(context, e.Right, orCondition.Conditions);

						conditions.Add(new SqlQuery.Condition(false, orCondition));

						break;
					}

				case ExpressionType.Not    :
					{
						var e            = expression as UnaryExpression;
						var notCondition = new SqlQuery.SearchCondition();

						BuildSearchCondition(context, e.Operand, notCondition.Conditions);

						if (notCondition.Conditions.Count == 1 && notCondition.Conditions[0].Predicate is SqlQuery.Predicate.NotExpr)
						{
							var p = notCondition.Conditions[0].Predicate as SqlQuery.Predicate.NotExpr;
							p.IsNot = !p.IsNot;
							conditions.Add(notCondition.Conditions[0]);
						}
						else
							conditions.Add(new SqlQuery.Condition(true, notCondition));

						break;
					}

				default                    :
					var predicate = ConvertPredicate(context, expression);

					if (predicate is SqlQuery.Predicate.Expr)
					{
						var expr = ((SqlQuery.Predicate.Expr)predicate).Expr1;

						if (expr.ElementType == QueryElementType.SearchCondition)
						{
							var sc = (SqlQuery.SearchCondition)expr;

							if (sc.Conditions.Count == 1)
							{
								conditions.Add(sc.Conditions[0]);
								break;
							}
						}
					}

					conditions.Add(new SqlQuery.Condition(false, predicate));

					break;
			}
		}

		#endregion

		#region CanBeTranslatedToSql

		bool CanBeTranslatedToSql(IBuildContext context, Expression expr, bool canBeCompiled)
		{
			List<Expression> ignoredMembers = null;

			return null == expr.Find(pi =>
			{
				if (ignoredMembers != null)
				{
					if (pi != ignoredMembers[ignoredMembers.Count - 1])
						throw new InvalidOperationException();

					if (ignoredMembers.Count == 1)
						ignoredMembers = null;
					else
						ignoredMembers.RemoveAt(ignoredMembers.Count - 1);

					return false;
				}

				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var ma   = (MemberExpression)pi;
							var attr = GetFunctionAttribute(ma.Member);

							if (attr == null && !TypeHelper.IsNullableValueMember(ma.Member))
							{
								if (canBeCompiled)
								{
									var ctx = GetContext(context, pi);

									if (ctx == null)
										return !CanBeCompiled(pi);

									if (ctx.IsExpression(pi, 0, RequestFor.Object).Result)
										return !CanBeCompiled(pi);

									ignoredMembers = ma.Expression.GetMembers();
								}
							}

							break;
						}

					case ExpressionType.Parameter    :
						{
							var ctx = GetContext(context, pi);

							if (ctx == null)
								if (canBeCompiled)
									return !CanBeCompiled(pi);

							break;
						}

					case ExpressionType.Call         :
						{
							var e = (MethodCallExpression)pi;

							if (e.Method.DeclaringType != typeof(Enumerable))
							{
								var attr = GetFunctionAttribute(e.Method);

								if (attr == null && canBeCompiled)
									return !CanBeCompiled(pi);
							}

							break;
						}

					case ExpressionType.TypeIs       : return canBeCompiled;
					case ExpressionType.TypeAs       :
					case ExpressionType.New          : return true;

					case ExpressionType.NotEqual     :
					case ExpressionType.Equal        :
						{
							var e = (BinaryExpression)pi;

							Expression obj = null;

							if (e.Left.NodeType == ExpressionType.Constant && ((ConstantExpression)e.Left).Value == null)
								obj = e.Right;
							else if (e.Right.NodeType == ExpressionType.Constant && ((ConstantExpression)e.Right).Value == null)
								obj = e.Left;

							if (obj != null)
							{
								var ctx = GetContext(context, obj);

								if (ctx != null)
								{
									if (ctx.IsExpression(obj, 0, RequestFor.Table).      Result ||
									    ctx.IsExpression(obj, 0, RequestFor.Association).Result)
									{
										ignoredMembers = obj.GetMembers();
									}
								}
							}

							break;
						}
				}

				return false;
			});
		}

		#endregion

		#region Helpers

		public IBuildContext GetContext([JetBrains.Annotations.NotNull] IBuildContext current, Expression expression)
		{
			var root = expression.GetRootObject();

			for (; current != null; current = current.Parent)
				if (current.IsExpression(root, 0, RequestFor.Root).Result)
					return current;

			return null;
		}

		SqlFunctionAttribute GetFunctionAttribute(ICustomAttributeProvider member)
		{
			var attrs = member.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

			if (attrs.Length == 0)
				return null;

			SqlFunctionAttribute attr = null;

			foreach (SqlFunctionAttribute a in attrs)
			{
				if (a.SqlProvider == SqlProvider.Name)
				{
					attr = a;
					break;
				}

				if (a.SqlProvider == null)
					attr = a;
			}

			return attr;
		}

		internal TableFunctionAttribute GetTableFunctionAttribute(ICustomAttributeProvider member)
		{
			var attrs = member.GetCustomAttributes(typeof(TableFunctionAttribute), true);

			if (attrs.Length == 0)
				return null;

			TableFunctionAttribute attr = null;

			foreach (TableFunctionAttribute a in attrs)
			{
				if (a.SqlProvider == SqlProvider.Name)
				{
					attr = a;
					break;
				}

				if (a.SqlProvider == null)
					attr = a;
			}

			return attr;
		}

		public ISqlExpression Convert(IBuildContext context, ISqlExpression expr)
		{
			SqlProvider.SqlQuery = context.SqlQuery;
			return SqlProvider.ConvertExpression(expr);
		}

		public ISqlPredicate Convert(IBuildContext context, ISqlPredicate predicate)
		{
			SqlProvider.SqlQuery = context.SqlQuery;
			return SqlProvider.ConvertPredicate(predicate);
		}

		public ISqlExpression ConvertTimeSpanMember(IBuildContext context, MemberExpression expression)
		{
			if (expression.Member.DeclaringType == typeof(TimeSpan))
			{
				switch (expression.Expression.NodeType)
				{
					case ExpressionType.Subtract       :
					case ExpressionType.SubtractChecked:

						Sql.DateParts datePart;

						switch (expression.Member.Name)
						{
							case "TotalMilliseconds" : datePart = Sql.DateParts.Millisecond; break;
							case "TotalSeconds"      : datePart = Sql.DateParts.Second;      break;
							case "TotalMinutes"      : datePart = Sql.DateParts.Minute;      break;
							case "TotalHours"        : datePart = Sql.DateParts.Hour;        break;
							case "TotalDays"         : datePart = Sql.DateParts.Day;         break;
							default                  : return null;
						}

						var e = (BinaryExpression)expression.Expression;

						return new SqlFunction(
							typeof(int),
							"DateDiff",
							new SqlValue(datePart),
							ConvertToSql(context, e.Right, false),
							ConvertToSql(context, e.Left,  false));
				}
			}

			return null;
		}

		internal ISqlExpression ConvertSearchCondition(IBuildContext context, ISqlExpression sqlExpression)
		{
			if (sqlExpression is SqlQuery.SearchCondition)
			{
				if (sqlExpression.CanBeNull())
				{
					var notExpr = new SqlQuery.SearchCondition
					{
						Conditions = { new SqlQuery.Condition(true, new SqlQuery.Predicate.Expr(sqlExpression, sqlExpression.Precedence)) }
					};

					return Convert(context, new SqlFunction(sqlExpression.SystemType, "CASE", sqlExpression, new SqlValue(1), notExpr, new SqlValue(0), new SqlValue(null)));
				}

				return Convert(context, new SqlFunction(sqlExpression.SystemType, "CASE", sqlExpression, new SqlValue(1), new SqlValue(0)));
			}

			return sqlExpression;
		}

		public bool ProcessProjection(Dictionary<MemberInfo,Expression> members, Expression expression)
		{
			switch (expression.NodeType)
			{
				// new { ... }
				//
				case ExpressionType.New        :
					{
						var expr = (NewExpression)expression;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
						if (expr.Members == null)
							return false;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

						for (var i = 0; i < expr.Members.Count; i++)
						{
							var member = expr.Members[i];

							members.Add(member, expr.Arguments[i]);

							if (member is MethodInfo)
								members.Add(TypeHelper.GetPropertyByMethod((MethodInfo)member), expr.Arguments[i]);
						}

						return true;
					}

				// new MyObject { ... }
				//
				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)expression;
						var dic  = TypeAccessor.GetAccessor(expr.Type)
							.Select((m,i) => new { m, i })
							.ToDictionary(_ => _.m.MemberInfo.Name, _ => _.i);

						foreach (var binding in expr.Bindings.Cast<MemberAssignment>().OrderBy(b => dic.ContainsKey(b.Member.Name) ? dic[b.Member.Name] : 1000000))
						{
							members.Add(binding.Member, binding.Expression);

							if (binding.Member is MethodInfo)
								members.Add(TypeHelper.GetPropertyByMethod((MethodInfo)binding.Member), binding.Expression);
						}

						return true;
					}

				// .Select(p => everything else)
				//
				default                        :
					return false;
			}
		}

		public void ReplaceParent(IBuildContext oldParent, IBuildContext newParent)
		{
			foreach (var context in Contexts)
				if (context != newParent)
					if (context.Parent == oldParent)
						context.Parent = newParent;
		}

		#endregion
	}
}
