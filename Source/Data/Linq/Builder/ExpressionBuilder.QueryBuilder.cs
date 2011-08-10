using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	partial class ExpressionBuilder
	{
		#region BuildExpression

		readonly HashSet<Expression> _skippedExpressions = new HashSet<Expression>();

		public Expression BuildExpression(IBuildContext context, Expression expression)
		{
			var newExpr = expression.Convert2(pi =>
			{
				if (_skippedExpressions.Contains(pi))
					return new ExpressionHelper.ConvertInfo(pi, true);

				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));

							var ma = (MemberExpression)pi;

							if (SqlProvider.ConvertMember(ma.Member) != null)
								break;

							var ctx = GetContext(context, pi);

							if (ctx != null)
								return new ExpressionHelper.ConvertInfo(ctx.BuildExpression(pi, 0));

							var ex = ma.Expression;

							if (ex != null && ex.NodeType == ExpressionType.Constant)
							{
								// field = localVariable
								//
								var c = _expressionAccessors[ex];
								return new ExpressionHelper.ConvertInfo(
									Expression.MakeMemberAccess(Expression.Convert(c, ex.Type), ma.Member));
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							if (pi == ParametersParam)
								break;

							var ctx = GetContext(context, pi);

							if (ctx != null)
								return new ExpressionHelper.ConvertInfo(ctx.BuildExpression(pi, 0));

							throw new NotImplementedException();
						}

					case ExpressionType.Constant:
						{
							if (ExpressionHelper.IsConstant(pi.Type))
								break;

							if (_expressionAccessors.ContainsKey(pi))
								return new ExpressionHelper.ConvertInfo(Expression.Convert(_expressionAccessors[pi], pi.Type));

							throw new NotImplementedException();
						}

					case ExpressionType.Coalesce:

						if (pi.Type == typeof(string) && MappingSchema.GetDefaultNullValue<string>() != null)
							return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));

						if (CanBeTranslatedToSql(context, ConvertExpression(pi), true))
							return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));

						break;

					case ExpressionType.Conditional:

						if (CanBeTranslatedToSql(context, ConvertExpression(pi), true))
							return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));
						break;

					case ExpressionType.Call:
						{
							var ce = (MethodCallExpression)pi;

							if (IsGroupJoinSource(context, ce))
							{
								foreach (var arg in ce.Arguments.Skip(1))
									if (!_skippedExpressions.Contains(arg))
										_skippedExpressions.Add(arg);

								break;
							}

							if (IsSubQuery(context, ce))
								return new ExpressionHelper.ConvertInfo(GetSubQuery(context, ce).BuildExpression(null, 0));

							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));
						}

						break;
				}

				if (EnforceServerSide(context))
				{
					switch (pi.NodeType)
					{
						case ExpressionType.MemberInit:
						case ExpressionType.New:
						case ExpressionType.Convert:
							break;
						default:
							if (CanBeCompiled(pi))
								break;
							return new ExpressionHelper.ConvertInfo(BuildSql(context, pi));
					}
				}

				return new ExpressionHelper.ConvertInfo(pi);
			});

			return newExpr;
		}

		static bool EnforceServerSide(IBuildContext context)
		{
			return context.SqlQuery.Select.IsDistinct;
		}

		#endregion

		#region BuildSql

		Expression BuildSql(IBuildContext context, Expression expression)
		{
			var sqlex = ConvertToSqlExpression(context, expression);
			var idx   = context.SqlQuery.Select.Add(sqlex);

			idx = context.ConvertToParentIndex(idx, context);

			var field = BuildSql(expression.Type, idx);

			return field;
		}

		public Expression BuildSql(Type type, int idx, MethodInfo checkNullFunction, Expression context)
		{
			var expr = Expression.Call(DataReaderParam, ReflectionHelper.DataReader.GetValue, Expression.Constant(idx));

			if (checkNullFunction != null)
				expr = Expression.Call(null, checkNullFunction, expr, context);

			Expression mapper;

			if (type.IsEnum)
			{
				mapper =
					Expression.Convert(
						Expression.Call(
							Expression.Constant(MappingSchema),
							ReflectionHelper.MapSchema.MapValueToEnum,
								expr,
								Expression.Constant(type)),
						type);
			}
			else
			{
				MethodInfo mi;

				if (!ReflectionHelper.MapSchema.Converters.TryGetValue(type, out mi))
				{
					//throw new LinqException("Cannot find converter for the '{0}' type.", type.FullName);

					mapper =
						Expression.Convert(
							Expression.Call(
								Expression.Constant(MappingSchema),
								ReflectionHelper.MapSchema.ChangeType,
									expr,
									Expression.Constant(type)),
							type);
				}
				else
				{
					mapper = Expression.Call(Expression.Constant(MappingSchema), mi, expr);
				}
			}

			return mapper;
		}

		public Expression BuildSql(Type type, int idx)
		{
			return BuildSql(type, idx, null, null);
		}

		#endregion

		#region PreferServerSide

		bool PreferServerSide(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var pi = (MemberExpression)expr;
						var l  = SqlProvider.ConvertMember(pi.Member);

						if (l != null)
						{
							var info = l.Body.Unwrap();

							if (l.Parameters.Count == 1 && pi.Expression != null)
								info = info.Convert(wpi => wpi == l.Parameters[0] ? pi.Expression : wpi);

							return info.Find(PreferServerSide) != null;
						}

						var attr = GetFunctionAttribute(pi.Member);
						return attr != null && attr.PreferServerSide && !CanBeCompiled(expr);
					}

				case ExpressionType.Call:
					{
						var pi = (MethodCallExpression)expr;
						var e  = pi;
						var l  = SqlProvider.ConvertMember(e.Method);

						if (l != null)
							return l.Body.Unwrap().Find(PreferServerSide) != null;

						var attr = GetFunctionAttribute(e.Method);
						return attr != null && attr.PreferServerSide && !CanBeCompiled(expr);
					}
			}

			return false;
		}

		#endregion
	}
}
