﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Reflection;

	partial class ExpressionBuilder
	{
		#region BuildExpression

		readonly HashSet<Expression> _skippedExpressions = new HashSet<Expression>();

		public Expression BuildExpression(IBuildContext context, Expression expression)
		{
			var newExpr = expression.Convert2(expr =>
			{
				if (_skippedExpressions.Contains(expr))
					return new ExpressionHelper.ConvertInfo(expr, true);

				if (expr.Find(IsNoneSqlMember) != null)
					return new ExpressionHelper.ConvertInfo(expr);

				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							if (IsServerSideOnly(expr) || PreferServerSide(expr))
								return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));

							var ma = (MemberExpression)expr;

							if (SqlProvider.ConvertMember(ma.Member) != null)
								break;

							var ctx = GetContext(context, expr);

							if (ctx != null)
								return new ExpressionHelper.ConvertInfo(ctx.BuildExpression(expr, 0));

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
							if (expr == ParametersParam)
								break;

							var ctx = GetContext(context, expr);

							if (ctx != null)
								return new ExpressionHelper.ConvertInfo(ctx.BuildExpression(expr, 0));

							break;
						}

					case ExpressionType.Constant:
						{
							if (ExpressionHelper.IsConstant(expr.Type))
								break;

							if (_expressionAccessors.ContainsKey(expr))
								return new ExpressionHelper.ConvertInfo(Expression.Convert(_expressionAccessors[expr], expr.Type));

							break;
						}

					case ExpressionType.Coalesce:

						if (expr.Type == typeof(string) && MappingSchema.GetDefaultNullValue<string>() != null)
							return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));

						if (CanBeTranslatedToSql(context, ConvertExpression(expr), true))
							return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));

						break;

					case ExpressionType.Conditional:

						if (CanBeTranslatedToSql(context, ConvertExpression(expr), true))
							return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));
						break;

					case ExpressionType.Call:
						{
							var ce = (MethodCallExpression)expr;

							if (IsGroupJoinSource(context, ce))
							{
								foreach (var arg in ce.Arguments.Skip(1))
									if (!_skippedExpressions.Contains(arg))
										_skippedExpressions.Add(arg);

								break;
							}

							if (IsSubQuery(context, ce))
							{
								if (TypeHelper.IsSameOrParent(typeof(IEnumerable), expr.Type) && expr.Type != typeof(string) && !expr.Type.IsArray)
									return new ExpressionHelper.ConvertInfo(BuildMultipleQuery(context, expr));

								return new ExpressionHelper.ConvertInfo(GetSubQuery(context, ce).BuildExpression(null, 0));
							}

							if (IsServerSideOnly(expr) || PreferServerSide(expr))
								return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));
						}

						break;
				}

				if (EnforceServerSide(context))
				{
					switch (expr.NodeType)
					{
						case ExpressionType.MemberInit :
						case ExpressionType.New        :
						case ExpressionType.Convert    :
							break;

						default                        :
							if (CanBeCompiled(expr))
								break;
							return new ExpressionHelper.ConvertInfo(BuildSql(context, expr));
					}
				}

				return new ExpressionHelper.ConvertInfo(expr);
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
			var sqlex = ConvertToSqlExpression(context, expression, true);
			var idx   = context.SqlQuery.Select.Add(sqlex);

			idx = context.ConvertToParentIndex(idx, context);

			var field = BuildSql(expression.Type, idx);

			return field;
		}

		public Expression BuildSql(MemberAccessor ma, int idx, MethodInfo checkNullFunction, Expression context)
		{
			var expr = Expression.Call(DataReaderParam, ReflectionHelper.DataReader.GetValue, Expression.Constant(idx));

			if (checkNullFunction != null)
				expr = Expression.Call(null, checkNullFunction, expr, context);

			Expression mapper;

			if (TypeHelper.IsEnumOrNullableEnum(ma.Type))
			{
				var type = TypeHelper.ToNullable(ma.Type);
				mapper =
					Expression.Convert(
						Expression.Call(
							Expression.Constant(MappingSchema),
							ReflectionHelper.MapSchema.MapValueToEnumWithMemberAccessor,
								expr,
								Expression.Constant(ma)),
						type);
			}
			else
			{
				MethodInfo mi;

				if (!ReflectionHelper.MapSchema.Converters.TryGetValue(ma.Type, out mi))
				{
					mapper =
						Expression.Convert(
							Expression.Call(
								Expression.Constant(MappingSchema),
								ReflectionHelper.MapSchema.ChangeType,
									expr,
									Expression.Constant(ma.Type)),
							ma.Type);
				}
				else
				{
					mapper = Expression.Call(Expression.Constant(MappingSchema), mi, expr);
				}
			}

			return mapper;
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

		public Expression BuildSql(MemberAccessor ma, int idx)
		{
			return BuildSql(ma, idx, null, null);
		}

		#endregion

		#region IsNonSqlMember

		bool IsNoneSqlMember(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var me = (MemberExpression)expr;

						var om = (
							from c in Contexts.OfType<TableBuilder.TableContext>()
							where c.ObjectType == me.Member.DeclaringType
							select c.ObjectMapper
						).FirstOrDefault();

						return om != null && om.Associations.All(a => !TypeHelper.Equals(a.MemberAccessor.MemberInfo, me.Member)) && om[me.Member.Name, true] == null;
					}
			}

			return false;
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

		#region Build Mapper

		public Expression BuildBlock(Expression expression)
		{
#if FW4 || SILVERLIGHT

			if (IsBlockDisable || BlockExpressions.Count == 0)
				return expression;

			BlockExpressions.Add(expression);

			expression = Expression.Block(BlockVariables, BlockExpressions);

			BlockVariables.  Clear();
			BlockExpressions.Clear();

#endif

			return expression;
		}

		public Expression<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>> BuildMapper<T>(Expression expr)
		{
			var type = typeof(T);

			if (expr.Type != type)
				expr = Expression.Convert(expr, type);

			var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
				BuildBlock(expr), new []
				{
					ContextParam,
					DataContextParam,
					DataReaderParam,
					ExpressionParam,
					ParametersParam,
				});

			return mapper;
		}

		#endregion

		#region BuildMultipleQuery

		interface IMultipleQueryHelper
		{
			Expression GetSubquery(
				ExpressionBuilder       builder,
				Expression              expression,
				ParameterExpression     paramArray,
				IEnumerable<Expression> parameters);
		}

		class MultipleQueryHelper<TRet> : IMultipleQueryHelper
		{
			public Expression GetSubquery(
				ExpressionBuilder       builder,
				Expression              expression,
				ParameterExpression     paramArray,
				IEnumerable<Expression> parameters)
			{
				var lambda      = Expression.Lambda<Func<IDataContext,object[],TRet>>(
					expression,
					Expression.Parameter(typeof(IDataContext), "ctx"),
					paramArray);
				var queryReader = CompiledQuery.Compile(lambda);

				return Expression.Call(
					null,
					ReflectionHelper.Expressor<object>.MethodExpressor(_ => ExecuteSubQuery(null, null, null)),
						ContextParam,
						Expression.NewArrayInit(typeof(object), parameters),
						Expression.Constant(queryReader)
					);
			}

			static TRet ExecuteSubQuery(
				QueryContext                     queryContext,
				object[]                         parameters,
				Func<IDataContext,object[],TRet> queryReader)
			{
				var db = queryContext.GetDataContext();

				try
				{
					return queryReader(db.DataContextInfo.DataContext, parameters);
				}
				finally
				{
					queryContext.ReleaseDataContext(db);
				}
			}
		}

		public Expression BuildMultipleQuery(IBuildContext context, Expression expression)
		{
			if (!Common.Configuration.Linq.AllowMultipleQuery)
				throw new LinqException("Multiple queries are not allowed. Set the 'BLToolkit.Common.Configuration.Linq.AllowMultipleQuery' flag to 'true' to allow multiple queries.");

			var parameters = new HashSet<ParameterExpression>();

			expression.Visit(e =>
			{
				if (e.NodeType == ExpressionType.Lambda)
					foreach (var p in ((LambdaExpression)e).Parameters)
						parameters.Add(p);
			});

			// Convert associations.
			//
			expression = expression.Convert(e =>
			{
				switch (e.NodeType)
				{
					case ExpressionType.MemberAccess :
						{
							var root = e.GetRootObject();

							if (root != null &&
								root.NodeType == ExpressionType.Parameter &&
								!parameters.Contains((ParameterExpression)root))
							{
								var res = context.IsExpression(e, 0, RequestFor.Association);

								if (res.Result)
								{
									var table = (TableBuilder.AssociatedTableContext)res.Context;

									if (table.IsList)
									{
										var ttype  = typeof(Table<>).MakeGenericType(table.ObjectType);
										var tbl    = Activator.CreateInstance(ttype);
										var method = typeof(LinqExtensions)
											.GetMethod("Where", BindingFlags.NonPublic | BindingFlags.Static)
											.MakeGenericMethod(e.Type, table.ObjectType, ttype);

										var me = (MemberExpression)e;
										var op = Expression.Parameter(table.ObjectType, "t");

										parameters.Add(op);

										Expression ex = null;

										for (var i = 0; i < table.Association.ThisKey.Length; i++)
										{
											var field1 = table.ParentAssociation.SqlTable.Fields[table.Association.ThisKey [i]];
											var field2 = table.                  SqlTable.Fields[table.Association.OtherKey[i]];

											var ee = Expression.Equal(
												Expression.MakeMemberAccess(op,            field2.MemberMapper.MemberAccessor.MemberInfo),
												Expression.MakeMemberAccess(me.Expression, field1.MemberMapper.MemberAccessor.MemberInfo));

											ex = ex == null ? ee : Expression.AndAlso(ex, ee);
										}

										return Expression.Call(null, method, Expression.Constant(tbl), Expression.Lambda(ex, op));
									}
								}
							}

							break;
						}
				}

				return e;
			});

			var paramex = Expression.Parameter(typeof(object[]), "ps");
			var parms   = new List<Expression>();

			// Convert parameters.
			//
			expression = expression.Convert(e =>
			{
				var root = e.GetRootObject();

				if (root != null &&
					root.NodeType == ExpressionType.Parameter &&
					!parameters.Contains((ParameterExpression)root))
				{
					var ex = Expression.Convert(BuildExpression(context, e), typeof(object));

					parms.Add(ex);

					return Expression.Convert(
						Expression.ArrayIndex(paramex, Expression.Constant(parms.Count - 1)),
						e.Type);
				}

				return e;
			});

			var sqtype = typeof(MultipleQueryHelper<>).MakeGenericType(expression.Type);
			var helper = (IMultipleQueryHelper)Activator.CreateInstance(sqtype);

			return helper.GetSubquery(this, expression, paramex, parms);
		}

		#endregion
	}
}
