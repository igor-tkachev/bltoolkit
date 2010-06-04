using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	using BLToolkit.Linq;
	using Data.Sql;
	using DataProvider;
	using Mapping;
	using Reflection;

	partial class ExpressionParser<T> : ExpressionParser
	{
		#region Init

		public ExpressionParser()
		{
			_info.Queries.Add(new ExpressionInfo<T>.QueryInfo());
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _contextParam    = Expression.Parameter(typeof(QueryContext),      "context");
		readonly ParameterExpression _dataReaderParam = Expression.Parameter(typeof(IDataReader),       "rd");
		readonly ParameterExpression _mapSchemaParam  = Expression.Parameter(typeof(MappingSchema),     "ms");
		readonly ParameterExpression _infoParam       = Expression.Parameter(typeof(ExpressionInfo<T>), "info");

		bool   _isSubQueryParsing;
		int    _currentSql = 0;
		Action _buildSelect;

		Func<QuerySource,LambdaInfo,QuerySource> _convertSource = (s,_) => s;

		SqlQuery CurrentSql
		{
			get { return _info.Queries[_currentSql].SqlQuery;  }
			set { _info.Queries[_currentSql].SqlQuery = value; }
		}

		List<ExpressionInfo<T>.Parameter> CurrentSqlParameters
		{
			get { return _info.Queries[_currentSql].Parameters; }
		}

		#endregion

		#region Parsing Method

		enum ParsingMethod
		{
			Select,
			Where
		}

		readonly List<ParsingMethod> _parsingMethod = new List<ParsingMethod> { ParsingMethod.Select };

		#endregion

		#region Parse

		public ExpressionInfo<T> Parse(
			DataProviderBase      dataProvider,
			MappingSchema         mappingSchema,
			Expression            expression,
			ParameterExpression[] parameters)
		{
			ParsingTracer.WriteLine(expression);
			ParsingTracer.IncIndentLevel();

			ExpressionAccessors = expression.GetExpressionAccessors(ExpressionParam);

			expression = ConvertExpression(expression, parameters);

			_info.DataProvider  = dataProvider;
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;
			_info.Parameters    = parameters;

			expression.Match(
				//
				// db.Select(() => ...)
				//
				pi => pi.IsLambda(0, body => { BuildScalarSelect(body); return true; }),
				//
				// db.Table.ToList()
				//
				pi => pi.IsConstant<IQueryable>((value,_) => BuildSimpleQuery(pi, value.ElementType, null)),
				//
				// from p in db.Table select p
				// db.Table.Select(p => p)
				//
				pi => pi.IsMethod(typeof(Queryable), "Select",
					obj => obj.IsConstant<IQueryable>(),
					arg => arg.IsLambda<T>(
						body => body.NodeType == ExpressionType.Parameter,
						l    => BuildSimpleQuery(pi, typeof(T), l.Parameters[0].Name))),
				//
				// everything else
				//
				pi =>
				{
					ParsingTracer.WriteLine("Sequence parsing phase...");

					var query = ParseSequence(pi);

					ParsingTracer.WriteLine("Select building phase...");

					if (_buildSelect != null)
						_buildSelect();
					else
					{
						var info = BuildSelect(query[0], pi, i => i);
						SetQuery(info);
					}

					return true;
				}
			);

			ParsingTracer.DecIndentLevel();
			return _info;
		}

		static Expression ConvertExpression(Expression expression, ParameterExpression[] parameters)
		{
			var result = expression;

			do
			{
				expression = result;

				// Find let subqueries.
				//
				var dic = new Dictionary<MemberInfo,Expression>();

				expression.Visit(ex =>
				{
					switch (ex.NodeType)
					{
						case ExpressionType.Call:
							{
								var me = (MethodCallExpression)ex;

								LambdaInfo lambda = null;

								if (me.Method.Name == "Select" &&
								    (me.IsQueryableMethod((_, l) => { lambda = l; return true; }) ||
								     me.IsQueryableMethod(null, 2, _ => { }, l => lambda = l)))
								{
									lambda.Body.Visit(e =>
									{
										switch (e.NodeType)
										{
											case ExpressionType.New:
												{
													var ne = (NewExpression)e;

													if (ne.Members == null || ne.Arguments.Count != ne.Members.Count)
														break;

													var args = ne.Arguments.Zip(ne.Members, (a,m) => new { a, m }).ToList();

													var q =
														from a in args
														where
															a.a.NodeType == ExpressionType.Call &&
															a.a.Type != typeof(string) &&
															!a.a.Type.IsArray &&
															TypeHelper.GetGenericType(typeof(IEnumerable<>), a.a.Type) != null
														select a;

													foreach (var item in q)
														dic.Add(item.m, item.a);
												}

												break;
										}
									});
								}
							}

							break;
					}
				});

				result = expression.Convert(ex =>
				{
					switch (ex.NodeType)
					{
						case ExpressionType.Parameter:
							if (parameters != null)
							{
								var idx = Array.IndexOf(parameters, (ParameterExpression)ex);

								if (idx > 0)
									return
										Expression.Convert(
											Expression.ArrayIndex(
												ParametersParam,
												Expression.Constant(Array.IndexOf(parameters, (ParameterExpression)ex))),
											ex.Type);
							}

							break;


						case ExpressionType.MemberAccess:
							{
								var me     = (MemberExpression)ex;
								var member = me.Member;

								if (member is PropertyInfo)
									member = ((PropertyInfo)member).GetGetMethod();

								Expression arg;

								if (dic.Count > 0 && dic.TryGetValue(member, out arg))
									return arg;
							}

							break;
					}

					return ex;
				});
			} while (result != expression);

			return result;
		}

		int _sequenceNumber;

		QuerySource[] ParseSequence(Expression info)
		{
			ParsingTracer.WriteLine(info);
			ParsingTracer.IncIndentLevel();

			_sequenceNumber++;
			var result = ParseSequenceInternal(info);
			_sequenceNumber--;

			ParsingTracer.DecIndentLevel();
			ParsingTracer.WriteLine(result);

			return result;
		}

		QuerySource[] ParseSequenceInternal(Expression info)
		{
			var select = ParseTable(info);

			if (select != null)
				return new[] { select };

			switch (info.NodeType)
			{
				case ExpressionType.Call:
					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)info;

						if (IsListCountMember(ma.Member))
						{
							var qs = ParseSequence(ma.Expression);

							CurrentSql.Select.Expr(SqlFunction.CreateCount(info.Type, CurrentSql.From.Tables[0]), "cnt");

							return qs;
						}

						var originalInfo = info;

						if (TypeHelper.IsSameOrParent(typeof(IQueryable), info.Type))
						{
							info   = GetIQueriable(info);
							select = ParseTable(info);

							if (select != null)
								return new[] { select };
						}

						var association = GetSource(null, info);

						if (association != null)
						{
							association = _convertSource(association, new LambdaInfo(info));

							if (CurrentSql.From.Tables.Count == 0)
							{
								var table = association as QuerySource.Table;

								if (table != null)
								{
									if (_isSubQueryParsing && ParentQueries.Count > 0)
									{
										foreach (var parentQuery in ParentQueries)
										{
											if (parentQuery.Parent.Find(table.ParentAssociation))
											{
												var query = CreateTable(CurrentSql, table.ObjectType);

												foreach (var cond in table.ParentAssociationJoin.Condition.Conditions)
												{
													var predicate = (SqlQuery.Predicate.ExprExpr)cond.Predicate;
													CurrentSql.Where
														.Expr(predicate.Expr1)
														.Equal
														.Field(query.Columns[((SqlField)predicate.Expr2).Name].Field);
												}

												return new[] { query };
											}
										}
									}

									while (table.ParentAssociation != null)
										table = table.ParentAssociation;

									CurrentSql = table.SqlQuery;
								}
							}

							return new[] { association };
						}

						if (ma.Expression != null) switch (ma.Expression.NodeType)
						{
							case ExpressionType.Call:
							case ExpressionType.MemberAccess:
							case ExpressionType.Parameter:
								{
									var pi = (MemberExpression)originalInfo;
									var ex = ma.Expression;
									var qs = ParseSequence(ex);

									qs[0].GetField(pi.Member).Select(this);

									return qs;
								}

							default:
								break;
						}
					}

					break;

				case ExpressionType.Parameter:
					if (ParentQueries.Count > 0)
						foreach (var query in ParentQueries)
							if (query.Parameter == info)
								return new[] { query.Parent };

					goto default;

				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					return ParseSequence(((UnaryExpression)info).Operand);

				default:
					throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info), "info");
			}

			QuerySource[] sequence = null;

			((MethodCallExpression)info).Match
			(
				//
				// db.Table.Method()
				//
				pi => pi.IsQueryableMethod(seq =>
				{
					ParsingTracer.WriteLine(pi);
					ParsingTracer.IncIndentLevel();

					switch (pi.Method.Name)
					{
						case "Distinct"           : sequence = ParseSequence(seq); select = ParseDistinct(sequence[0]);                 break;
						case "First"              : sequence = ParseSequence(seq); ParseElementOperator(ElementMethod.First);           break;
						case "FirstOrDefault"     : sequence = ParseSequence(seq); ParseElementOperator(ElementMethod.FirstOrDefault);  break;
						case "Single"             : sequence = ParseSequence(seq); ParseElementOperator(ElementMethod.Single);          break;
						case "SingleOrDefault"    : sequence = ParseSequence(seq); ParseElementOperator(ElementMethod.SingleOrDefault); break;
						case "Count"              :
						case "Min"                :
						case "Max"                :
						case "Sum"                :
						case "Average"            : sequence = ParseSequence(seq); ParseAggregate(pi, null, sequence[0]); break;
						case "OfType"             : sequence = ParseSequence(seq); select = ParseOfType(pi, sequence);    break;
						case "Any"                : sequence = ParseAny(SetType.Any, seq, null, pi);                      break;
						case "Cast"               :
						case "AsQueryable"        : sequence = ParseSequence(seq);                                        break;
						case "Delete"             : sequence = ParseSequence(seq); ParseDelete(       null,       sequence[0]); break;
						case "Update"             : sequence = ParseSequence(seq); ParseUpdate(       null, null, sequence[0]); break;
						case "Insert"             : sequence = ParseSequence(seq); ParseInsert(false, null, null, sequence[0]); break;
						case "InsertWithIdentity" : sequence = ParseSequence(seq); ParseInsert(true,  null, null, sequence[0]); break;
						default                   :
							ParsingTracer.DecIndentLevel();
							return false;
					}

					ParsingTracer.DecIndentLevel();
					return true;
				}),
				//
				// db.Table.Method(l => ...)
				//
				pi => pi.IsQueryableMethod((seq,l) =>
				{
					switch (pi.Method.Name)
					{
						case "Select"            : sequence = ParseSequence(seq); sequence = ParseSelect    (l, sequence[0]);               break;
						case "Where"             : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]);               break;
						case "SelectMany"        : sequence = ParseSequence(seq); select   = ParseSelectMany(l, null, sequence[0]);         break;
						case "OrderBy"           : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], false, true);  break;
						case "OrderByDescending" : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], false, false); break;
						case "ThenBy"            : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], true,  true);  break;
						case "ThenByDescending"  : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], true,  false); break;
						case "GroupBy"           : sequence = ParseSequence(seq); select   = ParseGroupBy   (l, null, null, sequence[0], pi.Type.GetGenericArguments()[0]);        break;
						case "First"             : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]); ParseElementOperator(ElementMethod.First);           break;
						case "FirstOrDefault"    : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]); ParseElementOperator(ElementMethod.FirstOrDefault);  break;
						case "Single"            : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]); ParseElementOperator(ElementMethod.Single);          break;
						case "SingleOrDefault"   : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]); ParseElementOperator(ElementMethod.SingleOrDefault); break;
						case "Count"             : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]); ParseAggregate(pi, null, sequence[0]); break;
						case "Min"               :
						case "Max"               :
						case "Sum"               :
						case "Average"           : sequence = ParseSequence(seq); ParseAggregate(pi, l, sequence[0]); break;
						case "Any"               : sequence = ParseAny(SetType.Any, seq, l, pi);                      break;
						case "All"               : sequence = ParseAny(SetType.All, seq, l, pi);                      break;
						case "Delete"            : sequence = ParseSequence(seq); ParseDelete(      l, sequence[0]);  break;
						case "Update"            : sequence = ParseSequence(seq); ParseUpdate(null, l, sequence[0]);  break;
						default                  : return false;
					}
					return true;
				}),
				//
				// everything else
				//
				pi => pi.IsQueryableMethod("SelectMany",      1, 2, seq => sequence = ParseSequence(seq), (l1, l2)        => select   = ParseSelectMany(l1, l2,         sequence[0])),
				pi => pi.IsQueryableMethod("Select",             2, seq => sequence = ParseSequence(seq), l               => sequence = ParseSelect    (l,              sequence[0])),
				pi => pi.IsQueryableMethod("Join",                  seq => sequence = ParseSequence(seq), (i, l2, l3, l4) => select   = ParseJoin      (i,  l2, l3, l4, sequence[0])),
				pi => pi.IsQueryableMethod("GroupJoin",             seq => sequence = ParseSequence(seq), (i, l2, l3, l4) => select   = ParseGroupJoin (i,  l2, l3, l4, sequence[0])),
				pi => pi.IsQueryableMethod("GroupBy",         1, 1, seq => sequence = ParseSequence(seq), (l1, l2)        => select   = ParseGroupBy   (l1, l2,   null, sequence[0], pi.Type.GetGenericArguments()[0])),
				pi => pi.IsQueryableMethod("GroupBy",         1, 2, seq => sequence = ParseSequence(seq), (l1, l2)        => select   = ParseGroupBy   (l1, null, l2,   sequence[0], null)),
				pi => pi.IsQueryableMethod("GroupBy",      1, 1, 2, seq => sequence = ParseSequence(seq), (l1, l2, l3)    => select   = ParseGroupBy   (l1, l2,   l3,   sequence[0], null)),
				pi => pi.IsQueryableMethod("Update",          1, 1, seq => sequence = ParseSequence(seq), (l1, l2)        => ParseUpdate(l1, l2, sequence[0])),
				pi => pi.IsQueryableMethod("Set",             1, 1, seq => sequence = ParseSequence(seq), (l1, l2)        => ParseSet   (l1, l2, sequence[0])),
				pi => pi.IsQueryableMethod("Set",             1, 0, seq => sequence = ParseSequence(seq), (l1, l2)        => ParseSet   (l1, l2, sequence[0])),
				pi => pi.IsQueryableMethod("Set",                   seq => sequence = ParseSequence(seq), (l,  ex)        => ParseSet   (l,  ex, sequence[0])),
				pi => pi.IsQueryableMethod("Value",           1, 1, seq => sequence = ParseSequence(seq), (l1, l2)        => ParseValue (l1, l2, sequence[0])),
				pi => pi.IsQueryableMethod("Value",           1, 0, seq => sequence = ParseSequence(seq), (l1, l2)        => ParseValue (l1, l2, sequence[0])),
				pi => pi.IsQueryableMethod("Value",                 seq => sequence = ParseSequence(seq), (l,  ex)        => ParseValue (l,  ex, sequence[0])),
				pi => pi.IsQueryableMethod("Take",               0, seq => sequence = ParseSequence(seq), l               => ParseTake  (sequence[0], l)),
				pi => pi.IsQueryableMethod("Take",                  seq => sequence = ParseSequence(seq), ex              => ParseTake  (sequence[0], ex)),
				pi => pi.IsQueryableMethod("Skip",               0, seq => sequence = ParseSequence(seq), l               => ParseSkip  (sequence[0], l)),
				pi => pi.IsQueryableMethod("Skip",                  seq => sequence = ParseSequence(seq), ex              => ParseSkip  (sequence[0], ex)),
				pi => pi.IsQueryableMethod("ElementAt",          0, seq => sequence = ParseSequence(seq), l               => ParseElementAt(sequence[0], l,  false)),
				pi => pi.IsQueryableMethod("ElementAt",             seq => sequence = ParseSequence(seq), ex              => ParseElementAt(sequence[0], ex, false)),
				pi => pi.IsQueryableMethod("ElementAtOrDefault", 0, seq => sequence = ParseSequence(seq), l               => ParseElementAt(sequence[0], l,  true)),
				pi => pi.IsQueryableMethod("ElementAtOrDefault",    seq => sequence = ParseSequence(seq), ex              => ParseElementAt(sequence[0], ex, true)),
				pi => pi.IsQueryableMethod("Concat",                seq => sequence = ParseSequence(seq), ex => { select = ParseUnion    (sequence[0], ex, true);  return true; }),
				pi => pi.IsQueryableMethod("Union",                 seq => sequence = ParseSequence(seq), ex => { select = ParseUnion    (sequence[0], ex, false); return true; }),
				pi => pi.IsQueryableMethod("Except",                seq => sequence = ParseSequence(seq), ex => { select = ParseIntersect(sequence,    ex, true);  return true; }),
				pi => pi.IsQueryableMethod("Intersect",             seq => sequence = ParseSequence(seq), ex => { select = ParseIntersect(sequence,    ex, false); return true; }),
				pi => pi.IsQueryableMethod("DefaultIfEmpty",        seq => { sequence = ParseDefaultIfEmpty(seq); return sequence != null; }),
				pi => pi.IsQueryableMethod("Insert",             0, seq => sequence = ParseSequence(seq), l               => ParseInsert(false, null, l, sequence[0])),
				pi => pi.IsQueryableMethod("InsertWithIdentity", 0, seq => sequence = ParseSequence(seq), l               => ParseInsert(true,  null, l, sequence[0])),
				pi =>
				{
					Expression into = null;

					return
						pi.IsMethod(null, "Insert", new Func<Expression,bool>[]
						{
							p => { sequence = ParseSequence(p); return true; },
							p => { into = p; return true; },
							p => { p.IsLambda(1, l => ParseInsert(false, into, l, sequence[0]) ); return true; }
						}, _ => true)
						||
						pi.IsMethod(null, "InsertWithIdentity", new Func<Expression,bool>[]
						{
							p => { sequence = ParseSequence(p); return true; },
							p => { into = p; return true; },
							p => { p.IsLambda(1, l => ParseInsert(true, into, l, sequence[0]) ); return true; }
						}, _ => true);
				},
				pi =>
				{
					Expression arg1 = null;
					return pi.IsMethod(null, "Into", new Func<Expression,bool>[]
					{
						p => { arg1 = p; return true; },
						p => { sequence = ParseInto(arg1, p); return true; }
					}, _ => true);
				},
				pi =>
				{
					Expression s = null;
					return pi.IsQueryableMethod("Contains", seq => s = seq, ex => { sequence = ParseContains(s, ex, pi); return true; });
				},
				pi => pi.IsMethod(m =>
				{
					if (m.Method.DeclaringType == typeof(Queryable) || !TypeHelper.IsSameOrParent(typeof(IQueryable), pi.Type))
						return false;

					sequence = ParseSequence(GetIQueriable(info));
					return true;
				}),
				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi), "info"); }
			);

			if (select   == null) return sequence;
			if (sequence == null) return new[] { select };

			return new[] { select }.Concat(sequence).ToArray();
		}

		QuerySource ParseTable(Expression expression)
		{
			if (expression.NodeType == ExpressionType.MemberAccess)
			{
				if (_info.Parameters != null)
				{
					var me = (MemberExpression)expression;

					if (me.Expression == _info.Parameters[0])
						return CreateTable(CurrentSql, new LambdaInfo(expression));
				}
			}

			if (expression.NodeType == ExpressionType.Call)
			{
				if (_info.Parameters != null)
				{
					var mc = (MethodCallExpression)expression;

					if (mc.Object == _info.Parameters[0])
						return CreateTable(CurrentSql, new LambdaInfo(expression));
				}
			}

			QuerySource select = null;

			if (expression.IsConstant<IQueryable>((value,expr) =>
			{
				select = CreateTable(CurrentSql, new LambdaInfo(expr));
				return true;
			}))
			{}

			return select;
		}

		Expression GetIQueriable(Expression expression)
		{
			if (expression.NodeType == ExpressionType.MemberAccess || expression.NodeType == ExpressionType.Call)
			{
				var p    = Expression.Parameter(typeof(Expression), "exp");
				var exas = expression.GetExpressionAccessors(p);
				var expr = ReplaceParameter(exas, expression, _ => {});
				var l    = Expression.Lambda<Func<Expression,IQueryable>>(Expression.Convert(expr, typeof(IQueryable)), new [] { p });
				var qe   = l.Compile();
				var n    = _info.AddQueryableAccessors(expression, qe);

				Expression accessor;

				ExpressionAccessors.TryGetValue(expression, out accessor);

				var path =
					Expression.Call(
						_infoParam,
						Expressor<ExpressionInfo<T>>.MethodExpressor(a => a.GetIQueryable(0, null)),
						new[] { Expression.Constant(n), accessor ?? Expression.Constant(null) });

				var qex  = qe(expression).Expression;

				foreach (var a in qex.GetExpressionAccessors(path))
					if (!ExpressionAccessors.ContainsKey(a.Key))
						ExpressionAccessors.Add(a.Key, a.Value);

				return qex;
			}

			throw new InvalidOperationException();
		}

		#endregion

		#region Parse Select

		QuerySource[] ParseSelect(LambdaInfo l, params QuerySource[] sources)
		{
#if DEBUG && TRACE_PARSING
			foreach (var source in sources)
				ParsingTracer.WriteLine(source);
#endif

			ParsingTracer.IncIndentLevel();

			_parsingMethod.Insert(0, ParsingMethod.Select);

			var select = ParseSelectInternal(l, sources);

			_parsingMethod.RemoveAt(0);

			ParsingTracer.DecIndentLevel();

			if (sources.Length > 0 && select == sources[0])
				return sources;

			return new[] { select }.Concat(sources).ToArray();
		}

		QuerySource ParseSelectInternal(LambdaInfo l, QuerySource[] sources)
		{
			ParsingTracer.WriteLine(l);

			for (var i = 0; i < sources.Length && i < l.Parameters.Length; i++)
				SetAlias(sources[i], l.Parameters[i].Name);

			switch (l.Body.NodeType)
			{
				case ExpressionType.Parameter  :
					for (var i = 0; i < sources.Length; i++)
						if (l.Body == l.Parameters[i])
							return sources[i];

					foreach (var query in ParentQueries)
						if (l.Body == query.Parameter)
							return query.Parent;

					throw new InvalidOperationException();

				case ExpressionType.MemberAccess:
					{
						CheckSubQueryForSelect(sources);

						var src = GetSource(l, l.Body, sources);

						if (src != null)
						{
							src = _convertSource(src, l);

							if (CurrentSql.From.Tables.Count == 0)
							{
								var table = src as QuerySource.Table;

								if (table != null)
								{
									while (table.ParentAssociation != null)
										table = table.ParentAssociation;

									CurrentSql = table.SqlQuery;
								}
							}

							return src;
						}
					}

					goto default;

				case ExpressionType.New        :
					{
						CheckSubQueryForSelect(sources);

						if (_sequenceNumber > 1)
						{
							var pie = ConvertNew((NewExpression)l.Body);
							if (pie != null)
								return ParseSelect(new LambdaInfo(pie, l.Parameters), sources)[0];
						}

						return new QuerySource.Expr(CurrentSql, l, Concat(sources, ParentQueries));
					}

				case ExpressionType.MemberInit :
					{
						CheckSubQueryForSelect(sources);
						return new QuerySource.Expr(CurrentSql, l, Concat(sources, ParentQueries));
					}

				default                        :
					{
						CheckSubQueryForSelect(sources);
						var scalar = new QuerySource.Scalar(CurrentSql, l, Concat(sources, ParentQueries));
						return scalar.Fields[0] is QuerySource ? (QuerySource)scalar.Fields[0] : scalar;
					}
			}
		}

		void CheckSubQueryForSelect(QuerySource[] sources)
		{
			if (CurrentSql.Select.IsDistinct)
				sources[0] = WrapInSubQuery(sources[0]);
		}

		#endregion

		#region Parse SelectMany

		QuerySource ParseSelectMany(LambdaInfo collectionSelector, LambdaInfo resultSelector, QuerySource source)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			if (collectionSelector.Parameters[0] == collectionSelector.Body)
			{
				ParsingTracer.DecIndentLevel();
				return resultSelector == null ? source : ParseSelect(resultSelector, source)[0];
			}

			var sql  = CurrentSql;
			var conv = _convertSource;

			CurrentSql = new SqlQuery();

			var associationList = new Dictionary<QuerySource,QuerySource>();

			_convertSource = (s,l) =>
			{
				var t = s as QuerySource.Table;

				if (t != null && t.ParentAssociation != null)
				{
					t.ParentAssociationJoin.IsWeak   = false;
					t.ParentAssociationJoin.JoinType = SqlQuery.JoinType.Inner;

					var orig = s;

					if (t.ParentAssociation != source)
						s = CreateTable(new SqlQuery(), l);

					associationList.Add(s, orig);
				}

				return s;
			};

			ParentQueries.Insert(0, new ParentQuery { Parent = source, Parameter = collectionSelector.Parameters[0] });
			var seq2 = ParseSequence(collectionSelector.Body);
			ParentQueries.RemoveAt(0);

			_convertSource = conv;

			if (associationList.Count > 0)
			{
				foreach (var a in associationList)
				{
					if (a.Key == a.Value)
					{
						CurrentSql = sql;
						break;
					}

					var assc = (QuerySource.Table)a.Key;
					var orig = (QuerySource.Table)a.Value;

					var current = new SqlQuery();
					var source1 = new QuerySource.SubQuery(current, sql,        source,  false);
					var source2 = new QuerySource.SubQuery(current, CurrentSql, seq2[0], false);
					var join    = SqlQuery.InnerJoin(CurrentSql);

					current.From.Table(sql, join);

					CurrentSql = current;

					foreach (var cond in orig.ParentAssociationJoin.Condition.Conditions)
					{
						var ee = (SqlQuery.Predicate.ExprExpr)cond.Predicate;

						var field1 = (SqlField)ee.Expr2;
						var field2 = assc.SqlTable[((SqlField)ee.Expr2).Name];

						var col1 = source1.GetField(field1);
						var col2 = source2.GetField(field2);

						join
							.Expr(col2.GetExpressions(this)[0])
							.Equal
							.Expr(col1.GetExpressions(this)[0]);
					}

					break;
				}

				ParsingTracer.DecIndentLevel();
				return resultSelector == null ? seq2[0] : ParseSelect(resultSelector, source, seq2[0])[0];
			}

			if ((source.SqlQuery == seq2[0].SqlQuery && CurrentSql.From.Tables.Count == 0) || source.Sources.Contains(seq2[0]))
			{
				CurrentSql = sql;
				ParsingTracer.DecIndentLevel();
				return resultSelector == null ? seq2[0] : ParseSelect(resultSelector, source, seq2[0])[0];
			}

			if (collectionSelector.Body.NodeType == ExpressionType.Call)
			{
				var call = (MethodCallExpression)collectionSelector.Body;

				if (call.Method.Name == "DefaultIfEmpty" &&
					seq2.OfType<QuerySource.Table>().FirstOrDefault(t => t.ParentAssociation != null) == null &&
					(call.Method.DeclaringType == typeof(Enumerable) ||
					 call.Method.DeclaringType == typeof(Queryable)))
				{
					var keyField = seq2[0].Fields.FirstOrDefault(f => !f.CanBeNull());
					var subQuery = seq2[0] as QuerySource.SubQuery ?? WrapInSubQuery(seq2[0]);

					subQuery.CheckNullField = keyField ?? new QueryField.ExprColumn(subQuery, new SqlValue((int?)1), "test");
					subQuery.Fields.Add(subQuery.CheckNullField);

					var current = new SqlQuery();
					var source1 = new QuerySource.SubQuery(current, sql,        source,   false);
					var source2 = new QuerySource.SubQuery(current, CurrentSql, subQuery, false);
					var join    = SqlQuery.LeftJoin(CurrentSql);

					current.From.Table(sql, join);

					var cond = seq2[0].SqlQuery.Where.SearchCondition;

					if (subQuery != seq2[0])
					{
						cond = new QueryVisitor().Convert(cond, e =>
						{
							if (e.ElementType == QueryElementType.SqlField)
							{
								var field = source2.GetField((SqlField)e);
								if (field != null)
									return field.GetExpressions(this)[0];
							}

							return null;
						});
					}

					join.JoinedTable.Condition.Conditions.AddRange(cond.Conditions);

					seq2[0].SqlQuery.Where.SearchCondition.Conditions.Clear();
					CurrentSql = current;

					var result = resultSelector == null ?
						new QuerySource.SubQuery(current, subQuery.SqlQuery, subQuery) :
						ParseSelect(resultSelector, source1, source2)[0];

					ParsingTracer.DecIndentLevel();
					return result;
				}
			}

			/*
			if (CurrentSql.From.Tables.Count == 0)
			{
				var table = seq2[0] as QuerySource.Table;

				if (table != null)
				{
					while (table.ParentAssociation != null)
						table = table.ParentAssociation;

					CurrentSql = table.SqlQuery; //.From.Table(table.SqlTable);
				}
			}
			*/

			/*
			if (seq2[0] is QuerySource.Table)
			{
				var tbl = (QuerySource.Table)seq2[0];

				if (tbl.ParentAssociation == source)
				{
					tbl.ParentAssociationJoin.IsWeak   = false;
					tbl.ParentAssociationJoin.JoinType = SqlQuery.JoinType.Inner;

					CurrentSql = sql;
					ParsingTracer.DecIndentLevel();
					return resultSelector == null ? tbl : ParseSelect(resultSelector, source, tbl);
				}

				if (seq2.Length > 1 && seq2[1] is QuerySource.GroupBy)
				{
					var gby = (QuerySource.GroupBy)seq2[1];

					if (tbl.ParentAssociation == gby.OriginalQuery)
					{
						
					}
				}
			}
			*/

			/*
			if (seq2.Length > 1 && seq2[1] is QuerySource.Table)
			{
				var tbl = (QuerySource.Table)seq2[1];

				if (HasSource(seq2[0], tbl.ParentAssociation))
				{
					tbl.ParentAssociationJoin.IsWeak   = false;
					tbl.ParentAssociationJoin.JoinType = SqlQuery.JoinType.Inner;

					if (seq2[0] == tbl.ParentAssociation)
					{
						CurrentSql = sql;
						ParsingTracer.DecIndentLevel();
						return resultSelector == null ? seq2[0] : ParseSelect(resultSelector, source, seq2[0]);
					}
				}
			}
			*/

			{
				var current = new SqlQuery();
				var source1 = new QuerySource.SubQuery(current, sql,        source);
				var source2 = new QuerySource.SubQuery(current, CurrentSql, seq2[0]);

				//current.From.Table(source1.SubSql);
				//current.From.Table(source2.SubSql);

				CurrentSql = current;

				var result = resultSelector == null ?
					new QuerySource.SubQuery(current, seq2[0].SqlQuery, seq2[0]) :
					ParseSelect(resultSelector, source1, source2)[0];

				ParsingTracer.DecIndentLevel();
				return result;
			}
		}

		#endregion

		#region Parse Join

		QuerySource ParseJoin(
			Expression  inner,
			LambdaInfo  outerKeySelector,
			LambdaInfo  innerKeySelector,
			LambdaInfo  resultSelector,
			QuerySource outerSource)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			CheckExplicitCtor(outerKeySelector.Body);

			var current = new SqlQuery();
			var source1 = new QuerySource.SubQuery(current, CurrentSql, outerSource);

			CurrentSql = new SqlQuery();

			var seq     = ParseSequence(inner)[0];
			var source2 = new QuerySource.SubQuery(current, CurrentSql, seq, false);
			var join    = source2.SubSql.InnerJoin();

			CurrentSql = current;

			current.From.Table(source1.SubSql, join);

			if (outerKeySelector.Body.NodeType == ExpressionType.New)
			{
				var new1 = (NewExpression)outerKeySelector.Body;
				var new2 = (NewExpression)innerKeySelector.Body;

				for (var i = 0; i < new1.Arguments.Count; i++)
				{
					var arg1 = new1.Arguments[i];
					var arg2 = new2.Arguments[i];

					var predicate = ParseObjectComparison(
						ExpressionType.Equal,
						outerKeySelector, arg1, new[] { source1 },
						innerKeySelector, arg2, new[] { source2 });

					if (predicate != null)
						join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
					else
						join
							.Expr(ParseExpression(outerKeySelector, arg1, source1)).Equal
							.Expr(ParseExpression(innerKeySelector, arg2, source2));
				}
			}
			else
			{
				var predicate = ParseObjectComparison(
					ExpressionType.Equal,
					outerKeySelector, outerKeySelector.Body, new[] { source1 },
					innerKeySelector, innerKeySelector.Body, new[] { source2 });

				if (predicate != null)
					join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
				else
					join
						.Expr(ParseExpression(outerKeySelector, outerKeySelector.Body, source1)).Equal
						.Expr(ParseExpression(innerKeySelector, innerKeySelector.Body, source2));
			}

			var result = resultSelector == null ? source2 : ParseSelect(resultSelector, source1, source2)[0];
			ParsingTracer.DecIndentLevel();
			return result;
		}

		static void CheckExplicitCtor(Expression expr)
		{
			if (expr.NodeType == ExpressionType	.MemberInit)
				throw new NotSupportedException(
					string.Format("Explicit construction of entity type '{0}' in query is not allowed.", expr.Type));
		}

		#endregion

		#region Parse GroupJoin

		QuerySource ParseGroupJoin(
			Expression  inner,
			LambdaInfo  outerKeySelector,
			LambdaInfo  innerKeySelector,
			LambdaInfo  resultSelector,
			QuerySource outerSource)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			if (outerKeySelector.Body.NodeType == ExpressionType.MemberInit)
				throw new NotSupportedException(
					string.Format("Explicit construction of entity type '{0}' in query is not allowed.",
					outerKeySelector.Body.Type));

			// Process outer source.
			//
			var current = new SqlQuery();
			var source1 = new QuerySource.SubQuery(current, CurrentSql, outerSource);

			// Process inner source.
			//
			CurrentSql = new SqlQuery();

			var seq     = ParseSequence(inner)[0];
			var source2 = new QuerySource.GroupJoin(current, CurrentSql, seq);
			var join    = source2.SubSql.WeakLeftJoin();

			CurrentSql = current;

			current.From.Table(source1.SubSql, join);

			// Process counter.
			//
			CurrentSql = new SqlQuery { ParentSql = current };

			var cntseq  = ParseSequence(inner)[0];
			var counter = new QuerySource.SubQuery(current, CurrentSql, cntseq, false);
 
			CurrentSql = current;

			counter.SubSql.Select.Columns.Clear();
			counter.SubSql.Select.Expr(SqlFunction.CreateCount(typeof(int), counter.SubSql.From.Tables[0]), "cnt");

			// Make join and where for the counter.
			//
			if (outerKeySelector.Body.NodeType == ExpressionType.New)
			{
				var new1 = (NewExpression)outerKeySelector.Body;
				var new2 = (NewExpression)innerKeySelector.Body;

				for (var i = 0; i < new1.Arguments.Count; i++)
				{
					join
						.Expr(ParseExpression(outerKeySelector, new1.Arguments[i], source1)).Equal
						.Expr(ParseExpression(innerKeySelector, new2.Arguments[i], source2));

					counter.SubSql.Where
						.Expr(ParseExpression(outerKeySelector, new1.Arguments[i], source1)).Equal
						.Expr(ParseExpression(innerKeySelector, new2.Arguments[i], cntseq));
				}
			}
			else
			{
				join
					.Expr(ParseExpression(outerKeySelector, outerKeySelector.Body, source1)).Equal
					.Expr(ParseExpression(innerKeySelector, innerKeySelector.Body, source2));

				counter.SubSql.Where
					.Expr(ParseExpression(outerKeySelector, outerKeySelector.Body, source1)).Equal
					.Expr(ParseExpression(innerKeySelector, innerKeySelector.Body, cntseq));
			}

			if (resultSelector == null)
				return source2;
			
			var select = ParseSelect(resultSelector, source1, source2)[0];

			counter.SubSql.Select.Columns.RemoveRange(1, counter.SubSql.Select.Columns.Count - 1);
			source2.Counter = new QueryField.ExprColumn(select, counter.SubSql, "cnt");

			select.Fields.Add(source2.Counter);

			ParsingTracer.DecIndentLevel();
			return select;
		}

		#endregion

		#region Parse DefaultIfEmpty

		QuerySource[] ParseDefaultIfEmpty(Expression seq)
		{
			if (ParentQueries.Count > 0)
			{
				var groupJoin = ParentQueries[0].Parent.Sources.Where(s => s is QuerySource.GroupJoin).FirstOrDefault();

				if (groupJoin != null)
					groupJoin.SqlQuery.From.Tables[0].Joins[0].IsWeak = false;
			}

			var field = GetField(null, seq);

			if (field != null)
				return new[] { field as QuerySource };

			var conv = _convertSource;

			_convertSource = (s,l) => s;

			var query = ParseSequence(seq);

			_convertSource = conv;

			return query;
		}

		#endregion

		#region Parse OfType

		QuerySource ParseOfType(Expression pi, QuerySource[] sequence)
		{
			var table = sequence[0] as QuerySource.Table;

			if (table != null && table.InheritanceMapping.Count > 0)
			{
				var objectType = pi.Type.GetGenericArguments()[0];

				if (TypeHelper.IsSameOrParent(table.ObjectType, objectType))
				{
					var predicate = MakeIsPredicate(table, objectType);

					if (predicate.GetType() != typeof(SqlQuery.Predicate.Expr))
						CurrentSql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));
				}
			}

			return sequence[0];
		}

		#endregion

		#region Parse Where

		QuerySource ParseWhere(LambdaInfo l, QuerySource select)
		{
			ParsingTracer.WriteLine(l);
			ParsingTracer.WriteLine(select);
			ParsingTracer.IncIndentLevel();

			SetAlias(select, l.Parameters[0].Name);

			bool makeHaving;

			_parsingMethod.Insert(0, ParsingMethod.Where);
			ParentQueries.Insert(0, new ParentQuery { Parent = select, Parameter = l.Parameters[0] });

			if (CheckSubQueryForWhere(l, select, out makeHaving))
				select = WrapInSubQuery(select);

			ParseSearchCondition(
				makeHaving? CurrentSql.Having.SearchCondition.Conditions : CurrentSql.Where.SearchCondition.Conditions,
				l, l.Body, select);

			ParentQueries.RemoveAt(0);
			_parsingMethod.RemoveAt(0);

			ParsingTracer.DecIndentLevel();
			return select;
		}

		bool CheckSubQueryForWhere(LambdaInfo lambda, QuerySource query, out bool makeHaving)
		{
			var checkParameter = query is QuerySource.Scalar && query.Fields[0] is QueryField.ExprColumn;
			var makeSubQuery   = false;
			var isHaving       = false;
			var isWhere        = false;

			lambda.Body.Find(pi =>
			{
				if (IsSubQuery(pi, query))
					return isWhere = true;

				var stopWalking = false;

				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma      = (MemberExpression)pi;
							var isCount = IsListCountMember(ma.Member);

							if (!IsNullableValueMember(ma.Member) && !isCount)
							{
								if (ConvertMember(ma.Member) == null)
								{
									var field = GetField(lambda, pi, query);

									if (field is QueryField.ExprColumn)
										stopWalking = makeSubQuery = true;
								}
							}

							if (isCount)
								stopWalking = isHaving = true;
							else
								isWhere = true;

							break;
						}

					case ExpressionType.Call:
						{
							var e = (MethodCallExpression)pi;

							if (e.Method.DeclaringType == typeof (Enumerable) && e.Method.Name != "Contains")
								return isHaving = true;

							isWhere = true;

							break;
						}

					case ExpressionType.Parameter:
						if (checkParameter)
							stopWalking = makeSubQuery = true;

						isWhere = true;

						break;
				}

				return stopWalking;
			});

			makeHaving = isHaving && !isWhere;
			return makeSubQuery || isHaving && isWhere;
		}

		#endregion

		#region Parse GroupBy

		QuerySource ParseGroupBy(LambdaInfo keySelector, LambdaInfo elementSelector, LambdaInfo resultSelector, QuerySource source, Type groupingType)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			CheckExplicitCtor(keySelector.Body);

			var group   = ParseSelect(keySelector, source)[0];
			var element = elementSelector != null? ParseSelect(elementSelector, source)[0] : null;
			var fields  = new List<QueryField>(group is QuerySource.Table ? group.GetKeyFields(true) : group.Fields);
			var byExprs = new ISqlExpression[fields.Count];
			var wrap    = false;

			for (var i = 0; i < fields.Count; i++)
			{
				var field = fields[i];
				var exprs = field.GetExpressions(this);

				if (exprs == null || exprs.Length != 1)
					throw new LinqException("Cannot group by type '{0}'", keySelector.Body.Type);

				byExprs[i] = exprs[0];

				wrap = wrap || !(exprs[0] is SqlField || exprs[0] is SqlQuery.Column);
			}

			// Can be used instead of GroupBy.Items.Clear().
			//
			//if (!wrap)
			//	wrap = CurrentSql.GroupBy.Items.Count > 0;

			if (wrap)
			{
				var subQuery = WrapInSubQuery(group);

				foreach (var field in fields)
					CurrentSql.GroupBy.Expr(group.SqlQuery.Select.Columns[field.Select(this)[0].Index]);

				group = subQuery;
			}
			else
			{
				CurrentSql.GroupBy.Items.Clear();
				foreach (var field in byExprs)
					CurrentSql.GroupBy.Expr(field);

				new QueryVisitor().Visit(CurrentSql.From, e =>
				{
					if (e.ElementType == QueryElementType.JoinedTable)
					{
						var jt = (SqlQuery.JoinedTable)e;
						if (jt.JoinType == SqlQuery.JoinType.Inner)
							jt.IsWeak = false;
					}
				});
			}

			var result =
				resultSelector == null ?
					new QuerySource.GroupBy(CurrentSql, group, source, keySelector, element, groupingType, wrap, byExprs) :
					ParseSelect(resultSelector, group, source)[0];

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region Parse OrderBy

		QuerySource ParseOrderBy(LambdaInfo lambda, QuerySource source, bool isThen, bool ascending)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			CheckExplicitCtor(lambda.Body);

			if (CurrentSql.Select.TakeValue != null || CurrentSql.Select.SkipValue != null)
				source = WrapInSubQuery(source);

			var order  = ParseSelect(lambda, source)[0];
			var fields = new List<QueryField>(order is QuerySource.Table ? order.GetKeyFields(true).Select(f => f) : order.Fields);

			if (!isThen)
				CurrentSql.OrderBy.Items.Clear();

			foreach (var field in fields)
			{
				var exprs = field.GetExpressions(this);

				if (exprs == null)
					throw new LinqException("Cannot order by type '{0}'", lambda.Body.Type);

				foreach (var expr in exprs)
				{
					var e = expr;

					if (e is SqlQuery.SearchCondition)
					{
						if (e.CanBeNull())
						{
							var notExpr = new SqlQuery.SearchCondition
							{
								Conditions = { new SqlQuery.Condition(true, new SqlQuery.Predicate.Expr(expr, expr.Precedence)) }
							};

							e = Convert(new SqlFunction(e.SystemType, "CASE", expr, new SqlValue(1), notExpr, new SqlValue(0), new SqlValue(null)));
						}
						else
							e = Convert(new SqlFunction(e.SystemType, "CASE", expr, new SqlValue(1), new SqlValue(0)));
					}

					CurrentSql.OrderBy.Expr(e, !ascending);
				}
			}

			ParsingTracer.DecIndentLevel();
			return source;
		}

		#endregion

		#region Parse Take

		bool ParseTake(QuerySource select, Expression value)
		{
			if (value.Type != typeof(int))
				return false;

			//if (!_asParameters.Contains(value.Expr))
			//	_asParameters.Add(value.Expr);

			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseTake(ParseExpression(value, select));

			ParsingTracer.DecIndentLevel();
			return true;
		}

		bool ParseTake(QuerySource select, LambdaInfo value)
		{
			if (value.Body.Type != typeof(int))
				return false;

			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseTake(ParseExpression(value.Body, select));

			ParsingTracer.DecIndentLevel();
			return true;
		}

		void ParseTake(ISqlExpression expr)
		{
			CurrentSql.Select.Take(expr);

			_info.SqlProvider.SqlQuery = CurrentSql;

			if (CurrentSql.Select.SkipValue != null && _info.SqlProvider.IsTakeSupported && !_info.SqlProvider.IsSkipSupported)
			{
				if (CurrentSql.Select.SkipValue is SqlParameter && CurrentSql.Select.TakeValue is SqlValue)
				{
					var skip = (SqlParameter)CurrentSql.Select.SkipValue;
					var parm = (SqlParameter)CurrentSql.Select.SkipValue.Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);
					var conv = parm.ValueConverter;
					var take = (int)((SqlValue)CurrentSql.Select.TakeValue).Value;

					if (conv == null)
						parm.ValueConverter = v => v == null ? null : (object)((int)v + take);
					else
						parm.ValueConverter = v => v == null ? null : (object)((int)conv(v) + take);

					CurrentSql.Select.Take(parm);

					var ep = (from pm in CurrentSqlParameters where pm.SqlParameter == skip select pm).First();

					ep = new ExpressionInfo<T>.Parameter
					{
						Expression   = ep.Expression,
						Accessor     = ep.Accessor,
						SqlParameter = parm
					};

					CurrentSqlParameters.Add(ep);
				}
				else
					CurrentSql.Select.Take(Convert(
						new SqlBinaryExpression(typeof(int), CurrentSql.Select.SkipValue, "+", CurrentSql.Select.TakeValue, Precedence.Additive)));
			}

			if (!_info.SqlProvider.TakeAcceptsParameter)
			{
				var p = CurrentSql.Select.TakeValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}

		#endregion

		#region Parse Skip

		bool ParseSkip(QuerySource select, Expression value)
		{
			if (value.Type != typeof(int))
				return false;

			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseSkip(CurrentSql.Select.SkipValue, ParseExpression(value, select));

			ParsingTracer.DecIndentLevel();
			return true;
		}

		bool ParseSkip(QuerySource select, LambdaInfo value)
		{
			if (value.Body.Type != typeof(int))
				return false;

			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseSkip(CurrentSql.Select.SkipValue, ParseExpression(value.Body, select));

			ParsingTracer.DecIndentLevel();
			return true;
		}

		void ParseSkip(ISqlExpression prevSkipValue, ISqlExpression expr)
		{
			CurrentSql.Select.Skip(expr);

			_info.SqlProvider.SqlQuery = CurrentSql;

			if (CurrentSql.Select.TakeValue != null)
			{
				if (_info.SqlProvider.IsSkipSupported || !_info.SqlProvider.IsTakeSupported)
					CurrentSql.Select.Take(Convert(
						new SqlBinaryExpression(typeof(int), CurrentSql.Select.TakeValue, "-", CurrentSql.Select.SkipValue, Precedence.Additive)));

				if (prevSkipValue != null)
					CurrentSql.Select.Skip(Convert(
						new SqlBinaryExpression(typeof(int), prevSkipValue, "+", CurrentSql.Select.SkipValue, Precedence.Additive)));
			}

			if (!_info.SqlProvider.TakeAcceptsParameter)
			{
				var p = CurrentSql.Select.SkipValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}

		#endregion

		#region Parse ElementAt

		bool ParseElementAt(QuerySource select, Expression value, bool orDefault)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseSkip(select, value);
			ParseTake(new SqlValue(1));

			_info.MakeElementOperator(orDefault ? ElementMethod.FirstOrDefault : ElementMethod.First);

			ParsingTracer.DecIndentLevel();
			return true;
		}

		bool ParseElementAt(QuerySource select, LambdaInfo value, bool orDefault)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseSkip(select, value);
			ParseTake(new SqlValue(1));

			_info.MakeElementOperator(orDefault ? ElementMethod.FirstOrDefault : ElementMethod.First);

			ParsingTracer.DecIndentLevel();
			return true;
		}

		#endregion

		#region ParseDistinct

		QuerySource ParseDistinct(QuerySource select)
		{
			ParsingTracer.WriteLine(select);
			ParsingTracer.IncIndentLevel();

			if (CurrentSql.Select.TakeValue != null || CurrentSql.Select.SkipValue != null)
				select = WrapInSubQuery(select);

			CurrentSql.Select.IsDistinct = true;
			//select.Select(this);

			ParsingTracer.DecIndentLevel();
			return select;
		}

		#endregion

		#region Parse Aggregate

		interface IAggregateHelper
		{
			void SetAggregate(ExpressionParser<T> parser, Expression pi);
		}

		class AggregateHelper<TE> : IAggregateHelper
		{
			public void SetAggregate(ExpressionParser<T> parser, Expression pi)
			{
				var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<TE>>(
					pi, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser.ExpressionParam,
						ParametersParam
					});

				parser._info.SetElementQuery(mapper.Compile());
			}
		}

		void ParseAggregate(MethodCallExpression expression, LambdaInfo lambda, QuerySource select)
		{
			ParsingTracer.WriteLine(expression);
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(select);
			ParsingTracer.IncIndentLevel();

			var query = select;

			if (query.SqlQuery.Select.IsDistinct || query.SqlQuery.Select.TakeValue != null || query.SqlQuery.Select.SkipValue != null)
			{
				query.Select(this);
				query = WrapInSubQuery(query);
			}
			else
			{
				foreach (var queryField in query.Fields)
					queryField.GetExpressions(this);
			}

			var name = expression.Method.Name;

			if (lambda == null && name != "Count" && query.Fields.Count != 1)
				throw new LinqException("Incorrent use of the '{0}' function.", name);

			if (CurrentSql.OrderBy.Items.Count > 0)
			{
				if (CurrentSql.Select.TakeValue == null && CurrentSql.Select.SkipValue == null)
					CurrentSql.OrderBy.Items.Clear();
				else
					query = WrapInSubQuery(query);
			}

			var sql = query.SqlQuery;
			var idx =
				name == "Count" ?
					sql.Select.Add(SqlFunction.CreateCount(expression.Type, sql), "cnt") :
					lambda != null ?
						sql.Select.Add(new SqlFunction(expression.Type, name, ParseExpression(lambda, lambda.Body, query))) :
						sql.Select.Add(new SqlFunction(expression.Type, name, query.Fields[0].GetExpressions(this)[0]));

			if (!_isSubQueryParsing)
				_buildSelect = () =>
				{
					var pi     = BuildField(expression, new[] { idx });
					var helper = (IAggregateHelper)Activator.CreateInstance(typeof(AggregateHelper<>).MakeGenericType(typeof(T), expression.Type));

					helper.SetAggregate(this, pi);
				};

			ParsingTracer.DecIndentLevel();
		}

		#endregion

		#region ParseElementOperator

		void ParseElementOperator(ElementMethod elementMethod)
		{
			var take = 0;

			if (!_isSubQueryParsing || _info.SqlProvider.IsSubQueryTakeSupported)
				switch (elementMethod)
				{
					case ElementMethod.First           :
					case ElementMethod.FirstOrDefault  :
						take = 1;
						break;

					case ElementMethod.Single          :
					case ElementMethod.SingleOrDefault :
						if (!_isSubQueryParsing)
							take = 2;
						break;
				}

			if (take != 0)
				ParseTake(new SqlValue(take));

			if (!_isSubQueryParsing)
				_info.MakeElementOperator(elementMethod);
		}

		#endregion

		#region ParseUnion

		QuerySource ParseUnion(QuerySource select, Expression ex, bool all)
		{
			var sql = CurrentSql;

			CurrentSql = new SqlQuery();

			var query = ParseSequence(ex);
			var union = new SqlQuery.Union(query[0].SqlQuery, all);

			CurrentSql = sql;

			var sq = select as QuerySource.SubQuery;

			if (sq == null || !sq.SubSql.HasUnion || !sql.IsSimple)
			{
				sq = WrapInSubQuery(select);
			}

			sq.SubSql.Unions.Add(union);
			sq.Unions.Add(query[0]);

			return sq;
		}

		#endregion

		#region ParseIntersect

		QuerySource ParseIntersect(QuerySource[] queries, Expression expr, bool isNot)
		{
			var select = WrapInSubQuery(queries[0]);
			var sql    = CurrentSql;

			CurrentSql = new SqlQuery();

			var query  = ParseSequence(expr)[0];
			var except = CurrentSql;

			except.ParentSql = sql;

			CurrentSql = sql;

			if (isNot)
				sql.Where.Not.Exists(except);
			else
				sql.Where.Exists(except);

			var keys1 = select.GetKeyFields(true);
			var keys2 = query. GetKeyFields(true);

			if (keys1 == null || keys1.Count == 0 || keys1.Count != keys2.Count)
				throw new InvalidOperationException();

			for (var i = 0; i < keys1.Count; i++)
			{
				except.Where
					.Expr(keys1[i].GetExpressions(this)[0])
					.Equal
					.Expr(keys2[i].GetExpressions(this)[0]);
			}

			return select;
		}

		#endregion

		#region ParseAny

		enum SetType { Any, All, In }

		QuerySource[] ParseAny(SetType setType, Expression expr, LambdaInfo lambda, Expression parentInfo)
		{
			var cond = ParseAnyCondition(setType, expr, lambda, null);

			if (_parsingMethod[0] == ParsingMethod.Select)
			{
				var sc = new SqlQuery.SearchCondition();
				sc.Conditions.Add(cond);

				var func   = Convert(new SqlFunction(typeof(bool), "CASE", sc, new SqlValue(true), new SqlValue(false)));
				var scalar = new QuerySource.Scalar(CurrentSql, new LambdaInfo(parentInfo), func, null);

				scalar.Select(this);

				_buildSelect = () =>
				{
					var pi = BuildField(parentInfo, new[] { 0 });

					var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<bool>>(
						pi, new[]
						{
							_infoParam,
							_contextParam,
							_dataReaderParam,
							_mapSchemaParam,
							ExpressionParam,
							ParametersParam
						});

						_info.SetElementQuery(mapper.Compile());
				};

				return new[] { scalar };
			}

			CurrentSql.Where.SearchCondition.Conditions.Add(cond);

			return new[] { ParentQueries[0].Parent };
		}

		SqlQuery.Condition ParseAnyCondition(SetType setType, Expression expr, LambdaInfo lambda, Expression inExpr)
		{
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(expr);
			ParsingTracer.IncIndentLevel();

			var sql = CurrentSql;
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
							if (parentQuery.Parent.Find(t.ParentAssociation))
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

			var query = ParseSequence(expr)[0];
			var any   = CurrentSql;

			_convertSource = cs;

			if (lambda != null)
			{
				if (setType == SetType.All)
				{
					var e  = Expression.Not(lambda.Body);
					//var pi = new NotParseInfo(e, lambda.Body);

					lambda = new LambdaInfo(e/*pi*/, lambda.Parameters);
				}

				if (inExpr == null || query.Fields.Count != 1)
					ParseWhere(lambda, query);
			}

			any.ParentSql = sql;
			CurrentSql    = sql;

			SqlQuery.Condition cond;

			if (inExpr != null && query.Fields.Count == 1)
			{
				query.Select(this);
				var ex = ParseExpression(inExpr);
				cond = new SqlQuery.Condition(false, new SqlQuery.Predicate.InSubQuery(ex, false, any));
			}
			else
				cond = new SqlQuery.Condition(setType == SetType.All, new SqlQuery.Predicate.FuncLike(SqlFunction.CreateExists(any)));

			ParsingTracer.DecIndentLevel();
			return cond;
		}

		#endregion

		#region Parse Contains

		QuerySource[] ParseContains(Expression sequence, Expression expr, MethodCallExpression parentInfo)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			var param  = Expression.Parameter(expr.Type, expr.NodeType == ExpressionType.Parameter ? ((ParameterExpression)expr).Name : "t");
			var lambda = new LambdaInfo(Expression.Equal(param, expr), param);
			var ret    = ParseAny(SetType.In, sequence, lambda, parentInfo);

			ParsingTracer.DecIndentLevel();
			return ret;
		}

		#endregion

		#region Parse Delete

		void ParseDelete(LambdaInfo lambda, QuerySource select)
		{
			if (lambda != null)
				ParseWhere(lambda, select);

			CurrentSql.QueryType = QueryType.Delete;

			_buildSelect = () => { _info.SetNonQueryQuery(); };
		}

		#endregion

		#region Parse Update

		void ParseUpdate(LambdaInfo predicate, LambdaInfo setter, QuerySource select)
		{
			if (predicate != null)
				select = ParseWhere(predicate, select);

			if (setter != null)
				ParseSetter(setter, select);

			CurrentSql.QueryType = QueryType.Update;

			_buildSelect = () => { _info.SetNonQueryQuery(); };
		}

		private void ParseSetter(LambdaInfo setter, QuerySource select)
		{
			if (setter.Body.NodeType != ExpressionType.MemberInit)
				throw new LinqException("Object initializer expected for update statement.");

			var body = (MemberInitExpression)setter.Body;
			var ex   = body;

			for (var i = 0; i < ex.Bindings.Count; i++)
			{
				var binding = ex.Bindings[i];
				var member  = binding.Member;

				if (member is MethodInfo)
					member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

				if (binding is MemberAssignment)
				{
					var ma     = binding as MemberAssignment;
					var column = select.GetField(member);
					var expr   = ParseExpression(setter, ma.Expression, select);

					if (expr is SqlParameter && ma.Expression.Type.IsEnum)
						SetParameterEnumConverter((SqlParameter)expr, ma.Expression.Type, _info.MappingSchema);

					CurrentSql.Set.Items.Add(new SqlQuery.SetExpression(column.GetExpressions(this)[0], expr));
				}
				else
					throw new InvalidOperationException();
			}
		}

		void ParseSet(LambdaInfo extract, LambdaInfo update, QuerySource select)
		{
			var pi = extract.Body;

			while (pi.NodeType == ExpressionType.Convert)
				pi = ((UnaryExpression)pi).Operand;

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for set statement.");

			var body   = (MemberExpression)pi;
			var member = body.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var column = CurrentSql.Set.Into != null ? CurrentSql.Set.Into.Fields[member.Name] : select.GetField(member).GetExpressions(this)[0];
			var expr   = ParseExpression(update, update.Body, select);

			if (expr is SqlParameter && update.Body.Type.IsEnum)
				SetParameterEnumConverter((SqlParameter)expr, update.Body.Type, _info.MappingSchema);

			CurrentSql.Set.Items.Add(new SqlQuery.SetExpression(column, expr));
		}

		void ParseSet(LambdaInfo extract, Expression update, QuerySource select)
		{
			var pi = extract.Body;

			if (!ExpressionHelper.IsConstant(update.Type) && !_asParameters.Contains(update))
				_asParameters.Add(update);

			while (pi.NodeType == ExpressionType.Convert)
				pi = ((UnaryExpression)pi).Operand;

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for set statement.");

			var body   = (MemberExpression)pi;
			var member = body.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var column = select.GetField(member);

			if (column == null)
				throw new LinqException("Member '{0}.{1}' is not a table column.", member.DeclaringType.Name, member.Name);

			var expr = ParseExpression(null, update, select);

			if (expr is SqlParameter && update.Type.IsEnum)
				SetParameterEnumConverter((SqlParameter)expr, update.Type, _info.MappingSchema);

			CurrentSql.Set.Items.Add(new SqlQuery.SetExpression(column.GetExpressions(this)[0], expr));
		}

		#endregion

		#region Parse Insert

		void ParseInsert(bool withIdentity, Expression into, LambdaInfo setter, QuerySource select)
		{
			if (into != null)
			{
				ParseSetter(setter, select);

				foreach (var item in CurrentSql.Set.Items)
					CurrentSql.Select.Expr(item.Expression);

				var sql = CurrentSql;

				CurrentSql = new SqlQuery();

				var source = ParseTable(into);

				CurrentSql = sql;
				CurrentSql.Set.Into  = ((QuerySource.Table)source).SqlTable;
			}
			else if (setter != null)
			{
				ParseSetter(setter, select);

				CurrentSql.Set.Into  = ((QuerySource.Table)select).SqlTable;
				CurrentSql.From.Tables.Clear();
			}
			else
			{
				foreach (var item in CurrentSql.Set.Items)
					CurrentSql.Select.Expr(item.Expression);
			}

			CurrentSql.QueryType        = QueryType.Insert;
			CurrentSql.Set.WithIdentity = withIdentity;

			if (withIdentity) _buildSelect = () => { _info.SetScalarQuery<object>(); };
			else              _buildSelect = () => { _info.SetNonQueryQuery(); };
		}

		QuerySource[] ParseInto(Expression source, Expression into)
		{
			QuerySource[] sequence;

			if (source.NodeType == ExpressionType.Constant && ((ConstantExpression)source).Value == null)
			{
				sequence = ParseSequence(into);
				CurrentSql.Set.Into = ((QuerySource.Table)sequence[0]).SqlTable;
				CurrentSql.From.Tables.Clear();
			}
			else
			{
				sequence = ParseSequence(source);

				var sql = CurrentSql;

				CurrentSql = new SqlQuery();

				var tbl = ParseSequence(into);

				CurrentSql = sql;

				CurrentSql.Set.Into = ((QuerySource.Table)tbl[0]).SqlTable;
			}

			CurrentSql.Select.Columns.Clear();

			return sequence;
		}

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

		#endregion
	}
}
