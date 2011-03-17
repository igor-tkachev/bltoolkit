using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	partial class ExpressionBuilder
	{
		public Expression BuildExpression(IBuildContext context, Expression expression)
		{
			var newExpr = expression.Convert(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							if (IsSubQuery(context, pi))
								return BuildSubQuery(context, pi);

							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return BuildSql(context, pi);

							var ma = (MemberExpression)pi;

							if (ConvertMember(ma.Member) != null)
								break;

							var ctx = GetContext(context, pi);

							if (ctx != null)
								return ctx.BuildExpression(pi, 0);

							var ex = ma.Expression;

							/*
							if (query.Sources.Length > 0)
							{
								var field = query.GetBaseField(lambda, ma);

								if (field != null)
								{
									if (field is QueryField.Column)
										return BuildField(ma, field, converter);

									if (field is QuerySource.SubQuery)
										return BuildSubQuerySource(ma, (QuerySource.SubQuery)field, converter);

									if (field is QueryField.ExprColumn)
									{
										var col = (QueryField.ExprColumn)field;
										return BuildNewExpression(lambda, col.QuerySource, col.Expr, converter);
									}

									if (field is QuerySource.Table)
										return BuildTable(ma, (QuerySource.Table)field, null, converter);

									if (field is QueryField.SubQueryColumn)
										return BuildSubQuerySource(ma, (QueryField.SubQueryColumn)field, converter);

									if (field is QueryField.GroupByColumn)
										return BuildGroupBy(ma, (QueryField.GroupByColumn)field, converter);

									if (field is QuerySource.SubQuerySourceColumn)
										return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)field, converter);

									throw new InvalidOperationException();
								}

								//if (ex.Expr == expr.Expr && query is QuerySource.Scalar && ex.NodeType == ExpressionType.Constant)
								//	return BuildField(lambda, query, ma);
							}
							else
							{
								var field = GetField(lambda, ma, query);

								if (field != null)
									return BuildField(ma, field, converter/*i => i/);
							}
							*/

							if (ex != null && ex.NodeType == ExpressionType.Constant)
							{
								// field = localVariable
								//
								var c = _expressionAccessors[ex];
								return Expression.MakeMemberAccess(Expression.Convert(c, ex.Type), ma.Member);
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							if (pi == ParametersParam)
								break;

							var ctx = GetContext(context, pi);

							if (ctx != null)
								return ctx.BuildExpression(pi, 0);

							throw new NotImplementedException();

							/*
							if (query.Lambda                    != null &&
							    query.Lambda.MethodInfo         != null     &&
							    query.Lambda.MethodInfo.Name    == "Select" &&
							    query.Lambda.Parameters.Length  == 2        &&
							    query.Lambda.Parameters[1]      == pi)
							{
								return Expression.MakeMemberAccess(_contextParam, QueryCtx.Counter);
							}

							var field = query.GetBaseField(lambda, pi);

							if (field != null)
							{
								//Func<FieldIndex,FieldIndex> conv = i => converter(query.EnsureField(i.Field).Select(this)[0]);

								if (field is QuerySource.Table)
									return BuildTable(pi, (QuerySource.Table)field, null, converter);

								if (field is QuerySource.Scalar)
								{
									var source = (QuerySource)field;
									return BuildNewExpression(lambda, source, source.Lambda.Body, converter);
								}

								if (field is QuerySource.Expr)
								{
									var source = (QuerySource)field;
									return BuildQuerySourceExpr(query, source.Lambda.Body, converter);
								}

								if (field is QuerySource.GroupJoin)
									return BuildGroupJoin(pi, (QuerySource.GroupJoin)field, converter);

								if (field is QuerySource.SubQuery)
									return BuildSubQuerySource(pi, (QuerySource.SubQuery)field, converter);

								if (field is QuerySource.SubQuerySourceColumn)
									return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)field, converter);

								throw new InvalidOperationException();
							}

							//if (query.Lambda == null && query is QuerySource.SubQuerySourceColumn)
							//{
							//	return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)query, converter);
							//}

							break;
							*/
						}

					case ExpressionType.Constant:
						{
							if (ExpressionHelper.IsConstant(pi.Type))
								break;

							if (_expressionAccessors.ContainsKey(pi))
								return Expression.Convert(_expressionAccessors[pi], pi.Type);

							throw new NotImplementedException();

							/*
							if (query.Sources.Length == 0)
							{
								var field = GetField(lambda, pi, query);

								if (field != null)
								{
									var idx = field.Select(this);
									return BuildField(pi, field.GetExpressions(this)[0], idx.Select(i => converter(i).Index).ToArray());
								}
							}

							if (query is QuerySource.Scalar && CurrentSql.Select.Columns.Count == 0 && expr == pi)
								return BuildField(lambda, query.BaseQuery, pi, converter);

							if (query is QuerySource.SubQuerySourceColumn)
								return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)query, converter);

							return Expression.Convert(ExpressionAccessors[pi], pi.Type);
							*/
						}

					case ExpressionType.Coalesce:

						if (pi.Type == typeof(string) && MappingSchema.GetDefaultNullValue<string>() != null)
							return BuildSql(context, pi);

						if (CanBeTranslatedToSql(context, ConvertExpression(pi), true))
							return BuildSql(context, pi);

						break;

					case ExpressionType.Conditional:

						if (CanBeTranslatedToSql(context, ConvertExpression(pi), true))
							return BuildSql(context, pi);
						break;

					case ExpressionType.Call:
						{
							var ce = (MethodCallExpression)pi;
							var cm = ConvertMethod(ce);

							if (cm != null)
								if (ce.Method.GetCustomAttributes(typeof(MethodExpressionAttribute), true).Length != 0)
									return BuildExpression(context, cm);

							if (IsSubQuery(context, pi))
								return BuildSubQuery(context, pi);

							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return BuildSql(context, pi);
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
							return BuildSql(context, pi);
					}
				}

				return pi;
			});

			return newExpr;
		}

		bool EnforceServerSide(IBuildContext context)
		{
			return context.SqlQuery.Select.IsDistinct;
		}

		#region BuildSubQuery

		Expression BuildSubQuery(IBuildContext context, Expression expression)
		{
			if (expression.NodeType == ExpressionType.MemberAccess)
			{
				var ma = (MemberExpression)expression;

				if (ma.Expression != null)
				{
					switch (ma.Expression.NodeType)
					{
						case ExpressionType.Call         :
						case ExpressionType.MemberAccess :
						case ExpressionType.Parameter    :
							{
								var ctx = GetSubQuery(context, ma.Expression);
								var ex  = expression.Convert(e => e == ma.Expression ? Expression.Constant(null, ma.Expression.Type) : e);
								var sql = ctx.ConvertToSql(ex, 0, ConvertFlags.Field);

								if (sql.Length != 1)
									throw new NotImplementedException();

								//ctx.SqlQuery.Select.Columns.Clear();
								ctx.SqlQuery.Select.Add(sql[0].Sql);

								var idx = context.SqlQuery.Select.Add(ctx.SqlQuery);

								return BuildSql(expression.Type, idx);
							}
					}
				}
			}

			var sequence = GetSubQuery(context, expression);

			return sequence.BuildExpression(null, 0);


			throw new NotImplementedException();

			/*
			ParentQueries.Insert(0, new ParentQuery { Parent = query.BaseQuery, Parameter = query.Lambda.Parameters[0]});
			var sql = CurrentSql;

			CurrentSql = new SqlQuery { ParentSql = sql };

			var prev = _isSubQueryParsing;
			_isSubQueryParsing = true;

			var seq = BuildSequence(expr)[0];

			_isSubQueryParsing = prev;

			if (seq.Fields.Count == 1 && CurrentSql.Select.Columns.Count == 0)
				seq.Fields[0].Select(this);

			var column = new QueryField.ExprColumn(query, CurrentSql, null);

			query.Fields.Add(column);

			var idx    = column.Select(this);
			var result = BuildField(expr, column.GetExpressions(this)[0], idx.Select(i => converter(i).Index).ToArray());

			CurrentSql = sql;
			ParentQueries.RemoveAt(0);

			ParsingTracer.DecIndentLevel();

			return result;
			*/
		}

		#endregion

		#region BuildSql

		Expression BuildSql(IBuildContext context, Expression expression)
		{
			var sqlex = ConvertToSqlAndBuild(context, expression);
			var idx   = context.SqlQuery.Select.Add(sqlex);
			var field = BuildSql(expression.Type, idx);

			return field;
		}

		public Expression BuildSql(Type type, int idx)
		{
			Expression mapper;

			if (type.IsEnum)
			{
				mapper =
					Expression.Convert(
						Expression.Call(
							Expression.Constant(MappingSchema),
							ReflectionHelper.MapSchema.MapValueToEnum,
								Expression.Call(DataReaderParam, ReflectionHelper.DataReader.GetValue, Expression.Constant(idx)),
								Expression.Constant(type)),
						type);
			}
			else
			{
				MethodInfo mi;

				if (!ReflectionHelper.MapSchema.Converters.TryGetValue(type, out mi))
					throw new LinqException("Cannot find converter for the '{0}' type.", type.FullName);

				mapper = Expression.Call(
					Expression.Constant(MappingSchema),
					mi,
					Expression.Call(DataReaderParam, ReflectionHelper.DataReader.GetValue, Expression.Constant(idx)));
			}

			return mapper;
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
						var l  = ConvertMember(pi.Member);

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
						var l  = ConvertMember(e.Method);

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
