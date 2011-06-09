using System;
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

			var  ctx  = new ExpressionContext(parent, sequence, condition);
			var  expr = ConvertExpression(condition.Body.Unwrap());

			if (checkForSubQuery && CheckSubQueryForWhere(ctx, expr, out makeHaving))
			{
				sequence.Parent = prevParent;
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

			sequence.Parent = prevParent;

			return sequence;
		}

		bool CheckSubQueryForWhere(IBuildContext context, Expression expression, out bool makeHaving)
		{
			//var checkParameter = true; //context.IsExpression(expression, 0, RequestFor.ScalarExpression);
			var makeSubQuery   = false;
			var isHaving       = false;
			var isWhere        = false;

			expression.Visit(expr =>
			{
				if (_subQueryExpressions != null && _subQueryExpressions.Contains(expr))
				{
					makeSubQuery = true;
					isWhere      = true;
					return false;
				}

				//if (IsSubQuery(context, expr))
				//	return isWhere = true;

				var stopWalking = false;

				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)expr;

							if (TypeHelper.IsNullableValueMember(ma.Member))
								break;

							if (ConvertMember(ma.Member) == null)
							{
								var ctx = GetContext(context, expr);

								if (ctx != null)
								{
									if (ctx.IsExpression(expr, 0, RequestFor.Expression))
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
								if (ctx.IsExpression(expr, 0, RequestFor.Expression))
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
				(ctx.IsExpression(null, 0, RequestFor.Expression) ||
				 ctx.IsExpression(null, 0, RequestFor.Field)))
			{
				ctx.ConvertToIndex(null, 0, ConvertFlags.Field);
			}

			return ctx;
		}

		ISqlExpression SubQueryToSql(IBuildContext context, MethodCallExpression expression)
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

			/*
			if (expr.NodeType == ExpressionType.Call)
			{
				var call = (MethodCallExpression)expr;

				if (call.Method.Name == "Any" && (call.Method.DeclaringType == typeof(Queryable) || call.Method.DeclaringType == typeof(Enumerable)))
					return ((SqlQuery.Predicate.FuncLike) result.Where.SearchCondition.Conditions[0].Predicate).Function;
			}
			*/

			return sequence.SqlQuery;
		}

		#endregion

		#region IsSubquery

		bool IsSubQuery(IBuildContext context, MethodCallExpression call)
		{
			if (call.IsQueryable())
			{
				var arg = call.Arguments[0];

				if (AggregationBuilder.MethodNames.Contains(call.Method.Name))
					while (arg.NodeType == ExpressionType.Call && ((MethodCallExpression) arg).Method.Name == "Select")
						arg = ((MethodCallExpression)arg).Arguments[0];

				var mc = arg as MethodCallExpression;

				while (mc != null)
				{
					if (!mc.IsQueryable())
						return false;

					mc = mc.Arguments[0] as MethodCallExpression;
				}

				return arg.NodeType == ExpressionType.Call || IsSubQuerySource(context, arg);
			}

			return false;
		}

		bool IsSubQuery1(IBuildContext context, MethodCallExpression call)
		{
			if (call.IsQueryable())
			{
				var expr = call.Arguments[0];

				if (AggregationBuilder.MethodNames.Contains(call.Method.Name))
					while (expr.NodeType == ExpressionType.Call && ((MethodCallExpression) expr).Method.Name == "Select")
						expr = ((MethodCallExpression)expr).Arguments[0];

				while (expr.NodeType == ExpressionType.Call)
				{
					var c = (MethodCallExpression)expr;

					if (!c.IsQueryable())
						break;

					expr = c.Arguments[0];
				}

				if (expr.NodeType == ExpressionType.Call)
					return ((MethodCallExpression)expr).IsQueryable();

				if (IsSubQuerySource(context, expr))
					return true;

				//if (IsIEnumerableType(expr))
				//	return !CanBeCompiled(expr);
			}

			return false;
		}

		bool IsSubQuerySource(IBuildContext context, Expression expr)
		{
			if (expr == null)
				return false;

			var ctx = GetContext(context, expr);

			if (ctx != null && ctx.IsExpression(expr, 0, RequestFor.Object))
				return true;

			while (expr != null && expr.NodeType == ExpressionType.MemberAccess)
				expr = ((MemberExpression)expr).Expression;

			return expr != null && expr.NodeType == ExpressionType.Constant;
		}

		static bool IsIEnumerableType(Expression expr)
		{
			var type = expr.Type;

			var res  = (type.IsClass || type.IsInterface)
				&& type != typeof(string)
				&& type != typeof(byte[])
				&& TypeHelper.IsSameOrParent(typeof(IEnumerable), type);

			if (res && expr.NodeType == ExpressionType.MemberAccess)
				res = TypeHelper.GetAttributes(type, typeof(IgnoreIEnumerableAttribute)).Length == 0;

			return res;
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
							var l  = ConvertMember(ma.Member);

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

		LambdaExpression ConvertMember(MemberInfo mi)
		{
			var lambda = SqlProvider.ConvertMember(mi);

			if (lambda == null)
			{
				var attrs = mi.GetCustomAttributes(typeof(MethodExpressionAttribute), true);

				if (attrs.Length == 0)
					return null;

				MethodExpressionAttribute attr = null;

				foreach (MethodExpressionAttribute a in attrs)
				{
					if (a.SqlProvider == SqlProvider.Name)
					{
						attr = a;
						break;
					}

					if (a.SqlProvider == null)
						attr = a;
				}

				if (attr != null)
				{
					var call = Expression.Lambda<Func<LambdaExpression>>(
						Expression.Convert(Expression.Call(mi.DeclaringType, attr.MethodName, Array<Type>.Empty), typeof(LambdaExpression)));

					lambda = call.Compile()();
				}
			}

			return lambda;
		}

		Expression ConvertMethod(MethodCallExpression pi)
		{
			var l = ConvertMember(pi.Method);

			if (l == null)
				return null;

			var ef    = l.Body.Unwrap();
			var parms = new Dictionary<string,int>(l.Parameters.Count);
			var pn    = pi.Method.IsStatic ? 0 : -1;

			foreach (var p in l.Parameters)
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
			var lambda = ConvertMember(pi.Constructor);

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
								var sql = ConvertToSql(context, arg);
								var mi  = expr.Members[i];

								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return new SqlInfo { Sql = sql, Member = mi };
							})
							.ToArray();
					}

				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)expression;
						var dic  = TypeAccessor.GetAccessor(expr.Type)
							.Select((m,i) => new { m, i })
							.ToDictionary(_ => _.m.MemberInfo, _ => _.i);

						return expr.Bindings
							.Where(b => b is MemberAssignment)
							.Cast<MemberAssignment>()
							.OrderBy(b => dic[b.Member])
							.Select(a =>
							{
								var sql = ConvertToSql(context, a.Expression);
								var mi  = a.Member;

								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return new SqlInfo { Sql = sql, Member = mi };
							})
							.ToArray();
					}
			}

			var ctx = GetContext(context, expression);

			if (ctx != null)
			{
				if (ctx.IsExpression(expression, 0, RequestFor.Object))
					return ctx.ConvertToSql(expression, 0, queryConvertFlag);
			}

			return new[] { new SqlInfo { Sql = ConvertToSql(context, expression) } };
		}

		public ISqlExpression ConvertToSqlAndBuild(IBuildContext context, Expression expression)
		{
			var expr = ConvertExpression(expression);
			return ConvertToSql(context, expr);
		}

		public ISqlExpression ConvertToSql(IBuildContext context, Expression expression)
		{
			/*
			var qlen = queries.Length;

			if (expression.NodeType == ExpressionType.Parameter && qlen == 1 && queries[0] is QuerySource.Scalar)
			{
				var ma = (QuerySource.Scalar)queries[0];
				return ConvertToSql(ma.Lambda, ma.Lambda.Body, ma.Sources);
			}
			*/

			if (CanBeConstant(expression))
				return BuildConstant(expression);

			if (CanBeCompiled(expression))
				return BuildParameter(expression).SqlParameter;

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
				case ExpressionType.OrElse:
				case ExpressionType.Not:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					{
						var condition = new SqlQuery.SearchCondition();
						BuildSearchCondition(context, expression, condition.Conditions);
						return condition;
					}

				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.Divide:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.Or:
				case ExpressionType.Power:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				case ExpressionType.Coalesce:
					{
						var e = (BinaryExpression)expression;
						var l = ConvertToSql(context, e.Left);
						var r = ConvertToSql(context, e.Right);
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

				case ExpressionType.UnaryPlus:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					{
						var e = (UnaryExpression)expression;
						var o = ConvertToSql(context, e.Operand);
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
						var o = ConvertToSql(context, e.Operand);

						if (e.Method == null && e.IsLifted)
							return o;

						if (e.Operand.Type.IsEnum && Enum.GetUnderlyingType(e.Operand.Type) == e.Type)
							return o;

						return Convert(
							context,
							new SqlFunction(e.Type, "$Convert$", SqlDataType.GetDataType(e.Type), SqlDataType.GetDataType(e.Operand.Type), o));
					}

				case ExpressionType.Conditional   :
					{
						var e = (ConditionalExpression)expression;
						var s = ConvertToSql(context, e.Test);
						var t = ConvertToSql(context, e.IfTrue);
						var f = ConvertToSql(context, e.IfFalse);

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

				case ExpressionType.MemberAccess:
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

				case ExpressionType.Parameter:
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

				case ExpressionType.Call:
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
								parms.Add(ConvertToSql(context, e.Object));

							parms.AddRange(e.Arguments.Select(t => ConvertToSql(context, t)));

							return Convert(context, attr.GetExpression(e.Method, parms.ToArray()));
						}

						break;
					}

				case ExpressionType.Invoke:
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

							return ConvertToSql(context, pie);
						}

						break;
					}

				case (ExpressionType)ChangeTypeExpression.ChangeTypeType :
					return ConvertToSql(context, ((ChangeTypeExpression)expression).Expression);
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", expression);
		}

		#endregion

		#region IsServerSideOnly

		bool IsServerSideOnly(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var ex = (MemberExpression)expr;
						var l  = ConvertMember(ex.Member);

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
							var l = ConvertMember(e.Method);

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
				if (ex is BinaryExpression || ex is UnaryExpression || ex.NodeType == ExpressionType.Convert)
					return false;

				switch (ex.NodeType)
				{
					case ExpressionType.Constant:
						{
							var c = (ConstantExpression)ex;

							if (c.Value == null || ExpressionHelper.IsConstant(ex.Type))
								return false;

							break;
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)ex;

							if (ExpressionHelper.IsConstant(ma.Member.DeclaringType) || TypeHelper.IsNullableValueMember(ma.Member))
								return false;

							break;
						}

					case ExpressionType.Call:
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
					case ExpressionType.Parameter :
						return ex != ParametersParam;

					case ExpressionType.MemberAccess :
						{
							var attr = GetFunctionAttribute(((MemberExpression)ex).Member);
							return attr != null && attr.ServerSideOnly;
						}

					case ExpressionType.Call :
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

		SqlValue BuildConstant(Expression expr)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));

			var v = lambda.Compile()();

			if (v != null && v.GetType().IsEnum)
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
				SqlParameter = new SqlParameter(expr.Type, name, null)
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
						var val = expressionAccessors[expr];

						expr = Expression.Convert(val, expr.Type);

						if (expression.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)expression;
							setName(ma.Member.Name);
						}
					}
				}

				return expr;
			});
		}

		#endregion

		#region ConvertEnumerable

		ISqlExpression ConvertEnumerable(IBuildContext context, MethodCallExpression expression)
		{
			return SubQueryToSql(context, expression);

			//throw new NotImplementedException();

			/*
			QueryField field = queries.Length == 1 && queries[0] is QuerySource.GroupBy ? queries[0] : null;

			if (field == null)
				field = GetField(lambda, expression.Arguments[0], queries);

			while (field is QuerySource.SubQuerySourceColumn)
				field = ((QuerySource.SubQuerySourceColumn)field).SourceColumn;

			if (field is QuerySource.GroupJoin && expression.Method.Name == "Count")
			{
				var ex = ((QuerySource.GroupJoin)field).Counter.GetExpressions(this)[0];
				return ex;
			}

			if (!(field is QuerySource.GroupBy))
				throw new LinqException("'{0}' cannot be converted to SQL.", expression);

			var groupBy = (QuerySource.GroupBy)field;
			var expr    = ConvertEnumerable(expression, groupBy);

			if (queries.Length == 1 && queries[0] is QuerySource.SubQuery)
			{
				var subQuery  = (QuerySource.SubQuery)queries[0];
				var column    = groupBy.FindField(new QueryField.ExprColumn(groupBy, expr, null));
				var subColumn = subQuery.EnsureField(column);

				expr = subColumn.GetExpressions(this)[0];
			}

			return expr;
			*/
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
						else if (e.Method.DeclaringType == typeof(Enumerable))
						{
							switch (e.Method.Name)
							{
								case "Contains" :
									predicate = ConvertInPredicate(context, e);
									break;
							}
						}
						else if (TypeHelper.IsSameOrParent(typeof(IList), e.Method.DeclaringType))
						{
							switch (e.Method.Name)
							{
								case "Contains" :
									predicate = ConvertInPredicate(context, e);
									break;
							}
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

				case ExpressionType.Conditional:
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							ConvertToSql(context, expression),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(true)));

				case ExpressionType.MemberAccess:
					{
						var e = (MemberExpression)expression;

						if (e.Member.Name == "HasValue" && 
							e.Member.DeclaringType.IsGenericType && 
							e.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							var expr = ConvertToSql(context, e.Expression);
							return Convert(context, new SqlQuery.Predicate.IsNull(expr, true));
						}

						break;
					}

				case ExpressionType.TypeIs:
					{
						throw new NotImplementedException();

						/*
						var e     = expression as TypeBinaryExpression;
						var table = GetSource(lambda, e.Expression, queries) as QuerySource.Table;

						if (table != null && table.InheritanceMapping.Count > 0)
							return MakeIsPredicate(table, e.TypeOperand);

						break;
						*/
					}
			}

			var ex = ConvertToSql(context, expression);

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

			if (left.NodeType == ExpressionType.Convert || right.NodeType == ExpressionType.Convert)
			{
				var p = ConvertEnumConversion(context, left, op, right);
				if (p != null)
					return p;
			}

			var l = ConvertToSql(context, left);
			var r = ConvertToSql(context, right);

			switch (nodeType)
			{
				case ExpressionType.Equal   :
				case ExpressionType.NotEqual:

					if (!context.SqlQuery.ParameterDependent && (l is SqlParameter || r is SqlParameter) && l.CanBeNull() && r.CanBeNull())
						context.SqlQuery.ParameterDependent = true;

					break;
			}

			if (l is SqlQuery.SearchCondition)
				l = Convert(context, new SqlFunction(typeof(bool), "CASE", l, new SqlValue(true), new SqlValue(false)));
			//l = Convert(new SqlFunction("CASE",
			//	l, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)l) }), new SqlValue(false),
			//	new SqlValue(false)));

			if (r is SqlQuery.SearchCondition)
				r = Convert(context, new SqlFunction(typeof(bool), "CASE", r, new SqlValue(true), new SqlValue(false)));
			//r = Convert(new SqlFunction("CASE",
			//	r, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)r) }), new SqlValue(false),
			//	new SqlValue(false)));

			return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
		}

		#endregion

		#region ConvertEnumConversion

		ISqlPredicate ConvertEnumConversion(IBuildContext context, Expression left, SqlQuery.Predicate.Operator op, Expression right)
		{
			UnaryExpression conv;
			Expression      value;

			if (left.NodeType == ExpressionType.Convert)
			{
				conv  = (UnaryExpression)left;
				value = right;
			}
			else
			{
				conv  = (UnaryExpression)right;
				value = left;
			}

			var operand = conv.Operand;
			var type    = operand.Type;

			if (!type.IsEnum)
				return null;

			var dic = new Dictionary<object, object>();

			var nullValue = MappingSchema.GetNullValue(type);

			if (nullValue != null)
				dic.Add(nullValue, null);

			var mapValues = MappingSchema.GetMapValues(type);

			if (mapValues != null)
				foreach (var mv in mapValues)
					if (!dic.ContainsKey(mv.OrigValue))
						dic.Add(mv.OrigValue, mv.MapValues[0]);

			if (dic.Count == 0)
				return null;

			switch (value.NodeType)
			{
				case ExpressionType.Constant:
					{
						var    origValue = Enum.Parse(type, Enum.GetName(type, ((ConstantExpression)value).Value), false);
						object mapValue;

						if (!dic.TryGetValue(origValue, out mapValue))
							return null;

						ISqlExpression l, r;

						if (left.NodeType == ExpressionType.Convert)
						{
							l = ConvertToSql(context, operand);
							r = new SqlValue(mapValue);
						}
						else
						{
							r = ConvertToSql(context, operand);
							l = new SqlValue(mapValue);
						}

						return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
					}

				case ExpressionType.Convert:
					{
						value = ((UnaryExpression)value).Operand;

						var l = ConvertToSql(context, operand);
						var r = ConvertToSql(context, value);

						if (l is SqlParameter) ((SqlParameter)l).SetEnumConverter(type, MappingSchema);
						if (r is SqlParameter) ((SqlParameter)r).SetEnumConverter(type, MappingSchema);

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

					if (ctx != null && ctx.IsExpression(left, 0, RequestFor.Object) ||
						left.NodeType == ExpressionType.Parameter && ctx.IsExpression(left, 0, RequestFor.Field))
					{
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
					}

					/*
					var field = GetField(lambda, left, queries);

					if (field is QuerySource.GroupJoin)
					{
						var join = (QuerySource.GroupJoin)field;
						var expr = join.CheckNullField.GetExpressions(this)[0];

						return Convert(context, new SqlQuery.Predicate.IsNull(expr, !isEqual));
					}

					if (field is QuerySource || field == null && left.NodeType == ExpressionType.Parameter)
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
					*/
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

			var sl = qsl != null && qsl.IsExpression(left,  0, RequestFor.Object);
			var sr = qsr != null && qsr.IsExpression(right, 0, RequestFor.Object);

			bool      isNull;
			SqlInfo[] lcols;

			var rmembers = new Dictionary<MemberInfo,Expression>();

			if (sl == false && sr == false)
			{
				var lmembers = new Dictionary<MemberInfo,Expression>();

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
				lcols  =
					(from m in lmembers
					select new { sql = leftContext.ConvertToSql(m.Value, 0, ConvertFlags.Field)[0], member = m.Key } into mm
					select new SqlInfo { Sql = mm.sql.Sql, Member = mm.member }).ToArray();
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
					//qsr = qsl;
					qsl = q;

					//sl = true;
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
				if (lcol.Member == null)
					throw new InvalidOperationException();

				ISqlExpression rcol = null;

				if (sr)
				{
					var info = rightContext.ConvertToSql(Expression.MakeMemberAccess(right, lcol.Member), 0, ConvertFlags.Field).Single();
					rcol = info.Sql;
				}
				else
				{
					if (rmembers.Count != 0)
					{
						var info = rightContext.ConvertToSql(rmembers[lcol.Member], 0, ConvertFlags.Field)[0];
						rcol = info.Sql;
					}
				}

				var rex =
					isNull ?
						new SqlValue(right.Type, null) :
						rcol ?? GetParameter(right, lcol.Member);

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
				var lex = ConvertToSql(context, newExpr.Arguments[i]);
				var rex =
					newRight != null ?
						ConvertToSql(context, newRight.Arguments[i]) :
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
				SqlParameter = new SqlParameter(expr.Type, member.Name, null)
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
				throw new NotImplementedException();

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
			    ctx.IsExpression(arg, 0, RequestFor.Object))
			{
				expr = ctx.SqlQuery;
			}

			if (expr == null)
			{
				var sql = ConvertExpressions(context, arg, ConvertFlags.Key);

				if (sql.Length == 1 && sql[0].Member == null)
					expr = sql[0].Sql; //expr = ConvertToSql(context, arg);
				else
					expr = new SqlExpression(
						'\x1' + string.Join(",", sql.Select(s => s.Member.Name).ToArray()),
						sql.Select(s => s.Sql).ToArray());
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
							exprs[i] = ConvertToSql(context, newArr.Expressions[i]);

						return new SqlQuery.Predicate.InList(expr, false, exprs);
					}

				default :

					if (CanBeCompiled(arr))
					{
						var p = BuildParameter(arr).SqlParameter;
						p.IsQueryParameter = false;
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
			var o = ConvertToSql(context, e.Object);
			var a = ConvertToSql(context, e.Arguments[0]);

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

				_parameters.Add(e, ep);
				CurrentSqlParameters.Add(ep);

				return new SqlQuery.Predicate.Like(o, false, ep.SqlParameter, new SqlValue('~'));
			}

			return null;
		}

		ISqlPredicate ConvertLikePredicate(IBuildContext context, MethodCallExpression expression)
		{
			var e  = expression;
			var a1 = ConvertToSql(context, e.Arguments[0]);
			var a2 = ConvertToSql(context, e.Arguments[1]);

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ConvertToSql(context, e.Arguments[2]);

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

		ISqlPredicate MakeIsPredicate(IBuildContext context, QuerySource.Table table, Type typeOperand)
		{
			if (typeOperand == table.ObjectType && table.InheritanceMapping.Count(m => m.Type == typeOperand) == 0)
				return Convert(context, new SqlQuery.Predicate.Expr(new SqlValue(true)));

			var mapping = table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => m.m.Type == typeOperand && !m.m.IsDefault).ToList();

			switch (mapping.Count)
			{
				case 0:
					{
						var cond = new SqlQuery.SearchCondition();

						foreach (var m in table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => !m.m.IsDefault))
						{
							cond.Conditions.Add(
								new SqlQuery.Condition(
									false, 
									Convert(context,
										new SqlQuery.Predicate.ExprExpr(
											table.Columns[table.InheritanceDiscriminators[m.i]].Field,
											SqlQuery.Predicate.Operator.NotEqual,
											new SqlValue(m.m.Code)))));
						}

						return cond;
					}

				case 1:
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							table.Columns[table.InheritanceDiscriminators[mapping[0].i]].Field,
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
											table.Columns[table.InheritanceDiscriminators[m.i]].Field,
											SqlQuery.Predicate.Operator.Equal,
											new SqlValue(m.m.Code))),
									true));
						}

						return cond;
					}
			}
		}

		#endregion

		#endregion

		#region Search Condition Builder

		void BuildSearchCondition(IBuildContext context, Expression expression, List<SqlQuery.Condition> conditions)
		{
			/*if (IsSubQuery(context, expression))
			{
				var cond = BuildConditionSubQuery(context, expression);

				if (cond != null)
				{
					conditions.Add(cond);
					return;
				}
			}*/

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var e = (BinaryExpression)expression;

						BuildSearchCondition(context, e.Left,  conditions);
						BuildSearchCondition(context, e.Right, conditions);

						break;
					}

				case ExpressionType.OrElse:
					{
						var e           = (BinaryExpression)expression;
						var orCondition = new SqlQuery.SearchCondition();

						BuildSearchCondition(context, e.Left,  orCondition.Conditions);
						orCondition.Conditions[orCondition.Conditions.Count - 1].IsOr = true;
						BuildSearchCondition(context, e.Right, orCondition.Conditions);

						conditions.Add(new SqlQuery.Condition(false, orCondition));

						break;
					}

				case ExpressionType.Not:
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

				default:
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

		#region BuildConditionSubQuery

		SqlQuery.Condition BuildConditionSubQuery(IBuildContext context, Expression expr)
		{
			if (expr.NodeType != ExpressionType.Call)
				return null;

			var call = (MethodCallExpression)expr;

			if (!call.IsQueryable())
				return null;

			SqlQuery.Condition       cond = null;
			Func<SqlQuery.Condition> func = null;

			switch (call.Method.Name)
			{
				case "Any" :

					if (call.Arguments.Count == 1)
						func = () => BuildAnyCondition(context, SetType.Any, call, null, null);
					else if (call.Arguments.Count == 2)
						func = () => BuildAnyCondition(context, SetType.Any, call, (LambdaExpression)call.Arguments[1], null);
					else
						return null;

					break;

				case "All" :

					if (call.Arguments.Count == 2)
						func = () => BuildAnyCondition(context, SetType.All, call, (LambdaExpression)call.Arguments[1], null);
					else
						return null;

					break;

				case "Contains":

					if (call.Method.DeclaringType == typeof(Queryable))
					{
						if (call.Arguments.Count == 2)
						{
							var seq = call.Arguments[0];
							var ex  = call.Arguments[1];

							func = () =>
							{
								var param  = Expression.Parameter(ex.Type, ex.NodeType == ExpressionType.Parameter ? ((ParameterExpression)ex).Name : "t");
								var lambda = Expression.Lambda(Expression.Equal(param, ex), param);
								return BuildAnyCondition(context, SetType.In, (MethodCallExpression)seq, lambda, ex);
							};
						}
						else
							return null;

						break;

						/*
						Expression s = null;
						call.IsQueryableMethod2("Contains", seq => s = seq, ex =>
						{
							func = () =>
							{
								var param  = Expression.Parameter(ex.Type, ex.NodeType == ExpressionType.Parameter ? ((ParameterExpression)ex).Name : "t");
								var lambda = new LambdaInfo(Expression.Equal(param, ex), param);
								return ConvertAnyCondition(SetType.In, s, lambda, ex);
							};
							return true;
						});
						*/
					}

					break;
			}

			if (func != null)
			{
				cond = func();
			}

			return cond;
		}

		#endregion

		#endregion

		#region BuildAny

		enum SetType { Any, All, In }

		SqlQuery.Condition BuildAnyCondition(
			IBuildContext context, SetType setType, MethodCallExpression expr, LambdaExpression lambda, Expression inExpr)
		{
			/*
			var sql = context.SqlQuery;
			var cs  = _convertSource;

			CurrentSql = new SqlQuery();

			var associationList = new Dictionary<QuerySource,QuerySource>();

			_convertSource = (s,l) =>
			{
				var t = s as QuerySource.Table;

				if (t != null && t.ParentAssociation != null)
				{
					if (ParentQueries.Count > 0)
					{
						foreach (var parentQuery in ParentQueries)
						{
							var parent = parentQuery.Parent;

							while (parent is QuerySource.SubQuerySourceColumn)
								parent = ((QuerySource.SubQuerySourceColumn)parent).SourceColumn;

							if (parent.Find(t.ParentAssociation))
							{
								var orig = t;
								t = CreateTable(new SqlQuery(), l);

								associationList.Add(t, orig);

								var csql = CurrentSql.From.Tables.Count == 0 ? t.SqlQuery : CurrentSql;

								foreach (var c in orig.ParentAssociationJoin.Condition.Conditions)
								{
									var predicate = (SqlQuery.Predicate.ExprExpr)c.Predicate;
									csql.Where
										.Expr(predicate.Expr1)
										.Equal
										.Field(t.Columns[((SqlField)predicate.Expr2).Name].Field);
								}

								s = t;

								break;
							}
						}
					}
				}
				else
					s = cs(s, l);

				return s;
			};
			*/

			var query = GetSubQuery(context, expr);
			var any   = query.SqlQuery;

			//_convertSource = cs;

			if (lambda != null)
			{
				if (setType == SetType.All)
				{
					var e  = Expression.Not(lambda.Body);
					//var pi = new NotParseInfo(e, lambda.Body);

					lambda = Expression.Lambda(e, lambda.Parameters.ToArray());
				}

				if (inExpr == null || query.SqlQuery.Select.Columns.Count != 1)
					BuildWhere(context, query, lambda, true);
			}

			any.ParentSql = context.SqlQuery;

			//CurrentSql    = sql;

			SqlQuery.Condition cond;

			if (inExpr != null && query.SqlQuery.Select.Columns.Count == 1)
			{
				//query.Select(this);
				var ex = ConvertToSql(context, inExpr);
				cond = new SqlQuery.Condition(false, new SqlQuery.Predicate.InSubQuery(ex, false, any));
			}
			else
				cond = new SqlQuery.Condition(setType == SetType.All, new SqlQuery.Predicate.FuncLike(SqlFunction.CreateExists(any)));

			return cond;
		}

		#endregion

		#region CanBeTranslatedToSql

		bool CanBeTranslatedToSql(IBuildContext context, Expression expr, bool canBeCompiled)
		{
			return null == expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma   = (MemberExpression)pi;
							var attr = GetFunctionAttribute(ma.Member);

							if (attr == null && !TypeHelper.IsNullableValueMember(ma.Member))
								goto case ExpressionType.Parameter;

							break;
						}

					case ExpressionType.Parameter:
						{
							if (canBeCompiled && GetContext(context, pi) == null)
								return !CanBeCompiled(pi);
							break;
						}

					case ExpressionType.Call:
						{
							var e = pi as MethodCallExpression;

							if (e.Method.DeclaringType != typeof(Enumerable))
							{
								var attr = GetFunctionAttribute(e.Method);

								if (attr == null && canBeCompiled)
									return !CanBeCompiled(pi);
							}

							break;
						}

					case ExpressionType.New: return true;
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
				if (current.IsExpression(root, 0, RequestFor.Root))
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
							ConvertToSql(context, e.Right),
							ConvertToSql(context, e.Left));
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

						foreach (var binding in expr.Bindings.Cast<MemberAssignment>().OrderBy(b => dic[b.Member.Name]))
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

		#endregion
	}
}
