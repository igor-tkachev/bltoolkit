using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq
{
	using IndexConverter = Func<FieldIndex,FieldIndex>;

	using Common;
	using Data.Sql;
	using Mapping;
	using Reflection;

	partial class ExpressionParser<T>
	{
		#region SetQuery

		void SetQuery(Expression expr, QuerySource query, IndexConverter converter)
		{
			var table = query as QuerySource.Table;

			if (table != null)
				CurrentSql.Select.Columns.Clear();

			var idx = query.Select(this);

			if (table == null || table.InheritanceMapping.Count == 0)
			{
				foreach (var i in idx)
					converter(i);

				_info.SetQuery(null);
			}
			else
			{
				SetQuery(BuildTable(expr, table, null, converter));
			}
		}

		void SetQuery(Expression expr)
		{
			if (expr == null)
			{
				_info.SetQuery(null);
			}
			else
			{
				var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<T>>(
					expr, new[] { _infoParam, _contextParam, _dataReaderParam, _mapSchemaParam, ExpressionParam, ParametersParam });

				_info.SetQuery(mapper.Compile());
			}
		}

		#endregion

		#region Build Select

		#region BuildSelect

		Expression BuildSelect(QuerySource query, Expression ex, IndexConverter converter)
		{
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			Expression result = null;

			query.Match
			(
				table  => result = BuildTable   (table, ex ?? (table.Lambda != null ? table.Lambda.Body : null), null, converter), // QueryInfo.Table
				expr   => result = BuildNew     (query,       expr.  Lambda, expr.  Lambda.Body, converter), // QueryInfo.Expr
				sub    => result = BuildSubQuery(sub,   ex,                                      converter), // QueryInfo.SubQuery
				scalar => result = BuildNew     (query,       scalar.Lambda, scalar.Lambda.Body, converter), // QueryInfo.Scalar
				group  => result = BuildGroupBy (group,       group. Lambda, group. Lambda.Body),            // QueryInfo.GroupBy
				column => result = BuildNew     (query, column.Lambda, column.Lambda != null ? column.Lambda.Body : ex, converter) // QueryInfo.SubQuerySourceColumn
			);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		Expression BuildNew(QuerySource query, LambdaInfo lambda, Expression expr, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var addParents = lambda != null && lambda.Parameters.Length > 1 && query.Sources.Length >= lambda.Parameters.Length;

			if (addParents)
				for (var i = 0; i < lambda.Parameters.Length; i++)
					ParentQueries.Insert(i, new ParentQuery { Parent = query.Sources[i], Parameter = lambda.Parameters[i] });

			var result = BuildNewExpression(lambda, query, expr, converter);

			if (addParents)
				ParentQueries.RemoveRange(0, lambda.Parameters.Length);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region BuildSubQuerySource

		Expression BuildSubQuery(QuerySource.SubQuery subQuery, Expression ex, IndexConverter converter)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			Expression result = null;

			subQuery.BaseQuery.Match
			(
				table  => result = BuildTable(                      // QueryInfo.Table
					table,
					ex,
					subQuery.CheckNullField,
					i => i.Field == subQuery.CheckNullField ? converter(i) : converter(subQuery.EnsureField(i.Field).Select(this)[0])),
				expr   =>                                           // QueryInfo.Expr
				{
					if (expr.Lambda.Body.NodeType == ExpressionType.New)
						result = BuildQuerySourceExpr(subQuery, expr.Lambda.Body, converter);
					else
						throw new NotImplementedException();
				}, 
				sub    =>                                           // QueryInfo.SubQuery
					{
						result = BuildSubQuery(sub, ex, i => converter(subQuery.EnsureField(i.Field).Select(this)[0]));
					},
				scalar =>                                           // QueryInfo.Scalar
				{
					var idx = subQuery.Fields[0].Select(this);
					result = BuildField(scalar.Lambda.Body, idx.Select(i => converter(i).Index).ToArray());
				},
				_      => { throw new NotImplementedException(); }, // QueryInfo.GroupBy
				_      => { throw new NotImplementedException(); }  // QueryInfo.SubQuerySourceColumn
			);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		Expression BuildQuerySourceExpr(QuerySource query, Expression expr, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			NewExpression newExpr = null;
			var           member  = 0;

			var result = expr.Convert(ex =>
			{
				if (newExpr == null && ex.NodeType == ExpressionType.New)
				{
					newExpr = (NewExpression)ex;
				}
				else if (newExpr != null)
				{
					var mi = newExpr.Members[member++];

					if (mi is MethodInfo)
						mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

					var field = query.GetField(mi);

					if (field is QuerySource.SubQuerySourceColumn)
						return BuildSubQuerySourceColumn(ex, (QuerySource.SubQuerySourceColumn)field, converter);

					var idx   = field.Select(this);

					return BuildField(ex, idx.Select(i => converter(i).Index).ToArray());
				}

				return ex;
			});

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region BuildGroupBy

		interface IGroupByHelper
		{
			Expression GetParseInfo(ExpressionParser<T> parser, QuerySource.GroupBy query, Expression expr, Expression info);
		}

		class GroupByHelper<TKey,TElement,TSource> : IGroupByHelper
		{
			public Expression GetParseInfo(ExpressionParser<T> parser, QuerySource.GroupBy query, Expression expr, Expression info)
			{
				var valueParser = new ExpressionParser<TElement>();
				var keyParam    = Expression.Convert(Expression.ArrayIndex(ParametersParam, Expression.Constant(0)), typeof(TKey));

				Expression valueExpr = null;

				if (expr.NodeType == ExpressionType.New)
				{
					var ne = (NewExpression)expr;

					for (var i = 0; i < ne.Arguments.Count; i++)
					{
						var member = TypeHelper.GetPropertyByMethod((MethodInfo)ne.Members[i]);
						var equal  = Expression.Equal(ne.Arguments[i], Expression.MakeMemberAccess(keyParam, member));

						valueExpr = valueExpr == null ? equal : Expression.AndAlso(valueExpr, equal);
					}
				}
				else if (query.BaseQuery is QuerySource.Table)
				{
					var table  = (QuerySource.Table)query.BaseQuery;
					var parent = table.ParentAssociation;
					var pexpr  = ((MemberExpression)expr).Expression;
					var conds = table.ParentAssociationJoin.Condition.Conditions;

					foreach (var cond in conds)
					{
						var ee = (SqlQuery.Predicate.ExprExpr)cond.Predicate;

						var equal  = Expression.Equal(
							Expression.MakeMemberAccess(pexpr,    parent.Columns[((SqlField)ee.Expr1).Name].Mapper.MemberAccessor.MemberInfo),
							Expression.MakeMemberAccess(keyParam, table. Columns[((SqlField)ee.Expr2).Name].Mapper.MemberAccessor.MemberInfo));

						valueExpr = valueExpr == null ? equal : Expression.AndAlso(valueExpr, equal);
					}
				}
				else
				{
					valueExpr = Expression.Equal(query.Lambda.Body, keyParam);
				}

// ReSharper disable AssignNullToNotNullAttribute
				valueExpr = Expression.Call(
					null,
					Expressor<object>.MethodExpressor(_ => Queryable.Where(null, (Expression<Func<TSource,bool>>)null)),
					query.OriginalQuery.Lambda.Body,
					Expression.Lambda<Func<TSource,bool>>(valueExpr, new[] { query.Lambda.Parameters[0] }));

				if (query.ElementSource != null)
				{
					valueExpr = Expression.Call(
						null,
						Expressor<object>.MethodExpressor(_ => Queryable.Select(null, (Expression<Func<TSource,TElement>>)null)),
						valueExpr,
						Expression.Lambda<Func<TSource,TElement>>(query.ElementSource.Lambda.Body, new[] { query.ElementSource.Lambda.Parameters[0] }));
				}
// ReSharper restore AssignNullToNotNullAttribute

				var keyReader = Expression.Lambda<ExpressionInfo<T>.Mapper<TKey>>(
					info, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser.ExpressionParam,
						ParametersParam
					});

				return
					Expression.Call(parser._infoParam, parser._info.GetGroupingMethodInfo<TKey,TElement>(),
						parser._contextParam,
						parser._dataReaderParam,
						parser.ExpressionParam,
						ParametersParam,
						Expression.Constant(keyReader.Compile()),
						Expression.Constant(valueParser.Parse(parser._info.DataProvider, parser._info.MappingSchema, valueExpr, parser._info.Parameters)));
			}
		}

		Expression BuildGroupBy(QuerySource.GroupBy query, LambdaInfo lambda, Expression expr)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			Expression result;

			if (query.BaseQuery is QuerySource.Table)
			{
				var table = (QuerySource.Table)query.BaseQuery;
				var conds = table.ParentAssociationJoin.Condition.Conditions;
				var index = new int[table.Fields.Count];

				for (var i = 0; i < index.Length; i++)
					index[i] = -1;

				foreach (var cond in conds)
				{
					var field = (SqlField)((SqlQuery.Predicate.ExprExpr)cond.Predicate).Expr2;

					index[table.Fields.IndexOf(table.Columns[field.Name])] = query.GetField(field).Select(this)[0].Index;
				}

				result =
					Expression.Convert(
						Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
							Expression.Constant(expr.Type),
							_dataReaderParam,
							Expression.Constant(_info.GetMapperSlot(index))),
						expr.Type);
			}
			else if (query.IsWrapped && expr.NodeType != ExpressionType.New)
			{
				var idx = query.Fields[0].Select(this);

				if (idx.Length != 1)
					throw new InvalidOperationException();

				result = BuildField(expr, new[] { idx[0].Index });
			}
			else
				result = BuildNewExpression(lambda, query, expr, i => i);

			var args   = query.GroupingType.GetGenericArguments();
			var helper = (IGroupByHelper)Activator.CreateInstance(typeof(GroupByHelper<,,>).
				MakeGenericType(typeof(T), args[0], args[1], query.Lambda.Parameters[0].Type));

			result = helper.GetParseInfo(this, query, expr, result);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region BuildNewExpression

		Expression BuildNewExpression(LambdaInfo lambda, QuerySource query, Expression expr, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var newExpr = expr.Convert(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							if (IsSubQuery(pi, query))
								return BuildSubQuery(pi, query, converter);

							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return BuildField(lambda, query.BaseQuery, pi);

							var ma = (MemberExpression)pi;
							var ex = ma.Expression;

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
									{
										return BuildGroupBy(ma, (QueryField.GroupByColumn)field, converter);
									}

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
									return BuildField(ma, field, converter/*i => i*/);
							}

							if (ex != null && ex.NodeType == ExpressionType.Constant)
							{
								// field = localVariable
								//
								var c = ExpressionAccessors[ex];
								return Expression.MakeMemberAccess(Expression.Convert(c, ex.Type), ma.Member);
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							if (query.Lambda.MethodInfo         != null     &&
							    query.Lambda.MethodInfo.Name    == "Select" &&
							    query.Lambda.Parameters.Length  == 2        &&
							    query.Lambda.Parameters[1]      == pi)
							{
								return Expression.MakeMemberAccess(_contextParam, QueryCtx.Counter);
							}

							if (pi == ParametersParam)
								return pi;

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

							break;
						}

					case ExpressionType.Constant:
						{
							if (ExpressionHelper.IsConstant(pi.Type))
								break;

							if (query.Sources.Length == 0)
							{
								var field = GetField(lambda, pi, query);

								if (field != null)
								{
									var idx = field.Select(this);
									return BuildField(pi, idx.Select(i => converter(i).Index).ToArray());
								}
							}

							if (query is QuerySource.Scalar && CurrentSql.Select.Columns.Count == 0 && expr == pi)
								return BuildField(lambda, query.BaseQuery, pi);

							if (query is QuerySource.SubQuerySourceColumn)
								return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)query, converter);

							break;
						}

					case ExpressionType.Coalesce:
						if (pi.Type == typeof(string) && _info.MappingSchema.GetDefaultNullValue<string>() != null)
							return BuildField(lambda, query.BaseQuery, pi);
						goto case ExpressionType.Conditional;

					case ExpressionType.Conditional:
						if (CanBeTranslatedToSql(lambda, pi, true, query.BaseQuery))
							return BuildField(lambda, query.BaseQuery, pi);
						break;

					case ExpressionType.Call:
						{
							var ce = (MethodCallExpression)pi;
							var cm = ConvertMethod(ce);

							if (cm != null)
								if (ce.Method.GetCustomAttributes(typeof (MethodExpressionAttribute), true).Length != 0)
									return BuildNewExpression(lambda, query, cm, converter);

							if (IsSubQuery(pi, query))
								return BuildSubQuery(pi, query, converter);

							if (IsServerSideOnly(pi) || PreferServerSide(pi))
								return BuildField(lambda, query.BaseQuery, pi);
						}

						break;
				}

				if (EnforceServerSide())
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
							return BuildField(lambda, query.BaseQuery, pi);
					}
				}

				return pi;
			});

			ParsingTracer.DecIndentLevel();
			return newExpr;
		}

		bool EnforceServerSide()
		{
			return CurrentSql.Select.IsDistinct;
		}

		#endregion

		#region BuildSubQuery

		Expression BuildSubQuery(Expression expr, QuerySource query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			ParentQueries.Insert(0, new ParentQuery { Parent = query.BaseQuery, Parameter = query.Lambda.Parameters[0]});
			var sql = CurrentSql;

			CurrentSql = new SqlQuery { ParentSql = sql };

			var prev = _isSubQueryParsing;
			_isSubQueryParsing = true;

			var seq = ParseSequence(expr)[0];

			_isSubQueryParsing = prev;

			if (seq.Fields.Count == 1 && CurrentSql.Select.Columns.Count == 0)
				seq.Fields[0].Select(this);

			var column = new QueryField.ExprColumn(query, CurrentSql, null);

			query.Fields.Add(column);

			var idx    = column.Select(this);
			var result = BuildField(expr, idx.Select(i => converter(i).Index).ToArray());

			CurrentSql = sql;
			ParentQueries.RemoveAt(0);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region BuildTable

		static object DefaultInheritanceMappingException(object value, Type type)
		{
			throw new LinqException("Inheritance mapping is not defined for discriminator value '{0}' in the '{1}' hierarchy.", value, type);
		}

		Expression BuildTable(QuerySource.Table table, Expression expr, QueryField checkNullField, IndexConverter converter)
		{
			if (expr == null /*table.InheritanceMapping.Count == 0*/)
			{
				CurrentSql.Select.Columns.Clear();

				var idx = table.Select(this);

				foreach (var i in idx)
					converter(i);

				return null;
			}

			return BuildTable(expr, table, checkNullField, converter);
		}

		Expression BuildTable(Expression pi, QuerySource.Table table, QueryField checkNullField, IndexConverter converter)
		{
			ParsingTracer.WriteLine(pi);
			ParsingTracer.WriteLine(table);
			ParsingTracer.IncIndentLevel();

			var objectType = table.ObjectType;

			if (table.InheritanceMapping.Count > 0 && pi.Type.IsGenericType)
			{
				var types = pi.Type.GetGenericArguments();

				if (types.Length == 1 && TypeHelper.IsSameOrParent(objectType, types[0]))
					objectType = types[0];
			}

			var mapperMethod = _info.GetMapperMethodInfo();

			Func<Type,int[],UnaryExpression> makeExpr = (ty,idx) =>
				Expression.Convert(
					Expression.Call(_infoParam, mapperMethod,
						Expression.Constant(ty),
						_dataReaderParam,
						Expression.Constant(_info.GetMapperSlot(idx))),
					objectType);

			Func<Type,int[]> makeIndex = ty =>
			{
				var q =
					from mm in _info.MappingSchema.GetObjectMapper(ty)
					where !mm.MapMemberInfo.SqlIgnore
					select converter(table.Columns[mm.MemberName].Select(this)[0]).Index;

				return q.ToArray();
			};

			Expression expr;

			if (objectType != table.ObjectType)
			{
				expr = makeExpr(objectType, makeIndex(objectType));
			}
			else if (table.InheritanceMapping.Count == 0)
			{
				expr = makeExpr(objectType, table.Select(this).Select(i => converter(i).Index).ToArray());

				if (table.ParentAssociation != null && table.ParentAssociationJoin.JoinType == SqlQuery.JoinType.Left)
				{
					Expression cond = null;

					var checkNullOnly = CurrentSql.Select.IsDistinct || CurrentSql.GroupBy.Items.Count > 0;

					if (checkNullOnly)
					{
						checkNullOnly = false;

						foreach (var c in table.ParentAssociationJoin.Condition.Conditions)
						{
							var ee = (SqlQuery.Predicate.ExprExpr)c.Predicate;

							var field1  = (SqlField)ee.Expr1;
							var column1 = (QueryField.Column)table.ParentAssociation.GetField(field1);

							checkNullOnly = CurrentSql.Select.Columns.Find(col => col.Expression == column1.Field) == null;

							if (checkNullOnly)
								break;
						}
					}

					foreach (var c in table.ParentAssociationJoin.Condition.Conditions)
					{
						var ee = (SqlQuery.Predicate.ExprExpr)c.Predicate;

						var field2  = (SqlField)ee.Expr2;
						var column2 = (QueryField.Column)table.ParentAssociation.GetField(field2);
						var index2  = column2.Select(this)[0].Index;

						Expression e;

						if (checkNullOnly)
						{
							e = Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(index2));
						}
						else
						{
							var field1  = (SqlField)ee.Expr1;
							var column1 = (QueryField.Column)table.ParentAssociation.GetField(field1);
							var index1  = column1.Select(this)[0].Index;

							e =
								Expression.AndAlso(
									Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(index2)),
									Expression.Not(
										Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(index1))));
						}

						cond = cond == null ? e : Expression.AndAlso(cond, e);
					}

					expr = Expression.Condition(cond, Expression.Constant(null, objectType), expr);
				}
			}
			else
			{
				var defaultMapping = table.InheritanceMapping.SingleOrDefault(m => m.IsDefault);

				if (defaultMapping != null)
				{
					expr = makeExpr(defaultMapping.Type, makeIndex(defaultMapping.Type));
				}
				else
				{
					var exceptionMethod = Expressor<ExpressionParser<T>>.MethodExpressor(_ => DefaultInheritanceMappingException(null, null));
					var dindex          = table.Columns[table.InheritanceDiscriminators[0]].Select(this)[0].Index;

					expr = Expression.Convert(
						Expression.Call(null, exceptionMethod,
							Expression.Call(_dataReaderParam, DataReader.GetValue, Expression.Constant(dindex)),
							Expression.Constant(table.ObjectType)),
						table.ObjectType);
				}

				foreach (var mapping in table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => m.m != defaultMapping))
				{
					var dindex = table.Columns[table.InheritanceDiscriminators[mapping.i]].Select(this)[0].Index;
					Expression testExpr;

					if (mapping.m.Code == null)
					{
						testExpr = Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(dindex));
					}
					else
					{
						MethodInfo mi;
						var codeType = mapping.m.Code.GetType();

						if (!MapSchema.Converters.TryGetValue(codeType, out mi))
							throw new LinqException("Cannot find converter for the '{0}' type.", codeType.FullName);

						testExpr =
							Expression.Equal(
								Expression.Constant(mapping.m.Code),
								Expression.Call(_mapSchemaParam, mi, Expression.Call(_dataReaderParam, DataReader.GetValue, Expression.Constant(dindex))));
					}

					expr = Expression.Condition(testExpr, makeExpr(mapping.m.Type, makeIndex(mapping.m.Type)), expr);
				}
			}

			if (checkNullField != null)
			{
				var idx = converter(checkNullField.Select(this)[0]).Index;

				expr = Expression.Condition(
					Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(idx)),
					Expression.Constant(null, objectType),
					expr);
			}

			ParsingTracer.DecIndentLevel();
			return expr;
		}

		#endregion

		#region BuildSubQuerySource

		Expression BuildSubQuerySource(Expression ma, QuerySource.SubQuery query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			Expression result = null;

			if (query is QuerySource.GroupJoin && TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Type))
			{
				result = BuildGroupJoin(ma, (QuerySource.GroupJoin)query, converter);
			}
			else if (query.Sources.Length == 1)
			{
				var baseQuery = query.BaseQuery;

				Func<FieldIndex,FieldIndex> conv = i => converter(query.EnsureField(i.Field).Select(this)[0]);

				if (baseQuery is QuerySource.Table)
				{
					result = BuildTable(ma, (QuerySource.Table)baseQuery, query.CheckNullField, conv);
				}
				else if (baseQuery is QuerySource.SubQuery)
				{
					result = BuildSubQuerySource(ma, (QuerySource.SubQuery)baseQuery, conv);
				}
				else if (baseQuery is QuerySource.Scalar)
				{
					var idx = baseQuery.Fields[0].Select(this);
					result = BuildField(ma, idx.Select(i => conv(i).Index).ToArray());
				}
				else
					result = BuildNewExpression(baseQuery.Lambda, baseQuery, baseQuery.Lambda.Body, conv);

				if (query is QuerySource.GroupJoin)
				{
					var join  = (QuerySource.GroupJoin)query;
					var check = join.CheckNullField;
					var idx   = converter(check.Select(this)[0]);

					result = 
						Expression.Condition(
							Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(idx.Index)),
							Expression.Convert(
								Expression.Constant(_info.MappingSchema.GetNullValue(result.Type)),
								result.Type),
							result);
				}
			}

			if (result == null)
				throw new InvalidOperationException();

			ParsingTracer.DecIndentLevel();

			return result;
		}

		Expression BuildSubQuerySource(Expression ma, QueryField.SubQueryColumn query, IndexConverter converter)
		{
#if DEBUG
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			try
			{
#endif
				IndexConverter conv = i => converter(query.QuerySource.EnsureField(i.Field).Select(this)[0]);

				if (query.Field is QuerySource.Table)
					return BuildTable(ma, (QuerySource.Table)query.Field, null, conv);

				if (query.Field is QuerySource.SubQuery)
					return BuildSubQuerySource(ma, (QuerySource.SubQuery)query.Field, conv);

				if (query.Field is QuerySource)
					throw new InvalidOperationException();

				if (query.Field is QueryField.SubQueryColumn)
					return BuildSubQuerySource(ma, (QueryField.SubQueryColumn)query.Field, conv);

				return BuildField(ma, query, converter);
#if DEBUG
			}
			finally
			{
				ParsingTracer.DecIndentLevel();
			}
#endif
		}

		#endregion

		#region BuildSubQuerySourceColumn

		Expression BuildSubQuerySourceColumn(Expression info, QuerySource.SubQuerySourceColumn source, IndexConverter converter)
		{
			var pi = BuildSelect(
				source.SourceColumn,
				info,
				i => converter(source.QuerySource.EnsureField(i.Field).Select(this)[0]));

			return pi;
		}

		#endregion

		#region BuildGroupJoin

		interface IGroupJoinHelper
		{
			Expression GetParseInfo(ExpressionParser<T> parser, Expression ma, FieldIndex counterIndex, Expression info);
		}

		class GroupJoinHelper<TE> : IGroupJoinHelper
		{
			public Expression GetParseInfo(ExpressionParser<T> parser, Expression ma, FieldIndex counterIndex, Expression info)
			{
				var itemReader = Expression.Lambda<ExpressionInfo<T>.Mapper<TE>>(
					info, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser.ExpressionParam,
						ParametersParam
					});

				return
					Expression.Call(parser._infoParam, parser._info.GetGroupJoinEnumeratorMethodInfo<TE>(),
						parser._contextParam,
						parser._dataReaderParam,
						parser.ExpressionParam,
						ParametersParam,
						Expression.Constant(counterIndex.Index),
						Expression.Constant(itemReader.Compile()));
			}
		}

		Expression BuildGroupJoin(Expression ma, QuerySource.GroupJoin query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var args = ma.Type.GetGenericArguments();

			if (args.Length == 0)
				return BuildSubQuerySource(ma, query, converter);

			var expr         = BuildSelect(query.BaseQuery, ma, i => converter(query.EnsureField(i.Field).Select(this)[0]));
			var helper       = (IGroupJoinHelper)Activator.CreateInstance(typeof(GroupJoinHelper<>).MakeGenericType(typeof(T), args[0]));
			var counterIndex = converter(query.Counter.Select(this)[0]);
			var result       = helper.GetParseInfo(this, ma, counterIndex, expr);

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region BuildGroupBy

		Expression BuildGroupBy(MemberExpression ma, QueryField.GroupByColumn field, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(field);
			ParsingTracer.IncIndentLevel();

			var source = field.GroupBySource.BaseQuery;

			if (source is QuerySource.Scalar)
				return BuildField(ma, field.GroupBySource, converter);

			if (source is QuerySource.SubQuery)
			{
				if (source.BaseQuery is QuerySource.Scalar)
					return BuildField(ma, field.GroupBySource, converter);

				return BuildNewExpression(field.GroupBySource.Lambda, source, field.GroupBySource.Lambda.Body, converter /*i => converter(source.EnsureField(i.Field).Select(this)[0])*/);
			}

			var result = BuildNewExpression(field.GroupBySource.Lambda, source, field.GroupBySource.Lambda.Body, converter);
			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region BuildField

		Expression BuildField(LambdaInfo lambda, QuerySource query, Expression expr)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var sqlex = ParseExpression(lambda, expr, query);
			var idx   = CurrentSql.Select.Add(sqlex);
			var field = BuildField(expr, new[] { idx });

			ParsingTracer.IncIndentLevel();
			return field;
		}

		Expression BuildField(Expression ma, QueryField field, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(field);
			ParsingTracer.IncIndentLevel();

			Expression result = null;

			if (field is QuerySource.SubQuery)
			{
				var query = (QuerySource.SubQuery)field;

				if (query is QuerySource.GroupJoin && TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Type))
					result = BuildGroupJoin(ma, (QuerySource.GroupJoin)query, converter);
				else if (query.BaseQuery is QuerySource.Table)
				{
					var table = (QuerySource.Table)query.BaseQuery;

					if (ma.Type == table.ObjectType)
						result = BuildTable(ma, table, query.CheckNullField, i => converter(query.EnsureField(i.Field).Select(this)[0]));
				}
			}

			if (result == null)
			{
				var idx = field.Select(this);
				result = BuildField(ma, idx.Select(i => converter(i).Index).ToArray());
			}

			ParsingTracer.DecIndentLevel();
			return result;
		}

		Expression BuildField(Expression ma, int[] idx)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.IncIndentLevel();

			if (idx.Length != 1)
				throw new InvalidOperationException();

			var type = ma.Type;

			Expression mapper;

			if (type.IsEnum)
			{
				mapper =
					Expression.Convert(
						Expression.Call(
							_mapSchemaParam,
							MapSchema.MapValueToEnum,
								Expression.Call(_dataReaderParam, DataReader.GetValue, Expression.Constant(idx[0])),
								Expression.Constant(type)),
						type);
			}
			else
			{
				MethodInfo mi;

				if (!MapSchema.Converters.TryGetValue(type, out mi))
					throw new LinqException("Cannot find converter for the '{0}' type.", type.FullName);

				mapper = Expression.Call(_mapSchemaParam, mi, Expression.Call(_dataReaderParam, DataReader.GetValue, Expression.Constant(idx[0])));
			}

			ParsingTracer.DecIndentLevel();
			return mapper;
		}

		#endregion

		#endregion

		#region BuildSimpleQuery

		bool BuildSimpleQuery(Expression expr, Type type, string alias)
		{
			var table = CreateTable(CurrentSql, type);

			table.SqlTable.Alias = alias;

			SetQuery(expr, table, i => i);

			return true;
		}

		#endregion

		#region Build Scalar Select

		void BuildScalarSelect(Expression ex)
		{
			switch (ex.NodeType)
			{
				case ExpressionType.New:
				case ExpressionType.MemberInit:
					var query = ParseSelect(new LambdaInfo(ex))[0];

					query.Select(this);
					SetQuery(BuildNew(query, null, ex, i => i));

					return;
			}

			var expr = ParseExpression(ex);

			CurrentSql.Select.Expr(expr);

			var pi = BuildField(ex, new[] { 0 });

			var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<T>>(
				pi, new [] { _infoParam, _contextParam, _dataReaderParam, _mapSchemaParam, ExpressionParam, ParametersParam });

			_info.SetQuery(mapper.Compile());
		}

		#endregion

		#region Build Constant

		readonly Dictionary<Expression,SqlValue> _constants = new Dictionary<Expression,SqlValue>();

		SqlValue BuildConstant(Expression expr)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr,typeof(object)));

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

		readonly Dictionary<Expression,ExpressionInfo<T>.Parameter> _parameters   = new Dictionary<Expression, ExpressionInfo<T>.Parameter>();
		readonly HashSet<Expression>                                _asParameters = new HashSet<Expression>();

		ExpressionInfo<T>.Parameter BuildParameter(Expression expr)
		{
			ExpressionInfo<T>.Parameter p;

			if (_parameters.TryGetValue(expr, out p))
				return p;

			string name = null;

			var newExpr = ReplaceParameter(ExpressionAccessors, expr, nm => name = nm);
			var mapper  = Expression.Lambda<Func<ExpressionInfo<T>,Expression,object[],object>>(
				Expression.Convert(newExpr, typeof(object)),
				new [] { _infoParam, ExpressionParam, ParametersParam });

			p = new ExpressionInfo<T>.Parameter
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, name, null)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p;
		}

		Expression ReplaceParameter(IDictionary<Expression,Expression> expressionAccessors, Expression expr, Action<string> setName)
		{
			return expr.Convert(pi =>
			{
				pi.IsConstant(c =>
				{
					//if (!TypeHelper.IsScalar(pi.Type) || _asParameters.Contains(c))
					if (!ExpressionHelper.IsConstant(pi.Type) || _asParameters.Contains(c))
					{
						var val = expressionAccessors[pi];

						pi = Expression.Convert(val, pi.Type);

						if (expr.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)expr;
							setName(ma.Member.Name);
						}
					}
				});

				return pi;
			});
		}

		#endregion

		#region Expression Parser

		#region ParseExpression

		public ISqlExpression ParseExpression(Expression expr, params QuerySource[] queries)
		{
			return ParseExpression(null, expr, queries);
		}

		public ISqlExpression ParseExpression(LambdaInfo lambda, Expression expression, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(expression);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			try
			{
				var qlen = queries.Length;

				if (expression.NodeType == ExpressionType.Parameter && qlen == 1 && queries[0] is QuerySource.Scalar)
				{
					var ma = (QuerySource.Scalar)queries[0];
					return ParseExpression(ma.Lambda, ma.Lambda.Body, ma.Sources);
				}

				if (CanBeConstant(expression))
					return BuildConstant(expression);

				if (CanBeCompiled(expression))
					return BuildParameter(expression).SqlParameter;

				if (IsSubQuery(expression, queries))
					return ParseSubQuery(lambda, expression, queries);

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
							ParseSearchCondition(condition.Conditions, null, expression, queries);
							return condition;
						}

					case ExpressionType.Add:
					case ExpressionType.AddChecked:
					case ExpressionType.And:
					case ExpressionType.Divide:
					case ExpressionType.ExclusiveOr:
					case ExpressionType.Modulo:
					case ExpressionType.Multiply:
					case ExpressionType.Or:
					case ExpressionType.Power:
					case ExpressionType.Subtract:
					case ExpressionType.SubtractChecked:
					case ExpressionType.Coalesce:
						{
							var e = expression as BinaryExpression;
							var l = ParseExpression(lambda, e.Left,  queries);
							var r = ParseExpression(lambda, e.Right, queries);
							var t = e.Type;

							switch (expression.NodeType)
							{
								case ExpressionType.Add            :
								case ExpressionType.AddChecked     : return Convert(new SqlBinaryExpression(t, l, "+", r, Precedence.Additive));
								case ExpressionType.And            : return Convert(new SqlBinaryExpression(t, l, "&", r, Precedence.Bitwise));
								case ExpressionType.Divide         : return Convert(new SqlBinaryExpression(t, l, "/", r, Precedence.Multiplicative));
								case ExpressionType.ExclusiveOr    : return Convert(new SqlBinaryExpression(t, l, "^", r, Precedence.Bitwise));
								case ExpressionType.Modulo         : return Convert(new SqlBinaryExpression(t, l, "%", r, Precedence.Multiplicative));
								case ExpressionType.Multiply       : return Convert(new SqlBinaryExpression(t, l, "*", r, Precedence.Multiplicative));
								case ExpressionType.Or             : return Convert(new SqlBinaryExpression(t, l, "|", r, Precedence.Bitwise));
								case ExpressionType.Power          : return Convert(new SqlFunction(t, "Power", l, r));
								case ExpressionType.Subtract       :
								case ExpressionType.SubtractChecked: return Convert(new SqlBinaryExpression(t, l, "-", r, Precedence.Subtraction));
								case ExpressionType.Coalesce       :
									{
										if (r is SqlFunction)
										{
											var c = (SqlFunction)r;

											if (c.Name == "Coalesce")
											{
												var parms = new ISqlExpression[c.Parameters.Length + 1];

												parms[0] = l;
												c.Parameters.CopyTo(parms, 1);

												return Convert(new SqlFunction(t, "Coalesce", parms));
											}
										}

										return Convert(new SqlFunction(t, "Coalesce", l, r));
									}
							}

							break;
						}

					case ExpressionType.UnaryPlus:
					case ExpressionType.Negate:
					case ExpressionType.NegateChecked:
						{
							var e = expression as UnaryExpression;
							var o = ParseExpression(lambda, e.Operand, queries);
							var t = e.Type;

							switch (expression.NodeType)
							{
								case ExpressionType.UnaryPlus     : return o;
								case ExpressionType.Negate        :
								case ExpressionType.NegateChecked : return Convert(new SqlBinaryExpression(t, new SqlValue(-1), "*", o, Precedence.Multiplicative));
							}

							break;
						}

					case ExpressionType.Convert:
						{
							var e = expression as UnaryExpression;
							var o = ParseExpression(lambda, e.Operand, queries);

							if (e.Method == null && e.IsLifted)
								return o;

							if (e.Operand.Type.IsEnum && Enum.GetUnderlyingType(e.Operand.Type) == e.Type)
								return o;

							return Convert(new SqlFunction(e.Type, "$Convert$", SqlDataType.GetDataType(e.Type), SqlDataType.GetDataType(e.Operand.Type), o));
						}

					case ExpressionType.Conditional:
						{
							var e = expression as ConditionalExpression;
							var s = ParseExpression(lambda, e.Test,    queries);
							var t = ParseExpression(lambda, e.IfTrue,  queries);
							var f = ParseExpression(lambda, e.IfFalse, queries);

							if (f is SqlFunction)
							{
								var c = (SqlFunction)f;

								if (c.Name == "CASE")
								{
									var parms = new ISqlExpression[c.Parameters.Length + 2];

									parms[0] = s;
									parms[1] = t;
									c.Parameters.CopyTo(parms, 2);

									return Convert(new SqlFunction(e.Type, "CASE", parms));
								}
							}

							return Convert(new SqlFunction(e.Type, "CASE", s, t, f));
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)expression;
							var l  = ConvertMember(ma.Member);

							if (l != null)
							{
								var ef  = l.Body.UnwrapLambda();
								var pie = ef.Convert(wpi => wpi.NodeType == ExpressionType.Parameter ? ma.Expression : wpi);

								return ParseExpression(lambda, pie, queries);
							}

							var attr = GetFunctionAttribute(ma.Member);

							if (attr != null)
								return Convert(attr.GetExpression(ma.Member));

							if (IsNullableValueMember(ma.Member))
								return ParseExpression(lambda, ma.Expression, queries);

							if (IsListCountMember(ma.Member))
							{
								var src = GetSource(null, ma.Expression, queries);
								if (src != null)
									return SqlFunction.CreateCount(expression.Type, src.SqlQuery);
							}

							goto case ExpressionType.Parameter;
						}

					case ExpressionType.Parameter:
						{
							var field = GetField(lambda, expression, queries);

							if (field != null)
							{
								var exprs = field.GetExpressions(this);

								if (exprs == null)
									break;

								if (exprs.Length == 1)
									return exprs[0];

								throw new InvalidOperationException();
							}

							break;
						}

					case ExpressionType.Call:
						{
							var e = expression as MethodCallExpression;

							if (e.Method.DeclaringType == typeof(Enumerable))
								return ParseEnumerable(lambda, e, queries);

							var cm = ConvertMethod(e);
							if (cm != null)
								return ParseExpression(lambda, cm, queries);

							var attr = GetFunctionAttribute(e.Method);

							if (attr != null)
							{
								var parms = new List<ISqlExpression>();

								if (e.Object != null)
									parms.Add(ParseExpression(lambda, e.Object, queries));

								for (var i = 0; i < e.Arguments.Count; i++)
									parms.Add(ParseExpression(lambda, e.Arguments[i], queries));

								return Convert(attr.GetExpression(e.Method, parms.ToArray()));
							}

							break;
						}

					case ExpressionType.New:
						{
							var pie = ConvertNew((NewExpression)expression);

							if (pie != null)
								return ParseExpression(lambda, pie, queries);

							break;
						}

					case ExpressionType.Invoke:
						{
							var pi = (InvocationExpression)expression;
							var ex = pi.Expression;

							if (ex.NodeType == ExpressionType.Quote)
								ex = ((UnaryExpression)ex).Operand;

							//if (ex.NodeType == ExpressionType.MemberAccess)
							//	return ParseExpression(lambda, ex, queries);

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

								return ParseExpression(lambda, pie, queries);
							}

							break;
						}
				}

				throw new LinqException("'{0}' cannot be converted to SQL.", expression);
			}
			finally
			{
				ParsingTracer.DecIndentLevel();
			}
		}

		#endregion

		#region ParseEnumerable

		ISqlExpression ParseEnumerable(LambdaInfo lambda, MethodCallExpression expression, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(expression);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			QueryField field = queries.Length == 1 && queries[0] is QuerySource.GroupBy ? queries[0] : null;

			if (field == null)
				field = GetField(lambda, expression.Arguments[0], queries);

			while (field is QuerySource.SubQuerySourceColumn)
				field = ((QuerySource.SubQuerySourceColumn)field).SourceColumn;

			if (field is QuerySource.GroupJoin && expression.Method.Name == "Count")
			{
				var ex = ((QuerySource.GroupJoin)field).Counter.GetExpressions(this)[0];
				ParsingTracer.DecIndentLevel();
				return ex;
			}

			if (!(field is QuerySource.GroupBy))
				throw new LinqException("'{0}' cannot be converted to SQL.", expression);

			var groupBy = (QuerySource.GroupBy)field;
			var expr    = ParseEnumerable(expression, groupBy);

			if (queries.Length == 1 && queries[0] is QuerySource.SubQuery)
			{
				var subQuery  = (QuerySource.SubQuery)queries[0];
				var column    = groupBy.FindField(new QueryField.ExprColumn(groupBy, expr, null));
				var subColumn = subQuery.EnsureField(column);

				expr = subColumn.GetExpressions(this)[0];
			}

			ParsingTracer.DecIndentLevel();
			return expr;
		}

		ISqlExpression ParseEnumerable(MethodCallExpression expr, QuerySource.GroupBy query)
		{
			var groupBy = query.OriginalQuery;
			var args    = new ISqlExpression[expr.Arguments.Count - 1];

			if (expr.Method.Name == "Count")
			{
				if (args.Length > 0)
				{
					var predicate = ParsePredicate(null, ParseLambdaArgument(expr, 1), groupBy);

					groupBy.SqlQuery.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));

					var sql = groupBy.SqlQuery.Clone(o => !(o is SqlParameter));

					groupBy.SqlQuery.Where.SearchCondition.Conditions.RemoveAt(groupBy.SqlQuery.Where.SearchCondition.Conditions.Count - 1);

					sql.Select.Columns.Clear();

					if (_info.SqlProvider.IsSubQueryColumnSupported && _info.SqlProvider.IsCountSubQuerySupported)
					{
						for (var i = 0; i < sql.GroupBy.Items.Count; i++)
						{
							var item1 = sql.GroupBy.Items[i];
							var item2 = groupBy.SqlQuery.GroupBy.Items[i];
							var pr    = Convert(new SqlQuery.Predicate.ExprExpr(item1, SqlQuery.Predicate.Operator.Equal, item2));

							sql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, pr));
						}

						sql.GroupBy.Items.Clear();
						sql.Select.Expr(SqlFunction.CreateCount(expr.Type, sql));
						sql.ParentSql = groupBy.SqlQuery;

						return sql;
					}

					var join = sql.WeakLeftJoin();

					groupBy.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);

					for (var i = 0; i < sql.GroupBy.Items.Count; i++)
					{
						var item1 = sql.GroupBy.Items[i];
						var item2 = groupBy.SqlQuery.GroupBy.Items[i];
						var col   = sql.Select.Columns[sql.Select.Add(item1)];
						var pr    = Convert(new SqlQuery.Predicate.ExprExpr(col, SqlQuery.Predicate.Operator.Equal, item2));

						join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, pr));
					}

					sql.ParentSql = groupBy.SqlQuery;

					return new SqlFunction(expr.Type, "Count", sql.Select.Columns[0]);
				}

				return SqlFunction.CreateCount(expr.Type, groupBy.SqlQuery);
			}

			if (expr.Arguments.Count > 1)
				for (var i = 1; i < expr.Arguments.Count; i++)
					args[i - 1] = ParseExpression(ParseLambdaArgument(expr, i), groupBy);
			else
			{
				if (expr.Arguments[0].NodeType == ExpressionType.Call)
				{
					var arg = expr.Arguments[0];

					if (arg.NodeType == ExpressionType.Call)
					{
						var call = (MethodCallExpression)arg;

						if (call.Method.Name == "Select" && call.IsQueryableMethod((seq,l) =>
						{
							if (seq.NodeType == ExpressionType.Parameter)
							{
								args = new ISqlExpression[1];
								args[0] = ParseExpression(l.Body, groupBy);
							}

							return false;
						}))
						{}
					}
				}
			}

			return new SqlFunction(expr.Type, expr.Method.Name, args);
		}

		static Expression ParseLambdaArgument(Expression pi, int idx)
		{
			var expr = (MethodCallExpression)pi;
			var arg  = expr.Arguments[idx];
			
			arg.IsLambda(new Func<ParameterExpression,bool>[]
				{ _ => true },
				body => { arg = body; return true; },
				_ => true);

			return arg;
		}

		#endregion

		#region ParseSubQuery

		ISqlExpression ParseSubQuery(LambdaInfo lambda, Expression expr, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			var parentQueries = queries.Select(q => new ParentQuery { Parent = q, Parameter = q.Lambda.Parameters.FirstOrDefault() }).ToList();

			if (lambda != null && queries.Length > 0)
				parentQueries.Add(new ParentQuery { Parent = queries[0], Parameter = lambda.Parameters.FirstOrDefault() });

			ParentQueries.InsertRange(0, parentQueries);
			var sql = CurrentSql;

			CurrentSql = new SqlQuery { ParentSql = sql };

			var prev = _isSubQueryParsing;
			_isSubQueryParsing = true;

			var seq = ParseSequence(expr)[0];

			_isSubQueryParsing = prev;

			if (seq.Fields.Count == 1 && CurrentSql.Select.Columns.Count == 0)
			{
				seq.Fields[0].Select(this);
			}

			var result = CurrentSql;

			CurrentSql = sql;
			ParentQueries.RemoveRange(0, parentQueries.Count);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region IsSubQuery

		bool IsSubQuery(Expression expr, params QuerySource[] queries)
		{
			return IsSubQuery(expr, false, queries);
		}

		bool IsSubQuery(Expression expression, bool ignoreMembers, params QuerySource[] queries)
		{
			if (queries.Length > 0 && queries[0] is QuerySource.SubQuerySourceColumn)
				return false;

			switch (expression.NodeType)
			{
				case ExpressionType.Call:
					{
						var call = expression as MethodCallExpression;

						if (call.Method.DeclaringType == typeof(Queryable) || call.Method.DeclaringType == typeof(Enumerable))
						{
							var arg = call.Arguments[0];

							if (arg.NodeType == ExpressionType.Call)
								return IsSubQuery(arg, queries);

							var qs = queries;

							if (queries.Length > 0 && queries[0].GetType() == typeof(QuerySource.Expr))
								qs = new[] { queries[0].BaseQuery }.Concat(queries).ToArray();

							if (IsSubQuerySource(arg, qs))
								return true;

							/*
							if (arg.NodeType == ExpressionType.MemberAccess)
							{
								var ma = (MemberExpression)arg;

								if (ma.Expression != null && ma.Expression.NodeType == ExpressionType.Parameter)
								{
									var gt = TypeHelper.GetGenericType(typeof(IEnumerable<>), ma.Type);

									if (gt != null)
									{
										var isGroupJoin =
											queries.Length != 0 && queries[0].Sources.Length != 0 &&
											(queries[0].           Sources.Length > 1 && queries[0].           Sources[1] is QuerySource.GroupJoin ||
											 queries[0].Sources[0].Sources.Length > 1 && queries[0].Sources[0].Sources[1] is QuerySource.GroupJoin);

										if (!isGroupJoin)
											return true;
									}

									//return !CanBeCompiled(parseInfo);
								}
							}
							*/
						}

						if (IsIEnumerableType(expression))
							return !CanBeCompiled(expression);
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expression;

						if (IsSubQueryMember(expression) && IsSubQuerySource(ma.Expression, queries))
							return !CanBeCompiled(expression);

						if (!ignoreMembers && IsIEnumerableType(expression))
							return !CanBeCompiled(expression);

						if (ma.Expression != null)
							return IsSubQuery(ma.Expression, true, queries);
					}

					break;
			}

			return false;
		}

		bool IsSubQuerySource(Expression expr, params QuerySource[] queries)
		{
			if (expr == null)
				return false;

			var source = GetSource(null, expr, queries);

			while (source is QuerySource.SubQuerySourceColumn)
				source = ((QuerySource.SubQuerySourceColumn)source).SourceColumn;

			var tbl = source as QuerySource.Table;

			if (tbl != null)
				return true;

			while (expr != null && expr.NodeType == ExpressionType.MemberAccess)
				expr = ((MemberExpression)expr).Expression;

			return expr != null && expr.NodeType == ExpressionType.Constant;
		}

		static bool IsSubQueryMember(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Call:
					{
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expr;

						if (IsListCountMember(ma.Member))
							return true;
					}

					break;
			}

			return false;
		}

		static bool IsIEnumerableType(Expression expr)
		{
			var type = expr.Type;

			var res  = type.IsClass
				&& type != typeof(string)
				&& type != typeof(byte[])
				&& TypeHelper.IsSameOrParent(typeof(IEnumerable), type);

			if (res && expr.NodeType == ExpressionType.MemberAccess)
				res = TypeHelper.GetAttributes(type, typeof(IgnoreIEnumerableAttribute)).Length == 0;

			return res;
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
							return IsServerSideOnly(l.Body.UnwrapLambda());

						var attr = GetFunctionAttribute(ex.Member);
						return attr != null && attr.ServerSideOnly;
					}

				case ExpressionType.Call:
					{
						var e = (MethodCallExpression)expr;

						if (e.Method.DeclaringType == typeof(Enumerable))
						{
							switch (e.Method.Name)
							{
								case "Count":
								case "Average":
								case "Min":
								case "Max":
								case "Sum":
									return IsQueryMember(e.Arguments[0]);
							}
						}
						else if (e.Method.DeclaringType == typeof(Queryable))
						{
							switch (e.Method.Name)
							{
								case "Any":
								case "All":
								case "Contains":
									return true;
							}
						}
						else
						{
							var l = ConvertMember(e.Method);

							if (l != null)
								return l.Body.UnwrapLambda().Find(IsServerSideOnly) != null;

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

							if (ExpressionHelper.IsConstant(ma.Member.DeclaringType) || IsNullableValueMember(ma.Member))
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
							var info = l.Body.UnwrapLambda();

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
							return l.Body.UnwrapLambda().Find(PreferServerSide) != null;

						var attr = GetFunctionAttribute(e.Method);
						return attr != null && attr.PreferServerSide && !CanBeCompiled(expr);
					}
			}

			return false;
		}

		#endregion

		#region CanBeTranslatedToSql

		bool CanBeTranslatedToSql(LambdaInfo lambda, Expression expr, bool canBeCompiled, params QuerySource[] queries)
		{
			return null == expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)pi;
							var l  = ConvertMember(ma.Member);

							if (l != null)
								return !CanBeTranslatedToSql(lambda, l.Body.UnwrapLambda(), canBeCompiled, queries);

							var attr = GetFunctionAttribute(ma.Member);

							if (attr == null && !IsNullableValueMember(ma.Member))
							{
								if (IsListCountMember(ma.Member))
								{
									var src = GetSource(null, ma.Expression, queries);
									if (src == null)
										goto case ExpressionType.Parameter;
								}
								else
									goto case ExpressionType.Parameter;
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							var field = GetField(lambda, pi, queries);

							if (field == null && canBeCompiled)
								return !CanBeCompiled(pi);

							break;
						}

					case ExpressionType.Call:
						{
							var e = pi as MethodCallExpression;

							if (e.Method.DeclaringType != typeof(Enumerable))
							{
								var cm = ConvertMethod(e);

								if (cm != null)
									return !CanBeTranslatedToSql(lambda, cm, canBeCompiled, queries);

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

		#endregion

		#region Predicate Parser

		ISqlPredicate ParsePredicate(LambdaInfo lambda, Expression expression, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(expression);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			try
			{
				switch (expression.NodeType)
				{
					case ExpressionType.Equal :
					case ExpressionType.NotEqual :
					case ExpressionType.GreaterThan :
					case ExpressionType.GreaterThanOrEqual :
					case ExpressionType.LessThan :
					case ExpressionType.LessThanOrEqual :
						{
							var e = expression as BinaryExpression;
							return ParseCompare(lambda, expression.NodeType, e.Left, e.Right, queries);
						}

					case ExpressionType.Call:
						{
							var e = expression as MethodCallExpression;

							ISqlPredicate predicate = null;

							if (e.Method.Name == "Equals" && e.Object != null && e.Arguments.Count == 1)
								return ParseCompare(lambda, ExpressionType.Equal, e.Object, e.Arguments[0], queries);

							if (e.Method.DeclaringType == typeof (string))
							{
								switch (e.Method.Name)
								{
									case "Contains"   : predicate = ParseLikePredicate(e, "%", "%", queries); break;
									case "StartsWith" : predicate = ParseLikePredicate(e, "",  "%", queries); break;
									case "EndsWith"   : predicate = ParseLikePredicate(e, "%", "",  queries); break;
								}
							}
							else if (e.Method.DeclaringType == typeof (Enumerable))
							{
								switch (e.Method.Name)
								{
									case "Contains" :
										predicate = ParseInPredicate(e, queries);
										break;
								}
							}
							else if (TypeHelper.IsSameOrParent(typeof(IList), e.Method.DeclaringType))
							{
								switch (e.Method.Name)
								{
									case "Contains" :
										predicate = ParseInPredicate(e, queries);
										break;
								}
							}
							else if (e.Method == Functions.String.Like11) predicate = ParseLikePredicate(e, queries);
							else if (e.Method == Functions.String.Like12) predicate = ParseLikePredicate(e, queries);
							else if (e.Method == Functions.String.Like21) predicate = ParseLikePredicate(e, queries);
							else if (e.Method == Functions.String.Like22) predicate = ParseLikePredicate(e, queries);

							if (predicate != null)
								return Convert(predicate);

							break;
						}

					case ExpressionType.Conditional:
						return Convert(new SqlQuery.Predicate.ExprExpr(
							ParseExpression(lambda, expression, queries),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(true)));

					case ExpressionType.MemberAccess:
						{
							var e = expression as MemberExpression;

							if (e.Member.Name == "HasValue" && 
								e.Member.DeclaringType.IsGenericType && 
								e.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								var expr = ParseExpression(lambda, e.Expression, queries);
								return Convert(new SqlQuery.Predicate.IsNull(expr, true));
							}

							break;
						}

					case ExpressionType.TypeIs:
						{
							var e     = expression as TypeBinaryExpression;
							var table = GetSource(lambda, e.Expression, queries) as QuerySource.Table;

							if (table != null && table.InheritanceMapping.Count > 0)
								return MakeIsPredicate(table, e.TypeOperand);

							break;
						}
				}

				var ex = ParseExpression(lambda, expression, queries);

				if (SqlExpression.NeedsEqual(ex))
					return Convert(new SqlQuery.Predicate.ExprExpr(ex, SqlQuery.Predicate.Operator.Equal, new SqlValue(true)));
				else
					return Convert(new SqlQuery.Predicate.Expr(ex));
			}
			finally
			{
				ParsingTracer.DecIndentLevel();
			}
		}

		#region ParseCompare

		ISqlPredicate ParseCompare(LambdaInfo lambda, ExpressionType nodeType, Expression left, Expression right, QuerySource[] queries)
		{
			switch (nodeType)
			{
				case ExpressionType.Equal    :
				case ExpressionType.NotEqual :

					var p = ParseObjectComparison(nodeType, lambda, left, queries, lambda, right, queries);
					if (p != null)
						return p;

					p = ParseObjectNullComparison(lambda, left, right, queries, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					p = ParseObjectNullComparison(lambda, right, left, queries, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					if (left.NodeType == ExpressionType.New || right.NodeType == ExpressionType.New)
					{
						p = ParseNewObjectComparison(nodeType, left, right, queries);
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
				var p = ParseEnumConversion(left, op, right, queries);
				if (p != null)
					return p;
			}

			var l = ParseExpression(lambda, left,  queries);
			var r = ParseExpression(lambda, right, queries);

			switch (nodeType)
			{
				case ExpressionType.Equal   :
				case ExpressionType.NotEqual:

					if (!CurrentSql.ParameterDependent && (l is SqlParameter || r is SqlParameter) && l.CanBeNull() && r.CanBeNull())
						CurrentSql.ParameterDependent = true;

					break;
			}

			if (l is SqlQuery.SearchCondition)
				l = Convert(new SqlFunction(typeof(bool), "CASE", l, new SqlValue(true), new SqlValue(false)));
			//l = Convert(new SqlFunction("CASE",
			//	l, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)l) }), new SqlValue(false),
			//	new SqlValue(false)));

			if (r is SqlQuery.SearchCondition)
				r = Convert(new SqlFunction(typeof(bool), "CASE", r, new SqlValue(true), new SqlValue(false)));
			//r = Convert(new SqlFunction("CASE",
			//	r, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)r) }), new SqlValue(false),
			//	new SqlValue(false)));

			return Convert(new SqlQuery.Predicate.ExprExpr(l, op, r));
		}

		#endregion

		#region ParseEnumConversion

		ISqlPredicate ParseEnumConversion(Expression left, SqlQuery.Predicate.Operator op, Expression right, QuerySource[] queries)
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

			var nullValue = _info.MappingSchema.GetNullValue(type);

			if (nullValue != null)
				dic.Add(nullValue, null);

			var mapValues = _info.MappingSchema.GetMapValues(type);

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
						var    origValue = Enum.Parse(type, Enum.GetName(type, ((ConstantExpression)value).Value));
						object mapValue;

						if (!dic.TryGetValue(origValue, out mapValue))
							return null;

						ISqlExpression l, r;

						if (left.NodeType == ExpressionType.Convert)
						{
							l = ParseExpression(operand, queries);
							r = new SqlValue(mapValue);
						}
						else
						{
							r = ParseExpression(operand, queries);
							l = new SqlValue(mapValue);
						}

						return Convert(new SqlQuery.Predicate.ExprExpr(l, op, r));
					}

				case ExpressionType.Convert:
					{
						value = ((UnaryExpression)value).Operand;

						var l = ParseExpression(operand, queries);
						var r = ParseExpression(value,   queries);

						if (l is SqlParameter) SetParameterEnumConverter((SqlParameter)l, type, _info.MappingSchema);
						if (r is SqlParameter) SetParameterEnumConverter((SqlParameter)r, type, _info.MappingSchema);

						return Convert(new SqlQuery.Predicate.ExprExpr(l, op, r));
					}
			}

			return null;
		}

		static void SetParameterEnumConverter(SqlParameter p, Type type, MappingSchema ms)
		{
			if (p.ValueConverter == null)
			{
				p.ValueConverter = o => ms.MapEnumToValue(o, type);
			}
			else
			{
				var converter = p.ValueConverter;
				p.ValueConverter = o => ms.MapEnumToValue(converter(o), type);
			}
		}

		#endregion

		#region ParseObjectNullComparison

		ISqlPredicate ParseObjectNullComparison(LambdaInfo lambda, Expression left, Expression right, QuerySource[] queries, bool isEqual)
		{
			if (right.NodeType == ExpressionType.Constant && ((ConstantExpression)right).Value == null)
			{
				if (left.NodeType == ExpressionType.MemberAccess || left.NodeType == ExpressionType.Parameter)
				{
					var field = GetField(lambda, left, queries);

					if (field is QuerySource.GroupJoin)
					{
						var join = (QuerySource.GroupJoin)field;
						var expr = join.CheckNullField.GetExpressions(this)[0];

						return Convert(new SqlQuery.Predicate.IsNull(expr, !isEqual));
					}

					if (field is QuerySource || field == null && left.NodeType == ExpressionType.Parameter)
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
				}
			}

			return null;
		}

		#endregion

		#region ParseObjectComparison

		ISqlPredicate ParseObjectComparison(
			ExpressionType nodeType,
			LambdaInfo leftLambda,  Expression left,  QuerySource[] leftQueries,
			LambdaInfo rightLambda, Expression right, QuerySource[] rightQueries)
		{
			var qsl = GetSource(leftLambda,  left,  leftQueries);
			var qsr = GetSource(rightLambda, right, rightQueries);

			var sl = qsl as QuerySource.Table;
			var sr = qsr as QuerySource.Table;

			if (qsl != null) for (var query = qsl; sl == null; query = query.BaseQuery)
			{
				sl = query as QuerySource.Table;
				if (!(query is QuerySource.SubQuery))
					break;
			}

			if (qsr != null) for (var query = qsr; sr == null; query = query.BaseQuery)
			{
				sr = query as QuerySource.Table;
				if (!(query is QuerySource.SubQuery))
					break;
			}

			if (qsl != null)
				for (var query = qsl as QuerySource.SubQuerySourceColumn; query != null && sl == null; query = query.SourceColumn as QuerySource.SubQuerySourceColumn)
					sl = query.SourceColumn as QuerySource.Table;

			if (qsr != null)
				for (var query = qsr as QuerySource.SubQuerySourceColumn; query != null && sr == null; query = query.SourceColumn as QuerySource.SubQuerySourceColumn)
					sr = query.SourceColumn as QuerySource.Table;

			if (sl == null && sr == null)
				return null;

			if (sl == null)
			{
				var r = right;
				right = left;
				left  = r;

				var rq = rightQueries;
				rightQueries = leftQueries;
				leftQueries  = rq;

				sl    = sr;
				sr    = null;
			}

			var isNull = right is ConstantExpression && ((ConstantExpression)right).Value == null;
			var cols   = sl.GetKeyFields(true);

			var condition = new SqlQuery.SearchCondition();
			var ta        = TypeAccessor.GetAccessor(right.Type != typeof(object) ? right.Type : left.Type);

			foreach (QueryField.Column col in cols)
			{
				var mi = ta[col.Field.Name].MemberInfo;

				QueryField rcol = null;

				if (sr != null)
				{
					rcol = GetField(rightLambda, Expression.MakeMemberAccess(right, mi), rightQueries);

					var column = rcol as QueryField.Column;

					if (column == null && rcol is QueryField.SubQueryColumn)
					{
						var sc = rcol as QueryField.SubQueryColumn;

						while (sc != null)
						{
							column = sc.Field as QueryField.Column;
							if (column != null)
								break;
							sc = sc.Field as QueryField.SubQueryColumn;
						}
					}

					if (column != null && column.Table.ParentAssociation != null)
					{
						foreach (var c in column.Table.ParentAssociationJoin.Condition.Conditions)
						{
							var ee = (SqlQuery.Predicate.ExprExpr)c.Predicate;

							if (ee.Expr2 == column.Field)
							{
								var fld = rightQueries[0].GetField((SqlField)ee.Expr1);

								if (fld != null)
								{
									rcol = fld;
									break;
								}
}
						}
					}
				}

				var lcol = GetField(leftLambda, Expression.MakeMemberAccess(left, mi), leftQueries);

				{
					var column = lcol as QueryField.Column;

					if (column == null && lcol is QueryField.SubQueryColumn)
					{
						var sc = lcol as QueryField.SubQueryColumn;

						while (sc != null)
						{
							column = sc.Field as QueryField.Column;
							if (column != null)
								break;
							sc = sc.Field as QueryField.SubQueryColumn;
						}
					}

					if (column != null && column.Table.ParentAssociation != null)
					{
						foreach (var c in column.Table.ParentAssociationJoin.Condition.Conditions)
						{
							var ee = (SqlQuery.Predicate.ExprExpr)c.Predicate;

							if (ee.Expr2 == column.Field)
							{
								var fld = leftQueries[0].GetField((SqlField)ee.Expr1);

								if (fld != null)
								{
									lcol = fld;
									break;
								}
							}
						}
					}
				}

				var rex =
					isNull ?
						new SqlValue(right.Type, null) :
						sr != null ?
							rcol.GetExpressions(this)[0] :
							GetParameter(right, mi);

				var predicate = Convert(new SqlQuery.Predicate.ExprExpr(
					lcol.GetExpressions(this)[0],
					nodeType == ExpressionType.Equal ? SqlQuery.Predicate.Operator.Equal : SqlQuery.Predicate.Operator.NotEqual,
					rex));

				condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			if (nodeType == ExpressionType.NotEqual)
				foreach (var c in condition.Conditions)
					c.IsOr = true;

			return condition;
		}

		ISqlPredicate ParseNewObjectComparison(ExpressionType nodeType, Expression left, Expression right, params QuerySource[] queries)
		{
			left  = ConvertExpression(left);
			right = ConvertExpression(right);

			var condition = new SqlQuery.SearchCondition();

			if (left.NodeType != ExpressionType.New)
			{
				var temp = left;
				left  = right;
				right = temp;
			}

			var newRight = right as NewExpression;
			var newExpr  = (NewExpression)left;

			if (newExpr.Members == null)
				return null;

			for (var i = 0; i < newExpr.Arguments.Count; i++)
			{
				var lex = ParseExpression(newExpr.Arguments[i], queries);
				var rex =
					right.NodeType == ExpressionType.New ?
						ParseExpression(newRight.Arguments[i], queries) :
						GetParameter(right, newExpr.Members[i]);

				var predicate = Convert(new SqlQuery.Predicate.ExprExpr(
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

			var par    = ReplaceParameter(ExpressionAccessors, ex, _ => {});
			var expr   = Expression.MakeMemberAccess(par, member);
			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,Expression,object[],object>>(
				Expression.Convert(expr, typeof(object)),
				new [] { _infoParam, ExpressionParam, ParametersParam });

			var p = new ExpressionInfo<T>.Parameter
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, member.Name, null)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p.SqlParameter;
		}

		static Expression ConvertExpression(Expression expr)
		{
			var ret = expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
					case ExpressionType.New:
						return true;
				}

				return false;
			});

			if (ret == null)
				throw new NotImplementedException();

			return ret;
		}

		#endregion

		#region ParseInPredicate

		private ISqlPredicate ParseInPredicate(Expression expression, params QuerySource[] queries)
		{
			var e        = expression as MethodCallExpression;
			var argIndex = e.Object != null ? 0 : 1;

			var expr = ParseExpression(e.Arguments[argIndex], queries);
			var arr  = e.Object ?? e.Arguments[0];

			switch (arr.NodeType)
			{
				case ExpressionType.NewArrayInit:
					{
						var newArr = (NewArrayExpression)arr;

						if (newArr.Expressions.Count == 0)
							return new SqlQuery.Predicate.Expr(new SqlValue(false));

						var exprs  = new ISqlExpression[newArr.Expressions.Count];

						for (var i = 0; i < newArr.Expressions.Count; i++)
							exprs[i] = ParseExpression(newArr.Expressions[i], queries);

						return new SqlQuery.Predicate.InList(expr, false, exprs);
					}

				default:
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

		private ISqlPredicate ParseLikePredicate(Expression expression, string start, string end, params QuerySource[] queries)
		{
			var e = expression as MethodCallExpression;
			var o = ParseExpression(e.Object,       queries);
			var a = ParseExpression(e.Arguments[0], queries);

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

				ep = new ExpressionInfo<T>.Parameter
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

		private ISqlPredicate ParseLikePredicate(Expression expression, params QuerySource[] queries)
		{
			var e  = expression as MethodCallExpression;
			var a1 = ParseExpression(e.Arguments[0], queries);
			var a2 = ParseExpression(e.Arguments[1], queries);

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ParseExpression(e.Arguments[2], queries);

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

		ISqlPredicate MakeIsPredicate(QuerySource.Table table, Type typeOperand)
		{
			if (typeOperand == table.ObjectType && table.InheritanceMapping.Count(m => m.Type == typeOperand) == 0)
				return Convert(new SqlQuery.Predicate.Expr(new SqlValue(true)));

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
									Convert(new SqlQuery.Predicate.ExprExpr(
										table.Columns[table.InheritanceDiscriminators[m.i]].Field,
										SqlQuery.Predicate.Operator.NotEqual,
										new SqlValue(m.m.Code)))));
						}

						return cond;
					}

				case 1:
					return Convert(new SqlQuery.Predicate.ExprExpr(
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
									Convert(new SqlQuery.Predicate.ExprExpr(
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

		#region Search Condition Parser

		void ParseSearchCondition(ICollection<SqlQuery.Condition> conditions, LambdaInfo lambda, Expression expression, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(expression);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			if (IsSubQuery(expression, queries))
			{
				var cond = ParseConditionSubQuery(expression, queries);

				if (cond != null)
				{
					conditions.Add(cond);
					ParsingTracer.DecIndentLevel();
					return;
				}
			}

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var e = expression as BinaryExpression;

						ParseSearchCondition(conditions, lambda, e.Left,  queries);
						ParseSearchCondition(conditions, lambda, e.Right, queries);

						break;
					}

				case ExpressionType.OrElse:
					{
						var e           = expression as BinaryExpression;
						var orCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(orCondition.Conditions, lambda, e.Left,  queries);
						orCondition.Conditions[orCondition.Conditions.Count - 1].IsOr = true;
						ParseSearchCondition(orCondition.Conditions, lambda, e.Right, queries);

						conditions.Add(new SqlQuery.Condition(false, orCondition));

						break;
					}

				case ExpressionType.Not:
					{
						var e            = expression as UnaryExpression;
						var notCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(notCondition.Conditions, lambda, e.Operand, queries);

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
					var predicate = ParsePredicate(lambda, expression, queries);
					conditions.Add(new SqlQuery.Condition(false, predicate));
					break;
			}

			ParsingTracer.DecIndentLevel();
		}

		#region ParsePredicateSubQuery

		SqlQuery.Condition ParseConditionSubQuery(Expression expr, params QuerySource[] queries)
		{
			SqlQuery.Condition cond = null;

			if (expr.NodeType == ExpressionType.Call)
			{
				Func<SqlQuery.Condition> func = null;

				((MethodCallExpression)expr).Match
				(
					pi => pi.IsQueryableMethod(pexpr =>
					{
						switch (pi.Method.Name)
						{
							case "Any" : func = () => ParseAnyCondition(SetType.Any, pexpr, null, null); return true;
						}
						return false;
					}),
					pi => pi.IsQueryableMethod((pexpr,l) =>
					{
						switch (pi.Method.Name)
						{
							case "Any" : func = () => ParseAnyCondition(SetType.Any, pexpr, l, null); return true;
							case "All" : func = () => ParseAnyCondition(SetType.All, pexpr, l, null); return true;
						}
						return false;
					}),
					pi =>
					{
						Expression s = null;
						return pi.IsQueryableMethod2("Contains", seq => s = seq, ex =>
						{
							func = () =>
							{
								var param  = Expression.Parameter(ex.Type, ex.NodeType == ExpressionType.Parameter ? ((ParameterExpression)ex).Name : "t");
								var lambda = new LambdaInfo(Expression.Equal(param, ex), param);
								return ParseAnyCondition(SetType.In, s, lambda, ex);
							};
							return true;
						});
					}
				);

				if (func != null)
				{
					var parentQueries = queries.Select(q => new ParentQuery { Parent = q, Parameter = q.Lambda.Parameters.FirstOrDefault()}).ToList();

					ParentQueries.InsertRange(0, parentQueries);

					cond = func();

					ParentQueries.RemoveRange(0, parentQueries.Count);
				}
			}

			return cond;
		}

		#endregion

		#endregion

		#region Helpers

		QuerySource.Table CreateTable(SqlQuery sqlQuery, LambdaInfo lambda)
		{
			var table = new QuerySource.Table(_info.MappingSchema, sqlQuery, lambda);

			if (table.ObjectType != table.OriginalType)
			{
				var predicate = MakeIsPredicate(table, table.OriginalType);

				if (predicate.GetType() != typeof(SqlQuery.Predicate.Expr))
					CurrentSql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			return table;
		}

		QuerySource.Table CreateTable(SqlQuery sqlQuery, Type type)
		{
			var table = new QuerySource.Table(_info.MappingSchema, sqlQuery, type);

			if (table.ObjectType != table.OriginalType)
			{
				var predicate = MakeIsPredicate(table, table.OriginalType);

				if (predicate.GetType() != typeof(SqlQuery.Predicate.Expr))
					CurrentSql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			return table;
		}

		QueryField GetField(LambdaInfo lambda, Expression expr, params QuerySource[] queries)
		{
			foreach (var query in queries)
			{
				var field = query.GetField(lambda, expr, 0);

				if (field != null)
					return field;
			}

			ParameterExpression param = null;

			for (var ex = expr; ex != null; )
			{
				switch (ex.NodeType)
				{
					case ExpressionType.MemberAccess:
						ex = ((MemberExpression)ex).Expression;
						continue;

					case ExpressionType.Parameter:
						param = (ParameterExpression)ex;
						goto default;

					default:
						ex = null;
						break;
				}
			}

			if (param != null)
			{
				foreach (var query in ParentQueries)
				{
					if (query.Parameter == param)
					{
						var field = query.Parent.GetField(null, expr, 0);

						if (field != null)
							return field;

						if (param == expr && query.Parent is QuerySource.GroupJoin)
							return query.Parent;
					}
				}
			}

			foreach (var query in ParentQueries)
			{
				var field = query.Parent.GetField(null, expr, 0);

				if (field != null)
					return field;

				if (query.Parameter == expr)
					return query.Parent;
			}

			return null;
		}

		QuerySource GetSource(LambdaInfo lambda, Expression expr, params QuerySource[] queries)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Parameter:
					if (lambda != null)
					{
						for (var i = 0; i < lambda.Parameters.Length; i++)
							if (lambda.Parameters[i] == expr)
								return queries[i];

						foreach (var query in ParentQueries)
							if (query.Parameter == expr)
								return query.Parent;
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expr;

						if (IsListCountMember(ma.Member))
							return null;

						if (lambda != null && lambda.Parameters.Length > 0 && ma.Expression == lambda.Parameters[0])
						{
							foreach (var query in queries)
							{
								var gb = query as QuerySource.GroupBy;
								if (gb != null && gb.BaseQuery.ObjectType == expr.Type)
									return gb.BaseQuery;
							}
						}
					}

					break;
			}

			foreach (var query in queries)
			{
				var field = query.GetField(lambda, expr, 0);

				if (field != null)
				{
					if (field is QuerySource)
						return (QuerySource)field;

					var sq = field as QueryField.SubQueryColumn;

					if (sq != null)
					{
						while (sq.Field is QueryField.SubQueryColumn)
							sq = (QueryField.SubQueryColumn)sq.Field;

						return sq.Field as QuerySource;
					}

					return null;
				}
			}

			foreach (var query in ParentQueries)
			{
				var field = query.Parent.GetField(null, expr, 0) as QuerySource;

				if (field != null)
					return field;
			}

			return null;
		}

		static QuerySource[] Concat(QuerySource[] q1, QuerySource[] q2)
		{
			if (q2 == null || q2.Length == 0) return q1;
			if (q1 == null || q1.Length == 0) return q2;

			return q1.Concat(q2).ToArray();
		}

		static QuerySource[] Concat(QuerySource[] q1, ICollection<ParentQuery> q2)
		{
			if (q2 == null || q2.Count == 0) return q1;
			return Concat(q1, q2.Select(q => q.Parent).ToArray());
		}

		static void SetAlias(QuerySource query, string alias)
		{
			if (alias.Contains('<'))
				return;

			query.Match
			(
				table  =>
				{
					if (table.SqlTable.Alias == null)
						table.SqlTable.Alias = alias;
				},
				_ => {},
				subQuery =>
				{
					var table = subQuery.SqlQuery.From.Tables[0];
					if (table.Alias == null)
						table.Alias = alias;
				},
				_ => {},
				_ => {},
				column => SetAlias(column.SourceColumn, alias)
			);
		}

		QuerySource.SubQuery WrapInSubQuery(QuerySource source)
		{
			var result = new QuerySource.SubQuery(new SqlQuery { ParentSql = source.SqlQuery.ParentSql }, source.SqlQuery, source);
			CurrentSql = result.SqlQuery;
			return result;
		}

		SqlFunctionAttribute GetFunctionAttribute(ICustomAttributeProvider member)
		{
			var attrs = member.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

			if (attrs.Length == 0)
				return null;

			SqlFunctionAttribute attr = null;

			foreach (SqlFunctionAttribute a in attrs)
			{
				if (a.SqlProvider == _info.SqlProvider.Name)
				{
					attr = a;
					break;
				}

				if (a.SqlProvider == null)
					attr = a;
			}

			return attr;
		}

		LambdaExpression ConvertMember(MemberInfo mi)
		{
			var lambda = _info.SqlProvider.ConvertMember(mi);

			if (lambda == null)
			{
				var attrs = mi.GetCustomAttributes(typeof(MethodExpressionAttribute), true);

				if (attrs.Length == 0)
					return null;

				MethodExpressionAttribute attr = null;

				foreach (MethodExpressionAttribute a in attrs)
				{
					if (a.SqlProvider == _info.SqlProvider.Name)
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

		public ISqlExpression Convert(ISqlExpression expr)
		{
			_info.SqlProvider.SqlQuery = CurrentSql;
			return _info.SqlProvider.ConvertExpression(expr);
		}

		public ISqlPredicate Convert(ISqlPredicate predicate)
		{
			_info.SqlProvider.SqlQuery = CurrentSql;
			return _info.SqlProvider.ConvertPredicate(predicate);
		}

		static bool IsNullableValueMember(MemberInfo member)
		{
			return
				member.Name == "Value" &&
				member.DeclaringType.IsGenericType &&
				member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		static bool IsListCountMember(MemberInfo member)
		{
			if (member.Name == "Count")
			{
				if (member.DeclaringType.IsSubclassOf(typeof(CollectionBase)))
					return true;

				foreach (var t in member.DeclaringType.GetInterfaces())
					if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>))
						return true;
			}

			return false;
		}

		Expression ConvertNew(NewExpression pi)
		{
			var lambda = ConvertMember(pi.Constructor);

			if (lambda != null)
			{
				var ef    = lambda.Body.UnwrapLambda();
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

		Expression ConvertMethod(MethodCallExpression pi)
		{
			var l = ConvertMember(pi.Method);

			if (l == null)
				return null;

			var ef    = l.Body.UnwrapLambda();
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

			return pie;
		}

		#endregion
	}
}
