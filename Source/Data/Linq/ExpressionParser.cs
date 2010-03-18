using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq
{
	using IndexConverter = Func<FieldIndex,FieldIndex>;

	using Common;
	using Data.Sql;
	using DataProvider;
	using Mapping;
	using Reflection;

	#region ExpressionParser

	class ExpressionParser : ReflectionHelper
	{
		// Should be a single instance.
		//
		static protected readonly ParameterExpression ParametersParam = Expression.Parameter(typeof(object[]), "ps");


		static public Expression UnwrapLambda(Expression ex)
		{
			if (ex == null)
				return null;

			switch (ex.NodeType)
			{
				case ExpressionType.Convert        :
				case ExpressionType.ConvertChecked : return ((UnaryExpression)ex).Operand;
			}

			return ex;
		}
	}

	#endregion

	class ExpressionParser<T> : ExpressionParser
	{
		#region Init

		public ExpressionParser()
		{
			_info.Queries.Add(new ExpressionInfo<T>.QueryInfo());
		}

		readonly ExpressionInfo<T>   _info            = new ExpressionInfo<T>();
		readonly ParameterExpression _expressionParam = Expression.Parameter(typeof(Expression),        "expr");
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

		#region Parsing

		#region Parse

		public ExpressionInfo<T> Parse(
			DataProviderBase      dataProvider,
			MappingSchema         mappingSchema,
			Expression            expression,
			ParameterExpression[] parameters)
		{
			ParsingTracer.WriteLine(expression);
			ParsingTracer.IncIndentLevel();

			if (parameters != null)
				expression = ConvertParameters(expression, parameters);

			_info.DataProvider  = dataProvider;
			_info.MappingSchema = mappingSchema;
			_info.Expression    = expression;
			_info.Parameters    = parameters;

			ParseInfo.CreateRoot(expression, _expressionParam).Match(
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
						l    => BuildSimpleQuery(pi, typeof(T), l.Expr.Parameters[0].Name))),
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

		Expression ConvertParameters(Expression expression, ParameterExpression[] parameters)
		{
			return ParseInfo.CreateRoot(expression, _expressionParam).Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.Parameter)
				{
					var idx = Array.IndexOf(parameters, (ParameterExpression)pi.Expr);

					if (idx > 0)
						return pi.Parent.Replace(
							Expression.Convert(
								Expression.ArrayIndex(
									ParametersParam,
									Expression.Constant(Array.IndexOf(parameters, (ParameterExpression)pi.Expr))),
								pi.Expr.Type),
							pi.ParamAccessor);
				}

				return pi;
			});
		}

		int _sequenceNumber;

		QuerySource[] ParseSequence(ParseInfo info)
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

		QuerySource[] ParseSequenceInternal(ParseInfo info)
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
						var ma = (MemberExpression)info.Expr;

						if (IsListCountMember(ma.Member))
						{
							var pi = info.ConvertTo<MemberExpression>();
							var ex = pi.Create(ma.Expression, pi.Property(Member.Expression));
							var qs = ParseSequence(ex);

							CurrentSql.Select.Expr(SqlFunction.CreateCount(info.Expr.Type, CurrentSql.From.Tables[0]), "cnt");

							return qs;
						}

						var originalInfo = info;

						if (TypeHelper.IsSameOrParent(typeof(IQueryable), info.Expr.Type))
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
									if (_isSubQueryParsing && _parentQueries.Count > 0)
									{
										foreach (var parentQuery in _parentQueries)
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
									var pi = originalInfo.ConvertTo<MemberExpression>();
									var ex = pi.Create(ma.Expression, pi.Property(Member.Expression));
									var qs = ParseSequence(ex);

									qs[0].GetField(pi.Expr.Member).Select(this);

									return qs;
								}

							default:
								break;
						}
					}

					break;

				case ExpressionType.Parameter:
					if (_parentQueries.Count > 0)
						foreach (var query in _parentQueries)
							if (query.Parameter == info.Expr)
								return new[] { query.Parent };

					goto default;

				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
					{
						var pi = info.ConvertTo<UnaryExpression>();
						var ex = pi.Create(pi.Expr.Operand, pi.Property(Unary.Operand));
						return ParseSequence(ex);
					}

				default:
					throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", info.Expr), "info");
			}

			QuerySource[] sequence = null;

			info.ConvertTo<MethodCallExpression>().Match
			(
				//
				// db.Table.Method()
				//
				pi => pi.IsQueryableMethod(seq =>
				{
					ParsingTracer.WriteLine(pi);
					ParsingTracer.IncIndentLevel();

					switch (pi.Expr.Method.Name)
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
					switch (pi.Expr.Method.Name)
					{
						case "Select"            : sequence = ParseSequence(seq); sequence = ParseSelect    (l, sequence[0]);               break;
						case "Where"             : sequence = ParseSequence(seq); select   = ParseWhere     (l, sequence[0]);               break;
						case "SelectMany"        : sequence = ParseSequence(seq); select   = ParseSelectMany(l, null, sequence[0]);         break;
						case "OrderBy"           : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], false, true);  break;
						case "OrderByDescending" : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], false, false); break;
						case "ThenBy"            : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], true,  true);  break;
						case "ThenByDescending"  : sequence = ParseSequence(seq); select   = ParseOrderBy   (l, sequence[0], true,  false); break;
						case "GroupBy"           : sequence = ParseSequence(seq); select   = ParseGroupBy   (l, null, null, sequence[0], pi.Expr.Type.GetGenericArguments()[0]);   break;
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
				pi => pi.IsQueryableMethod("GroupBy",         1, 1, seq => sequence = ParseSequence(seq), (l1, l2)        => select   = ParseGroupBy   (l1, l2,   null, sequence[0], pi.Expr.Type.GetGenericArguments()[0])),
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
					ParseInfo into = null;

					return
						pi.IsMethod(null, "Insert", new Func<ParseInfo,bool>[]
						{
							p => { sequence = ParseSequence(p); return true; },
							p => { into = p; return true; },
							p => { p.IsLambda(1, l => ParseInsert(false, into, l, sequence[0]) ); return true; }
						}, _ => true)
						||
						pi.IsMethod(null, "InsertWithIdentity", new Func<ParseInfo,bool>[]
						{
							p => { sequence = ParseSequence(p); return true; },
							p => { into = p; return true; },
							p => { p.IsLambda(1, l => ParseInsert(true, into, l, sequence[0]) ); return true; }
						}, _ => true);
				},
				pi =>
				{
					ParseInfo arg1 = null;
					return pi.IsMethod(null, "Into", new Func<ParseInfo,bool>[]
					{
						p => { arg1 = p; return true; },
						p => { sequence = ParseInto(arg1, p); return true; }
					}, _ => true);
				},
				pi =>
				{
					ParseInfo s = null;
					return pi.IsQueryableMethod("Contains", seq => s = seq, ex => { sequence = ParseContains(s, ex, pi); return true; });
				},
				pi => pi.IsMethod(m =>
				{
					if (m.Expr.Method.DeclaringType == typeof(Queryable) || !TypeHelper.IsSameOrParent(typeof(IQueryable), pi.Expr.Type))
						return false;

					sequence = ParseSequence(GetIQueriable(info));
					return true;
				}),
				pi => { throw new ArgumentException(string.Format("Queryable method call expected. Got '{0}'.", pi.Expr), "info"); }
			);

			if (select   == null) return sequence;
			if (sequence == null) return new[] { select };

			return new[] { select }.Concat(sequence).ToArray();
		}

		QuerySource ParseTable(ParseInfo info)
		{
			if (info.NodeType == ExpressionType.MemberAccess)
			{
				if (_info.Parameters != null)
				{
					var me = (MemberExpression)info.Expr;

					if (me.Expression == _info.Parameters[0])
						return CreateTable(CurrentSql, new LambdaInfo(info));
				}
			}

			if (info.NodeType == ExpressionType.Call)
			{
				if (_info.Parameters != null)
				{
					var mc = (MethodCallExpression)info.Expr;

					if (mc.Object == _info.Parameters[0])
						return CreateTable(CurrentSql, new LambdaInfo(info));
				}
			}

			QuerySource select = null;

			if (info.IsConstant<IQueryable>((value,expr) =>
			{
				select = CreateTable(CurrentSql, new LambdaInfo(expr));
				return true;
			}))
			{}

			return select;
		}

		ParseInfo GetIQueriable(ParseInfo info)
		{
			if (info.NodeType == ExpressionType.MemberAccess || info.NodeType == ExpressionType.Call)
			{
				var p    = Expression.Parameter(typeof(Expression), "exp");
				var expr = ReplaceParameter(ParseInfo.CreateRoot(info.Expr, p), _ => {});
				var l    = Expression.Lambda<Func<Expression,IQueryable>>(Expression.Convert(expr, typeof(IQueryable)), new [] { p });
				var qe   = l.Compile();
				var n    = _info.AddQueryableAccessors(info.Expr, qe);

				return info.Create(
					qe(info).Expression,
					Expression.Call(
						_infoParam,
						Expressor<ExpressionInfo<T>>.MethodExpressor(a => a.GetIQueryable(0, null)),
						new [] { Expression.Constant(n), info.ParamAccessor ?? Expression.Constant(null) }));
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
				SetAlias(sources[i], l.Parameters[i].Expr.Name);

			switch (l.Body.NodeType)
			{
				case ExpressionType.Parameter  :
					for (var i = 0; i < sources.Length; i++)
						if (l.Body == l.Parameters[i].Expr)
							return sources[i];

					foreach (var query in _parentQueries)
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
							var pie = ConvertNew(l.Body.ConvertTo<NewExpression>());

							if (pie != null)
								return ParseSelect(new LambdaInfo(pie, l.Parameters), sources)[0];
						}

						return new QuerySource.Expr(CurrentSql, l.ConvertTo<NewExpression>(), Concat(sources, _parentQueries));
					}

				case ExpressionType.MemberInit :
					{
						CheckSubQueryForSelect(sources);
						return new QuerySource.Expr(CurrentSql, l.ConvertTo<MemberInitExpression>(), Concat(sources, _parentQueries));
					}

				default                        :
					{
						CheckSubQueryForSelect(sources);
						var scalar = new QuerySource.Scalar(CurrentSql, l, Concat(sources, _parentQueries));
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

			if (collectionSelector.Parameters[0].Expr == collectionSelector.Body.Expr)
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

			_parentQueries.Insert(0, new ParentQuery { Parent = source, Parameter = collectionSelector.Parameters[0] });
			var seq2 = ParseSequence(collectionSelector.Body);
			_parentQueries.RemoveAt(0);

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
				var call = (MethodCallExpression)collectionSelector.Body.Expr;

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

		static QuerySource.Table GetParentSource(QuerySource parent, QuerySource query)
		{
			if (parent is QuerySource.Table)
			{
				var tbl = (QuerySource.Table)parent;

				foreach (var at in tbl.AssociatedTables.Values)
					if (at == query)
						return tbl;
			}

			foreach (var source in parent.Sources)
			{
				var table = GetParentSource(source, query);
				if (table != null)
					return table;
			}

			return null;
		}

		#endregion

		#region Parse Join

		QuerySource ParseJoin(
			ParseInfo   inner,
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
				var new1 = outerKeySelector.Body.ConvertTo<NewExpression>();
				var new2 = innerKeySelector.Body.ConvertTo<NewExpression>();

				for (var i = 0; i < new1.Expr.Arguments.Count; i++)
				{
					var arg1 = new1.Create(new1.Expr.Arguments[i], new1.Index(new1.Expr.Arguments, New.Arguments, i));
					var arg2 = new2.Create(new2.Expr.Arguments[i], new2.Index(new2.Expr.Arguments, New.Arguments, i));

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
			ParseInfo   inner,
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
					outerKeySelector.Body.Expr.Type));

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
				var new1 = outerKeySelector.Body.ConvertTo<NewExpression>();
				var new2 = innerKeySelector.Body.ConvertTo<NewExpression>();

				for (var i = 0; i < new1.Expr.Arguments.Count; i++)
				{
					join
						.Expr(ParseExpression(outerKeySelector, new1.Create(new1.Expr.Arguments[i], new1.Index(new1.Expr.Arguments, New.Arguments, i)), source1)).Equal
						.Expr(ParseExpression(innerKeySelector, new2.Create(new2.Expr.Arguments[i], new2.Index(new2.Expr.Arguments, New.Arguments, i)), source2));

					counter.SubSql.Where
						.Expr(ParseExpression(outerKeySelector, new1.Create(new1.Expr.Arguments[i], new1.Index(new1.Expr.Arguments, New.Arguments, i)), source1)).Equal
						.Expr(ParseExpression(innerKeySelector, new2.Create(new2.Expr.Arguments[i], new2.Index(new2.Expr.Arguments, New.Arguments, i)), cntseq));
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

		QuerySource[] ParseDefaultIfEmpty(ParseInfo seq)
		{
			if (_parentQueries.Count > 0)
			{
				var groupJoin = _parentQueries[0].Parent.Sources.Where(s => s is QuerySource.GroupJoin).FirstOrDefault();

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

		QuerySource ParseOfType(ParseInfo pi, QuerySource[] sequence)
		{
			var table = sequence[0] as QuerySource.Table;

			if (table != null && table.InheritanceMapping.Count > 0)
			{
				var objectType = pi.Expr.Type.GetGenericArguments()[0];

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

			SetAlias(select, l.Parameters[0].Expr.Name);

			bool makeHaving;

			_parsingMethod.Insert(0, ParsingMethod.Where);
			_parentQueries.Insert(0, new ParentQuery { Parent = select, Parameter = l.Parameters[0] });

			if (CheckSubQueryForWhere(l, select, out makeHaving))
				select = WrapInSubQuery(select);

			ParseSearchCondition(
				makeHaving? CurrentSql.Having.SearchCondition.Conditions : CurrentSql.Where.SearchCondition.Conditions,
				l, l.Body, select);

			_parentQueries.RemoveAt(0);
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

			lambda.Body.Walk(pi =>
			{
				if (IsSubQuery(pi, query))
				{
					pi.StopWalking = isWhere = true;
					return pi;
				}

				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma      = (MemberExpression)pi.Expr;
							var isCount = IsListCountMember(ma.Member);

							if (!IsNullableValueMember(ma.Member) && !isCount)
							{
								if (ConvertMember(ma.Member) == null)
								{
									var field = GetField(lambda, pi, query);

									if (field is QueryField.ExprColumn)
										makeSubQuery = pi.StopWalking = true;
								}
							}

							if (isCount)
							{
								isHaving = true;
								pi.StopWalking = true;
							}
							else
								isWhere = true;

							break;
						}

					case ExpressionType.Call:
						{
							var e = pi.Expr as MethodCallExpression;

							if (e.Method.DeclaringType == typeof(Enumerable) && e.Method.Name != "Contains")
							{
								isHaving = true;
								pi.StopWalking = true;
							}
							else
								isWhere = true;

							break;
						}

					case ExpressionType.Parameter:
						if (checkParameter)
							makeSubQuery = pi.StopWalking = true;

						isWhere = true;

						break;
				}

				return pi;
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
					throw new LinqException("Cannot group by type '{0}'", keySelector.Body.Expr.Type);

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
					throw new LinqException("Cannot order by type '{0}'", lambda.Body.Expr.Type);

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

		bool ParseTake(QuerySource select, ParseInfo value)
		{
			if (value.Expr.Type != typeof(int))
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
			if (value.Body.Expr.Type != typeof(int))
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

		bool ParseSkip(QuerySource select, ParseInfo value)
		{
			if (value.Expr.Type != typeof(int))
				return false;

			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseSkip(CurrentSql.Select.SkipValue, ParseExpression(value, select));

			ParsingTracer.DecIndentLevel();
			return true;
		}

		bool ParseSkip(QuerySource select, LambdaInfo value)
		{
			if (value.Body.Expr.Type != typeof(int))
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

		bool ParseElementAt(QuerySource select, ParseInfo value, bool orDefault)
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
			void SetAggregate(ExpressionParser<T> parser, ParseInfo pi);
		}

		class AggregateHelper<TE> : IAggregateHelper
		{
			public void SetAggregate(ExpressionParser<T> parser, ParseInfo pi)
			{
				var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<TE>>(
					pi, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser._expressionParam,
						ParametersParam
					});

				parser._info.SetElementQuery(mapper.Compile());
			}
		}

		void ParseAggregate(ParseInfo<MethodCallExpression> parseInfo, LambdaInfo lambda, QuerySource select)
		{
			ParsingTracer.WriteLine(parseInfo);
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(select);
			ParsingTracer.IncIndentLevel();

			var query = select;

			if (query.SqlQuery.Select.IsDistinct)
			{
				query.Select(this);
				query = WrapInSubQuery(query);
			}
			else
			{
				foreach (var queryField in query.Fields)
					queryField.GetExpressions(this);
			}

			var name = parseInfo.Expr.Method.Name;

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
					sql.Select.Add(SqlFunction.CreateCount(parseInfo.Expr.Type, sql), "cnt") :
					lambda != null ?
						sql.Select.Add(new SqlFunction(parseInfo.Expr.Type, name, ParseExpression(lambda, lambda.Body, query))) :
						sql.Select.Add(new SqlFunction(parseInfo.Expr.Type, name, query.Fields[0].GetExpressions(this)[0]));

			if (!_isSubQueryParsing)
				_buildSelect = () =>
				{
					var pi     = BuildField(parseInfo, new[] { idx });
					var helper = (IAggregateHelper)Activator.CreateInstance(typeof(AggregateHelper<>).MakeGenericType(typeof(T), parseInfo.Expr.Type));

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

		QuerySource ParseUnion(QuerySource select, ParseInfo ex, bool all)
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

		QuerySource ParseIntersect(QuerySource[] queries, ParseInfo expr, bool isNot)
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

		QuerySource[] ParseAny(SetType setType, ParseInfo expr, LambdaInfo lambda, ParseInfo parentInfo)
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
							_expressionParam,
							ParametersParam
						});

						_info.SetElementQuery(mapper.Compile());
				};

				return new[] { scalar };
			}

			CurrentSql.Where.SearchCondition.Conditions.Add(cond);

			return new[] { _parentQueries[0].Parent };
		}

		class NotParseInfo : ParseInfo<UnaryExpression>
		{
			public NotParseInfo(UnaryExpression e, ParseInfo pi)
			{
				Expr          = e;
				Parent        = pi.Parent;
				ParamAccessor = pi.ParamAccessor;
				_parseInfo    = pi;
			}

			readonly ParseInfo _parseInfo;

			public override ParseInfo<TE> Create<TE>(TE expr, Expression paramAccesor)
			{
				return (ParseInfo<TE>)_parseInfo;
			}
		}

		SqlQuery.Condition ParseAnyCondition(SetType setType, ParseInfo expr, LambdaInfo lambda, ParseInfo inExpr)
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
					if (_parentQueries.Count > 0)
					{
						foreach (var parentQuery in _parentQueries)
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
					var pi = new NotParseInfo(e, lambda.Body);

					lambda = new LambdaInfo(pi, lambda.Parameters);
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

		QuerySource[] ParseContains(ParseInfo sequence, ParseInfo expr, ParseInfo<MethodCallExpression> parentInfo)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			var param  = Expression.Parameter(expr.Expr.Type, expr.NodeType == ExpressionType.Parameter ? ((ParameterExpression)expr).Name : "t");
			var lambda = new LambdaInfo(ParseInfo.CreateRoot(Expression.Equal(param, expr), null), ParseInfo.CreateRoot(param, null));
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

			var body = setter.Body.ConvertTo<MemberInitExpression>();
			var ex   = body.Expr;

			for (var i = 0; i < ex.Bindings.Count; i++)
			{
				var binding = ex.Bindings[i];
				var member  = binding.Member;

				if (member is MethodInfo)
					member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

				if (binding is MemberAssignment)
				{
					var ma = binding as MemberAssignment;

					var piBinding    = body.     Create(ma.Expression, body.Index(ex.Bindings, MemberInit.Bindings, i));
					var piAssign     = piBinding.Create(ma.Expression, piBinding.ConvertExpressionTo<MemberAssignment>());
					var piExpression = piAssign. Create(ma.Expression, piAssign.Property(MemberAssignmentBind.Expression));

					var column = select.GetField(member);
					var expr   = ParseExpression(setter, piExpression, select);

					if (expr is SqlParameter && piExpression.Expr.Type.IsEnum)
						SetParameterEnumConverter((SqlParameter)expr, piExpression.Expr.Type, _info.MappingSchema);

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
			{
				var ue = pi.ConvertTo<UnaryExpression>();
				pi = ue.Create(ue.Expr.Operand, ue.Property(Unary.Operand));
			}

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for set statement.");

			var body   = pi.ConvertTo<MemberExpression>();
			var member = body.Expr.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var column = CurrentSql.Set.Into != null ? CurrentSql.Set.Into.Fields[member.Name] : select.GetField(member).GetExpressions(this)[0];
			var expr   = ParseExpression(update, update.Body, select);

			if (expr is SqlParameter && update.Body.Expr.Type.IsEnum)
				SetParameterEnumConverter((SqlParameter)expr, update.Body.Expr.Type, _info.MappingSchema);

			CurrentSql.Set.Items.Add(new SqlQuery.SetExpression(column, expr));
		}

		void ParseSet(LambdaInfo extract, ParseInfo update, QuerySource select)
		{
			var pi = extract.Body;

			if (!IsConstant(update.Expr.Type) && !_asParameters.Contains(update.Expr))
				_asParameters.Add(update.Expr);

			while (pi.NodeType == ExpressionType.Convert)
			{
				var ue = pi.ConvertTo<UnaryExpression>();
				pi = ue.Create(ue.Expr.Operand, ue.Property(Unary.Operand));
			}

			if (pi.NodeType != ExpressionType.MemberAccess)
				throw new LinqException("Member expression expected for set statement.");

			var body   = pi.ConvertTo<MemberExpression>();
			var member = body.Expr.Member;

			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var column = select.GetField(member);

			if (column == null)
				throw new LinqException("Member '{0}.{1}' is not a table column.", member.DeclaringType.Name, member.Name);

			var expr = ParseExpression(null, update, select);

			if (expr is SqlParameter && update.Expr.Type.IsEnum)
				SetParameterEnumConverter((SqlParameter)expr, update.Expr.Type, _info.MappingSchema);

			CurrentSql.Set.Items.Add(new SqlQuery.SetExpression(column.GetExpressions(this)[0], expr));
		}

		#endregion

		#region Parse Insert

		void ParseInsert(bool withIdentity, ParseInfo into, LambdaInfo setter, QuerySource select)
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

		QuerySource[] ParseInto(ParseInfo source, ParseInfo into)
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

		void ParseValue(LambdaInfo extract, ParseInfo update, QuerySource select)
		{
			if (!IsConstant(update.Expr.Type) && !_asParameters.Contains(update.Expr))
				_asParameters.Add(update.Expr);

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

		#endregion

		#region SetQuery

		void SetQuery(ParseInfo info, QuerySource query, IndexConverter converter)
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
				SetQuery(BuildTable(info, table, null, converter));
			}
		}

		void SetQuery(ParseInfo info)
		{
			SetQuery(info == null ? null : info.Expr);
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
					expr, new[] { _infoParam, _contextParam, _dataReaderParam, _mapSchemaParam, _expressionParam, ParametersParam });

				_info.SetQuery(mapper.Compile());
			}
		}

		#endregion

		#region Build Select

		#region BuildSelect

		ParseInfo BuildSelect(QuerySource query, ParseInfo pi, IndexConverter converter)
		{
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			ParseInfo info = null;

			query.Match
			(
				table  => info = BuildTable   (table, pi ?? (table.Lambda != null ? table.Lambda.Body : null), null, converter), // QueryInfo.Table
				expr   => info = BuildNew     (query,       expr.  Lambda, expr.  Lambda.Body, converter), // QueryInfo.Expr
				sub    => info = BuildSubQuery(sub,   pi,                                      converter), // QueryInfo.SubQuery
				scalar => info = BuildNew     (query,       scalar.Lambda, scalar.Lambda.Body, converter), // QueryInfo.Scalar
				group  => info = BuildGroupBy (group,       group. Lambda, group. Lambda.Body),            // QueryInfo.GroupBy
				column => info = BuildNew     (query, column.Lambda, column.Lambda != null ? column.Lambda.Body : pi, converter) // QueryInfo.SubQuerySourceColumn
			);

			ParsingTracer.DecIndentLevel();

			return info;
		}

		ParseInfo BuildNew(QuerySource query, LambdaInfo lambda, ParseInfo expr, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var addParents = lambda != null && lambda.Parameters.Length > 1 && query.Sources.Length >= lambda.Parameters.Length;

			if (addParents)
				for (var i = 0; i < lambda.Parameters.Length; i++)
					_parentQueries.Insert(i, new ParentQuery { Parent = query.Sources[i], Parameter = lambda.Parameters[i] });

			var info = BuildNewExpression(lambda, query, expr, converter);

			if (addParents)
				_parentQueries.RemoveRange(0, lambda.Parameters.Length);

			ParsingTracer.DecIndentLevel();

			return info;
		}

		#endregion

		#region BuildSubQuerySource

		ParseInfo BuildSubQuery(QuerySource.SubQuery subQuery, ParseInfo pi, IndexConverter converter)
		{
			ParsingTracer.WriteLine();
			ParsingTracer.IncIndentLevel();

			ParseInfo info = null;

			subQuery.BaseQuery.Match
			(
				table  => info = BuildTable(                        // QueryInfo.Table
					table,
					pi,
					subQuery.CheckNullField,
					i => i.Field == subQuery.CheckNullField ? converter(i) : converter(subQuery.EnsureField(i.Field).Select(this)[0])),
				expr   =>                                           // QueryInfo.Expr
				{
					if (expr.Lambda.Body.NodeType == ExpressionType.New)
						info = BuildQuerySourceExpr(subQuery, expr.Lambda.Body, converter);
					else
						throw new NotImplementedException();
				}, 
				sub    =>                                           // QueryInfo.SubQuery
					{
						info = BuildSubQuery(sub, pi, i => converter(subQuery.EnsureField(i.Field).Select(this)[0]));
					},
				scalar =>                                           // QueryInfo.Scalar
				{
					var idx  = subQuery.Fields[0].Select(this);
					info = BuildField(scalar.Lambda.Body, idx.Select(i => converter(i).Index).ToArray());
				},
				_      => { throw new NotImplementedException(); }, // QueryInfo.GroupBy
				_      => { throw new NotImplementedException(); }  // QueryInfo.SubQuerySourceColumn
			);

			ParsingTracer.DecIndentLevel();

			return info;
		}

		ParseInfo BuildQuerySourceExpr(QuerySource query, ParseInfo parseInfo, IndexConverter converter)
		{
			ParsingTracer.WriteLine(parseInfo);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			NewExpression newExpr = null;
			var           member  = 0;

			var result = parseInfo.Walk(pi =>
			{
				if (newExpr == null && pi.NodeType == ExpressionType.New)
				{
					newExpr = (NewExpression)pi.Expr;
				}
				else if (newExpr != null)
				{
					var mi = newExpr.Members[member++];

					if (mi is MethodInfo)
						mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

					var field = query.GetField(mi);

					if (field is QuerySource.SubQuerySourceColumn)
						return BuildSubQuerySourceColumn(pi, (QuerySource.SubQuerySourceColumn)field, converter);

					var idx   = field.Select(this);

					return BuildField(pi, idx.Select(i => converter(i).Index).ToArray());
				}

				return pi;
			});

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region BuildGroupBy

		interface IGroupByHelper
		{
			ParseInfo GetParseInfo(ExpressionParser<T> parser, QuerySource.GroupBy query, ParseInfo expr, Expression info);
		}

		class GroupByHelper<TKey,TElement,TSource> : IGroupByHelper
		{
			public ParseInfo GetParseInfo(ExpressionParser<T> parser, QuerySource.GroupBy query, ParseInfo expr, Expression info)
			{
				var valueParser = new ExpressionParser<TElement>();
				var keyParam    = Expression.Convert(Expression.ArrayIndex(ParametersParam, Expression.Constant(0)), typeof(TKey));

				Expression valueExpr = null;

				if (expr.NodeType == ExpressionType.New)
				{
					var ne = (NewExpression)expr.Expr;

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
					var pexpr  = ((MemberExpression)expr.Expr).Expression;
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
					query.OriginalQuery.Lambda.Body.Expr,
					Expression.Lambda<Func<TSource,bool>>(valueExpr, new[] { query.Lambda.Parameters[0].Expr }));

				if (query.ElementSource != null)
				{
					valueExpr = Expression.Call(
						null,
						Expressor<object>.MethodExpressor(_ => Queryable.Select(null, (Expression<Func<TSource,TElement>>)null)),
						valueExpr,
						Expression.Lambda<Func<TSource,TElement>>(query.ElementSource.Lambda.Body, new[] { query.ElementSource.Lambda.Parameters[0].Expr }));
				}
// ReSharper restore AssignNullToNotNullAttribute

				var keyReader = Expression.Lambda<ExpressionInfo<T>.Mapper<TKey>>(
					info, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser._expressionParam,
						ParametersParam
					});

				return expr.Parent.Replace(
					Expression.Call(parser._infoParam, parser._info.GetGroupingMethodInfo<TKey,TElement>(),
						parser._contextParam,
						parser._dataReaderParam,
						parser._expressionParam,
						ParametersParam,
						Expression.Constant(keyReader.Compile()),
						Expression.Constant(valueParser.Parse(parser._info.DataProvider, parser._info.MappingSchema, valueExpr, parser._info.Parameters))),
					expr.ParamAccessor);
			}
		}

		ParseInfo BuildGroupBy(QuerySource.GroupBy query, LambdaInfo lambda, ParseInfo expr)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			ParseInfo info;

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

				info = ParseInfo.CreateRoot(
					Expression.Convert(
						Expression.Call(_infoParam, _info.GetMapperMethodInfo(),
							Expression.Constant(expr.Expr.Type),
							_dataReaderParam,
							Expression.Constant(_info.GetMapperSlot(index))),
						expr.Expr.Type),
					expr);
			}
			else if (query.IsWrapped && expr.NodeType != ExpressionType.New)
			{
				var idx = query.Fields[0].Select(this);

				if (idx.Length != 1)
					throw new InvalidOperationException();

				info = BuildField(expr, new[] { idx[0].Index });
			}
			else
				info = BuildNewExpression(lambda, query, expr, i => i);

			var args   = query.GroupingType.GetGenericArguments();
			var helper = (IGroupByHelper)Activator.CreateInstance(typeof(GroupByHelper<,,>).
				MakeGenericType(typeof(T), args[0], args[1], query.Lambda.Parameters[0].Expr.Type));

			info = helper.GetParseInfo(this, query, expr, info);

			ParsingTracer.DecIndentLevel();

			return info;
		}

		#endregion

		#region BuildNewExpression

		ParseInfo BuildNewExpression(LambdaInfo lambda, QuerySource query, ParseInfo expr, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var newExpr = expr.Walk(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							if (IsSubQuery      (        pi,       query)) return BuildSubQuery(pi, query, converter);
							if (IsServerSideOnly(lambda, pi, true, query)) return BuildField   (lambda, query.BaseQuery, pi);

							var ma = (ParseInfo<MemberExpression>)pi;
							var ex = pi.Create(ma.Expr.Expression, pi.Property(Member.Expression));

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

										pi = BuildNewExpression(lambda, col.QuerySource, col.Expr, converter);
										pi.IsReplaced = pi.StopWalking = true;

										return pi;
									}

									if (field is QuerySource.Table)
										return BuildTable(ma, (QuerySource.Table)field, null, converter);

									if (field is QueryField.SubQueryColumn)
										return BuildSubQuerySource(ma, (QueryField.SubQueryColumn)field, converter);

									if (field is QueryField.GroupByColumn)
									{
										var ret = BuildGroupBy(ma, (QueryField.GroupByColumn)field, converter);
										ret.StopWalking = true;
										return ret;
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

							if (ex.Expr != null && ex.NodeType == ExpressionType.Constant)
							{
								// field = localVariable
								//
								var c = ex.Parent.Create((ConstantExpression)ex.Expr, ex.Property<ConstantExpression>(Constant.Value));

								return pi.Parent.Replace(
									Expression.MakeMemberAccess(
										Expression.Convert(c.ParamAccessor, ex.Expr.Type),
										ma.Expr.Member),
									c.ParamAccessor);
							}

							break;
						}

					case ExpressionType.Parameter:
						{
							if (query.Lambda.MethodInfo         != null     &&
							    query.Lambda.MethodInfo.Name    == "Select" &&
							    query.Lambda.Parameters.Length  == 2        &&
							    query.Lambda.Parameters[1].Expr == pi.Expr)
							{
								return pi.Create(Expression.MakeMemberAccess(_contextParam, QueryCtx.Counter), pi.ParamAccessor);
							}

							if (pi.Expr == ParametersParam)
							{
								pi.StopWalking = true;
								return pi;
							}

							var field = query.GetBaseField(lambda, pi.Expr);

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
							if (IsConstant(pi.Expr.Type))
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
						if (pi.Expr.Type == typeof(string) && _info.MappingSchema.GetDefaultNullValue<string>() != null)
							return BuildField(lambda, query.BaseQuery, pi);
						goto case ExpressionType.Conditional;

					case ExpressionType.Conditional:
						if (CanBeTranslatedToSql(lambda, pi, query.BaseQuery))
							return BuildField(lambda, query.BaseQuery, pi);
						break;

					case ExpressionType.Call:
						{
							var ce = pi.ConvertTo<MethodCallExpression>();
							var cm = ConvertMethod(ce);

							if (cm != null)
							{
								if (ce.Expr.Method.GetCustomAttributes(typeof(MethodExpressionAttribute), true).Length != 0)
								{
									var ex = BuildNewExpression(lambda, query, cm, converter);
									ex.StopWalking = true;
									return ex;
								}
							}

							if (IsSubQuery      (        pi,       query)) return BuildSubQuery(pi, query, converter);
							if (IsServerSideOnly(lambda, pi, true, query)) return BuildField   (lambda, query.BaseQuery, pi);
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
							if (CanBeCompiled(lambda, pi, query.BaseQuery))
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

		ParseInfo BuildSubQuery(ParseInfo expr, QuerySource query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			_parentQueries.Insert(0, new ParentQuery { Parent = query.BaseQuery, Parameter = query.Lambda.Parameters[0]});
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
			_parentQueries.RemoveAt(0);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region BuildTable

		static object DefaultInheritanceMappingException(object value, Type type)
		{
			throw new LinqException("Inheritance mapping is not defined for discriminator value '{0}' in the '{1}' hierarchy.", value, type);
		}

		ParseInfo BuildTable(QuerySource.Table table, ParseInfo info, QueryField checkNullField, IndexConverter converter)
		{
			if (info == null /*table.InheritanceMapping.Count == 0*/)
			{
				CurrentSql.Select.Columns.Clear();

				var idx = table.Select(this);

				foreach (var i in idx)
					converter(i);

				return null;
			}

			return BuildTable(info, table, checkNullField, converter);
		}

		ParseInfo BuildTable(ParseInfo pi, QuerySource.Table table, QueryField checkNullField, IndexConverter converter)
		{
			ParsingTracer.WriteLine(pi);
			ParsingTracer.WriteLine(table);
			ParsingTracer.IncIndentLevel();

			var objectType = table.ObjectType;

			if (table.InheritanceMapping.Count > 0 && pi.Expr.Type.IsGenericType)
			{
				var types = pi.Expr.Type.GetGenericArguments();

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
					var dindex         = table.Columns[table.InheritanceDiscriminators[0]].Select(this)[0].Index;

					expr = Expression.Convert(
						Expression.Call(_infoParam, exceptionMethod,
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

			var field = pi.Parent == null ?
				pi.       Replace(expr, pi.ParamAccessor) :
				pi.Parent.Replace(expr, pi.ParamAccessor);

			ParsingTracer.DecIndentLevel();
			return field;
		}

		#endregion

		#region BuildSubQuerySource

		ParseInfo BuildSubQuerySource(ParseInfo ma, QuerySource.SubQuery query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			ParseInfo result = null;

			if (query is QuerySource.GroupJoin && TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Expr.Type))
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

					result = result.Replace(
						Expression.Condition(
							Expression.Call(_dataReaderParam, DataReader.IsDBNull, Expression.Constant(idx.Index)),
							Expression.Convert(
								Expression.Constant(_info.MappingSchema.GetNullValue(result.Expr.Type)),
								result.Expr.Type),
							result.Expr),
						result.ParamAccessor);
				}
			}

			if (result == null)
				throw new InvalidOperationException();

			ParsingTracer.DecIndentLevel();

			return result;
		}

		ParseInfo BuildSubQuerySource(ParseInfo ma, QueryField.SubQueryColumn query, IndexConverter converter)
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

		ParseInfo BuildSubQuerySourceColumn(ParseInfo info, QuerySource.SubQuerySourceColumn source, IndexConverter converter)
		{
			var pi = BuildSelect(
				source.SourceColumn,
				info,
				i => converter(source.QuerySource.EnsureField(i.Field).Select(this)[0]));

			//pi.StopWalking = true;

			return pi;
		}

		#endregion

		#region BuildGroupJoin

		interface IGroupJoinHelper
		{
			ParseInfo GetParseInfo(ExpressionParser<T> parser, ParseInfo ma, FieldIndex counterIndex, Expression info);
		}

		class GroupJoinHelper<TE> : IGroupJoinHelper
		{
			public ParseInfo GetParseInfo(ExpressionParser<T> parser, ParseInfo ma, FieldIndex counterIndex, Expression info)
			{
				var itemReader = Expression.Lambda<ExpressionInfo<T>.Mapper<TE>>(
					info, new[]
					{
						parser._infoParam,
						parser._contextParam,
						parser._dataReaderParam,
						parser._mapSchemaParam,
						parser._expressionParam,
						ParametersParam
					});

				return ma.Parent.Replace(
					Expression.Call(parser._infoParam, parser._info.GetGroupJoinEnumeratorMethodInfo<TE>(),
						parser._contextParam,
						parser._dataReaderParam,
						parser._expressionParam,
						ParametersParam,
						Expression.Constant(counterIndex.Index),
						Expression.Constant(itemReader.Compile())),
					ma.ParamAccessor);
			}
		}

		ParseInfo BuildGroupJoin(ParseInfo ma, QuerySource.GroupJoin query, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var args = ma.Expr.Type.GetGenericArguments();

			if (args.Length == 0)
				return BuildSubQuerySource(ma, query, converter);

			var expr = BuildSelect(query.BaseQuery, ma, i => converter(query.EnsureField(i.Field).Select(this)[0])).Expr;

			var helper       = (IGroupJoinHelper)Activator.CreateInstance(typeof(GroupJoinHelper<>).MakeGenericType(typeof(T), args[0]));
			var counterIndex = converter(query.Counter.Select(this)[0]);
			var result       = helper.GetParseInfo(this, ma, counterIndex, expr);

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#region BuildGroupBy

		ParseInfo BuildGroupBy(ParseInfo<MemberExpression> ma, QueryField.GroupByColumn field, IndexConverter converter)
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

		ParseInfo BuildField(LambdaInfo lambda, QuerySource query, ParseInfo pi)
		{
			ParsingTracer.WriteLine(pi);
			ParsingTracer.WriteLine(query);
			ParsingTracer.IncIndentLevel();

			var sqlex = ParseExpression(lambda, pi, query);
			var idx   = CurrentSql.Select.Add(sqlex);
			var field = BuildField(pi, new[] { idx });

			ParsingTracer.IncIndentLevel();
			return field;
		}

		ParseInfo BuildField(ParseInfo ma, QueryField field, IndexConverter converter)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.WriteLine(field);
			ParsingTracer.IncIndentLevel();

			ParseInfo result = null;

			if (field is QuerySource.SubQuery)
			{
				var query = (QuerySource.SubQuery)field;

				if (query is QuerySource.GroupJoin && TypeHelper.IsSameOrParent(typeof(IEnumerable), ma.Expr.Type))
					result = BuildGroupJoin(ma, (QuerySource.GroupJoin)query, converter);
				else if (query.BaseQuery is QuerySource.Table)
				{
					var table = (QuerySource.Table)query.BaseQuery;

					if (ma.Expr.Type == table.ObjectType)
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

		ParseInfo BuildField(ParseInfo ma, int[] idx)
		{
			ParsingTracer.WriteLine(ma);
			ParsingTracer.IncIndentLevel();

			if (idx.Length != 1)
				throw new InvalidOperationException();

			var type = ma.Expr.Type;

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

			var result = ma.Parent == null ? ma.Create(mapper, ma.ParamAccessor) : ma.Parent.Replace(mapper, ma.ParamAccessor);

			ParsingTracer.DecIndentLevel();
			return result;
		}

		#endregion

		#endregion

		#region BuildSimpleQuery

		bool BuildSimpleQuery(ParseInfo info, Type type, string alias)
		{
			var table = CreateTable(CurrentSql, type);

			table.SqlTable.Alias = alias;

			SetQuery(info, table, i => i);

			return true;
		}

		#endregion

		#region Build Scalar Select

		void BuildScalarSelect(ParseInfo parseInfo)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.New:
				case ExpressionType.MemberInit:
					var query = ParseSelect(new LambdaInfo(parseInfo))[0];

					query.Select(this);
					SetQuery(BuildNew(query, null, parseInfo, i => i));

					return;
			}

			var expr = ParseExpression(parseInfo);

			CurrentSql.Select.Expr(expr);

			var pi = BuildField(parseInfo, new[] { 0 });

			var mapper = Expression.Lambda<ExpressionInfo<T>.Mapper<T>>(
				pi, new [] { _infoParam, _contextParam, _dataReaderParam, _mapSchemaParam, _expressionParam, ParametersParam });

			_info.SetQuery(mapper.Compile());
		}

		#endregion

		#region Build Constant

		readonly Dictionary<Expression,SqlValue> _constants = new Dictionary<Expression,SqlValue>();

		SqlValue BuildConstant(ParseInfo expr)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr.Expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr,typeof(object)));

			var v = lambda.Compile()();

			if (v != null && v.GetType().IsEnum)
			{
				var attrs = v.GetType().GetCustomAttributes(typeof(SqlEnumAttribute), true);

				v = Map.EnumToValue(v, attrs.Length == 0);
			}

			value = new SqlValue(v);

			_constants.Add(expr.Expr, value);

			return value;
		}

		#endregion

		#region Build Parameter

		readonly Dictionary<Expression,ExpressionInfo<T>.Parameter> _parameters   = new Dictionary<Expression, ExpressionInfo<T>.Parameter>();
		readonly Dictionary<Expression,Expression>                  _accessors    = new Dictionary<Expression, Expression>();
		readonly HashSet<Expression>                                _asParameters = new HashSet<Expression>();

		ExpressionInfo<T>.Parameter BuildParameter(ParseInfo expr)
		{
			ExpressionInfo<T>.Parameter p;

			if (_parameters.TryGetValue(expr.Expr, out p))
				return p;

			string name = null;

			var newExpr = ReplaceParameter(expr, nm => name = nm);
			var mapper  = Expression.Lambda<Func<ExpressionInfo<T>,Expression,object[],object>>(
				Expression.Convert(newExpr, typeof(object)),
				new [] { _infoParam, _expressionParam, ParametersParam });

			p = new ExpressionInfo<T>.Parameter
			{
				Expression   = expr.Expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Expr.Type, name, null)
			};

			_parameters.Add(expr.Expr, p);
			CurrentSqlParameters.Add(p);

			return p;
		}

		ParseInfo ReplaceParameter(ParseInfo expr, Action<string> setName)
		{
			return expr.Walk(pi =>
			{
				if (pi.NodeType == ExpressionType.MemberAccess)
				{
					Expression accessor;

					if (_accessors.TryGetValue(pi.Expr, out accessor))
					{
						var ma = (MemberExpression)pi.Expr;
						setName(ma.Member.Name);

						if (pi.Parent != null)
							return pi.Parent.Replace(pi.Expr, accessor);
					}
				}

				pi.IsConstant(c =>
				{
					if (!TypeHelper.IsScalar(pi.Expr.Type) || _asParameters.Contains(c))
					{
						if (c.ParamAccessor != null)
						{
							var e = Expression.Convert(c.ParamAccessor, pi.Expr.Type);
							pi = pi.Parent.Replace(e, c.ParamAccessor);
						}

						if (pi.Parent.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)pi.Parent.Expr;
							setName(ma.Member.Name);
						}
					}

					return true;
				});

				return pi;
			});
		}

		#endregion

		#region Expression Parser

		#region ParseExpression

		public ISqlExpression ParseExpression(ParseInfo parseInfo, params QuerySource[] queries)
		{
			return ParseExpression(null, parseInfo, queries);
		}

		public ISqlExpression ParseExpression(LambdaInfo lambda, ParseInfo parseInfo, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(parseInfo);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			try
			{
				var qlen = queries.Length;

				if (parseInfo.NodeType == ExpressionType.Parameter && qlen == 1 && queries[0] is QuerySource.Scalar)
				{
					var ma = (QuerySource.Scalar)queries[0];
					return ParseExpression(ma.Lambda, ma.Lambda.Body, ma.Sources);
				}

				if (CanBeConstant(parseInfo))
					return BuildConstant(parseInfo);

				if (CanBeCompiled(lambda, parseInfo, queries))
					return BuildParameter(parseInfo).SqlParameter;

				if (IsSubQuery(parseInfo, queries))
					return ParseSubQuery(lambda, parseInfo, queries);

				switch (parseInfo.NodeType)
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
							ParseSearchCondition(condition.Conditions, null, parseInfo, queries);
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
							var pi = parseInfo.Convert<BinaryExpression>();
							var e  = parseInfo.Expr as BinaryExpression;
							var l  = ParseExpression(lambda, pi.Create(e.Left,  pi.Property(Binary.Left)),  queries);
							var r  = ParseExpression(lambda, pi.Create(e.Right, pi.Property(Binary.Right)), queries);
							var t  = e.Type;

							switch (parseInfo.NodeType)
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
							var pi = parseInfo.Convert<UnaryExpression>();
							var e  = parseInfo.Expr as UnaryExpression;
							var o  = ParseExpression(lambda, pi.Create(e.Operand,  pi.Property(Unary.Operand)),  queries);
							var t  = e.Type;

							switch (parseInfo.NodeType)
							{
								case ExpressionType.UnaryPlus     : return o;
								case ExpressionType.Negate        :
								case ExpressionType.NegateChecked : return Convert(new SqlBinaryExpression(t, new SqlValue(-1), "*", o, Precedence.Multiplicative));
							}

							break;
						}

					case ExpressionType.Convert:
						{
							var pi = parseInfo.Convert<UnaryExpression>();
							var e  = parseInfo.Expr as UnaryExpression;
							var o  = ParseExpression(lambda, pi.Create(e.Operand, pi.Property(Unary.Operand)), queries);

							if (e.Method == null && e.IsLifted)
								return o;

							if (e.Operand.Type.IsEnum && Enum.GetUnderlyingType(e.Operand.Type) == e.Type)
								return o;

							return Convert(new SqlFunction(e.Type, "$Convert$", SqlDataType.GetDataType(e.Type), SqlDataType.GetDataType(e.Operand.Type), o));
						}

					case ExpressionType.Conditional:
						{
							var pi = parseInfo.Convert<ConditionalExpression>();
							var e  = parseInfo.Expr as ConditionalExpression;
							var s  = ParseExpression(lambda, pi.Create(e.Test,    pi.Property(Conditional.Test)),    queries);
							var t  = ParseExpression(lambda, pi.Create(e.IfTrue,  pi.Property(Conditional.IfTrue)),  queries);
							var f  = ParseExpression(lambda, pi.Create(e.IfFalse, pi.Property(Conditional.IfFalse)), queries);

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
							var pi = parseInfo.ConvertTo<MemberExpression>();
							var ma = (MemberExpression)parseInfo.Expr;
							var l  = ConvertMember(ma.Member);

							if (l != null)
							{
								var ef  = UnwrapLambda(l.Body);
								var pie = parseInfo.Parent.Replace(ef, null).Walk(wpi =>
								{
									if (wpi.NodeType == ExpressionType.Parameter)
									{
										var expr = ma.Expression;

										if (expr.NodeType == ExpressionType.MemberAccess)
											if (!_accessors.ContainsKey(expr))
												_accessors.Add(expr, pi.Property(Member.Expression));

										return pi.Create(expr, null);
									}

									return wpi;
								});

								return ParseExpression(lambda, pie, queries);
							}

							var attr = GetFunctionAttribute(ma.Member);

							if (attr != null)
								return Convert(attr.GetExpression(ma.Member));

							if (IsNullableValueMember(ma.Member))
								return ParseExpression(lambda, pi.Create(ma.Expression, pi.Property(Member.Expression)), queries);

							if (IsListCountMember(ma.Member))
							{
								var src = GetSource(null, pi.Create(ma.Expression, pi.Property(Member.Expression)), queries);
								if (src != null)
									return SqlFunction.CreateCount(parseInfo.Expr.Type, src.SqlQuery);
							}

							goto case ExpressionType.Parameter;
						}

					case ExpressionType.Parameter:
						{
							var field = GetField(lambda, parseInfo.Expr, queries);

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
							var pi = parseInfo.ConvertTo<MethodCallExpression>();
							var e  = parseInfo.Expr as MethodCallExpression;

							if (e.Method.DeclaringType == typeof(Enumerable))
								return ParseEnumerable(lambda, pi, queries);

							var cm = ConvertMethod(pi);
							if (cm != null)
								return ParseExpression(lambda, cm, queries);

							var attr = GetFunctionAttribute(e.Method);

							if (attr != null)
							{
								var parms = new List<ISqlExpression>();

								if (e.Object != null)
									parms.Add(ParseExpression(lambda, pi.Create(e.Object, pi.Property(MethodCall.Object)), queries));

								for (var i = 0; i < e.Arguments.Count; i++)
									parms.Add(ParseExpression(lambda, pi.Create(e.Arguments[i], pi.Index(e.Arguments, MethodCall.Arguments, i)), queries));

								return Convert(attr.GetExpression(e.Method, parms.ToArray()));
							}

							break;
						}

					case ExpressionType.New:
						{
							var pie = ConvertNew(parseInfo.ConvertTo<NewExpression>());

							if (pie != null)
								return ParseExpression(lambda, pie, queries);

							break;
						}

					case ExpressionType.Invoke:
						{
							var pi = parseInfo.ConvertTo<InvocationExpression>();
							var ex = pi.Create(pi.Expr.Expression, pi.Property(Invocation.Expression));

							if (ex.NodeType == ExpressionType.Quote)
								ex = ex.Create(((UnaryExpression)ex.Expr).Operand, ex.Property(Unary.Operand));

							//if (ex.NodeType == ExpressionType.MemberAccess)
							//	return ParseExpression(lambda, ex, queries);

							if (ex.NodeType == ExpressionType.Lambda)
							{
								var l   = ex.ConvertTo<LambdaExpression>();
								var dic = new Dictionary<Expression,ParseInfo>();

								for (var i = 0; i < l.Expr.Parameters.Count; i++)
									dic.Add(
										l.Expr.Parameters[i],
										pi.Create(pi.Expr.Arguments[i], pi.Index(pi.Expr.Arguments, Invocation.Arguments, i)));


								var pie = l.Create(l.Expr.Body, l.Property(LambdaExpr.Body)).Walk(wpi =>
								{
									ParseInfo ppi;

									return dic.TryGetValue(wpi.Expr, out ppi) ? ppi : wpi;
								});

								return ParseExpression(lambda, pie, queries);
							}

							break;
						}
				}

				throw new LinqException("'{0}' cannot be converted to SQL.", parseInfo.Expr);
			}
			finally
			{
				ParsingTracer.DecIndentLevel();
			}
		}

		#endregion

		#region ParseEnumerable

		ISqlExpression ParseEnumerable(LambdaInfo lambda, ParseInfo<MethodCallExpression> pi, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(pi);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			QueryField field = queries.Length == 1 && queries[0] is QuerySource.GroupBy ? queries[0] : null;

			if (field == null)
				field = GetField(lambda, pi.Expr.Arguments[0], queries);

			while (field is QuerySource.SubQuerySourceColumn)
				field = ((QuerySource.SubQuerySourceColumn)field).SourceColumn;

			if (field is QuerySource.GroupJoin && pi.Expr.Method.Name == "Count")
			{
				var ex = ((QuerySource.GroupJoin)field).Counter.GetExpressions(this)[0];
				ParsingTracer.DecIndentLevel();
				return ex;
			}

			if (!(field is QuerySource.GroupBy))
				throw new LinqException("'{0}' cannot be converted to SQL.", pi.Expr);

			var groupBy = (QuerySource.GroupBy)field;
			var expr    = ParseEnumerable(pi, groupBy);

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

		ISqlExpression ParseEnumerable(ParseInfo<MethodCallExpression> pi, QuerySource.GroupBy query)
		{
			var groupBy = query.OriginalQuery;
			var expr    = pi.Expr;
			var args    = new ISqlExpression[expr.Arguments.Count - 1];

			if (expr.Method.Name == "Count")
			{
				if (args.Length > 0)
				{
					var predicate = ParsePredicate(null, ParseLambdaArgument(pi, 1), groupBy);

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
					args[i - 1] = ParseExpression(ParseLambdaArgument(pi, i), groupBy);
			else
			{
				if (expr.Arguments[0].NodeType == ExpressionType.Call)
				{
					ParseInfo arg = pi.Create(expr.Arguments[0], pi.Index(expr.Arguments, MethodCall.Arguments, 0));

					if (arg.NodeType == ExpressionType.Call)
					{
						var call = arg.ConvertTo<MethodCallExpression>();

						if (call.Expr.Method.Name == "Select" && call.IsQueryableMethod((seq,l) =>
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

		static ParseInfo ParseLambdaArgument(ParseInfo pi, int idx)
		{
			var       expr = (MethodCallExpression)pi.Expr;
			ParseInfo arg  = pi.Create(expr.Arguments[idx], pi.Index(expr.Arguments, MethodCall.Arguments, idx));
			
			arg.IsLambda<Expression>(new Func<ParseInfo<ParameterExpression>,bool>[]
				{ _ => true },
				body => { arg = body; return true; },
				_ => true);

			return arg;
		}

		#endregion

		#region ParseSubQuery

		ISqlExpression ParseSubQuery(LambdaInfo lambda, ParseInfo expr, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(expr);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			var parentQueries = queries.Select(q => new ParentQuery { Parent = q, Parameter = q.Lambda.Parameters.FirstOrDefault() }).ToList();

			if (lambda != null && queries.Length > 0)
				parentQueries.Add(new ParentQuery { Parent = queries[0], Parameter = lambda.Parameters.FirstOrDefault() });

			_parentQueries.InsertRange(0, parentQueries);
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
			_parentQueries.RemoveRange(0, parentQueries.Count);

			ParsingTracer.DecIndentLevel();

			return result;
		}

		#endregion

		#region IsSubQuery

		bool IsSubQuery(ParseInfo parseInfo, params QuerySource[] queries)
		{
			return IsSubQuery(parseInfo, false, queries);
		}

		bool IsSubQuery(ParseInfo parseInfo, bool ignoreMembers, params QuerySource[] queries)
		{
			if (queries.Length > 0 && queries[0] is QuerySource.SubQuerySourceColumn)
				return false;

			switch (parseInfo.NodeType)
			{
				case ExpressionType.Call:
					{
						var call = parseInfo.Expr as MethodCallExpression;

						if (call.Method.DeclaringType == typeof(Queryable) || call.Method.DeclaringType == typeof(Enumerable))
						{
							var pi  = parseInfo.ConvertTo<MethodCallExpression>();
							var arg = call.Arguments[0];

							if (arg.NodeType == ExpressionType.Call)
								return IsSubQuery(pi.Create(arg, pi.Index(call.Arguments, MethodCall.Arguments, 0)), queries);

							var qs = queries;

							if (queries.Length > 0 && queries[0].GetType() == typeof(QuerySource.Expr))
								qs = new[] { queries[0].BaseQuery }.Concat(queries).ToArray();

							if (IsSubQuerySource(arg, qs))
								return true;
						}

						if (IsIEnumerableType(parseInfo.Expr))
							return !CanBeCompiled(null, parseInfo, queries);
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)parseInfo.Expr;

						if (IsSubQueryMember(parseInfo.Expr) && IsSubQuerySource(ma.Expression, queries))
							return !CanBeCompiled(null, parseInfo, queries);

						if (!ignoreMembers && IsIEnumerableType(parseInfo.Expr))
							return !CanBeCompiled(null, parseInfo, queries);

						if (ma.Expression != null)
						{
							var pi = parseInfo.ConvertTo<MemberExpression>();
							return IsSubQuery(pi.Create(ma.Expression, pi.Property(Member.Expression)), true, queries);
						}
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
				&& (type != typeof(byte[]))
				&& TypeHelper.IsSameOrParent(typeof(IEnumerable), type);

			if (res && expr.NodeType == ExpressionType.MemberAccess)
				res = TypeHelper.GetAttributes(type, typeof(IgnoreIEnumerableAttribute)).Length == 0;

			return res;
		}

		#endregion

		#region IsServerSideOnly

		bool IsServerSideOnly(LambdaInfo lambda, ParseInfo parseInfo, bool preferServer, params QuerySource[] queries)
		{
			switch (parseInfo.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var pi = parseInfo.ConvertTo<MemberExpression>();
						var l  = ConvertMember(pi.Expr.Member);

						if (l != null)
						{
							var ef   = UnwrapLambda(l.Body);
							var info = pi.Parent.Replace(ef, null) as ParseInfo;

							return IsServerSideOnly(lambda, info, preferServer, queries);


							//if (preferServer && l.Parameters.Count == 1 && pi.Expr.Expression != null)
							//{
							//	info = info.Walk(wpi =>
							//	{
							//		if (wpi == l.Parameters[0])
							//			return pi.Create(pi.Expr.Expression, null);

							//		return wpi;
							//	});
							//}

							/*
							var ret  = false;

							info.Walk(lpi =>
							{
								if (IsServerSideOnly(lambda, lpi, preferServer, queries))
									lpi.StopWalking = ret = true;
								return lpi;
							});

							return ret;
							*/
						}

						var attr = GetFunctionAttribute(pi.Expr.Member);

						if (attr != null)
						{
							if (attr.ServerSideOnly)
								return true;

							//return
							//	preferServer &&
							//	attr.PreferServerSide &&
							//	CanBeTranslatedToSql(lambda, parseInfo, queries) &&
							//	!CanBeCompiled(lambda, parseInfo, queries);
						}

						break;
					}

				case ExpressionType.Call:
					{
						var pi = parseInfo.ConvertTo<MethodCallExpression>();
						var e  = pi.Expr;

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
							{
								var info = pi.Parent.Replace(UnwrapLambda(l.Body), null);
								var ret  = false;

								info.Walk(lpi =>
								{
									if (IsServerSideOnly(lambda, lpi, preferServer, queries))
										lpi.StopWalking = ret = true;
									return lpi;
								});

								return ret;
							}

							var attr = GetFunctionAttribute(e.Method);

							if (attr != null)
							{
								if (attr.ServerSideOnly)
									return true;

								//return
								//	preferServer &&
								//	attr.PreferServerSide &&
								//	CanBeTranslatedToSql(lambda, parseInfo, queries) &&
								//	!CanBeCompiled(lambda, parseInfo, queries);
							}
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

		bool CanBeConstant(ParseInfo expr)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				var ex = pi.Expr;

				if (ex is BinaryExpression || ex is UnaryExpression || ex.NodeType == ExpressionType.Convert)
					return pi;

				switch (ex.NodeType)
				{
					case ExpressionType.Constant:
						{
							var c = (ConstantExpression)ex;

							if (c.Value == null || IsConstant(ex.Type))
								return pi;

							break;
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)ex;

							if (IsConstant(ma.Member.DeclaringType))
								return pi;

							break;
						}

					case ExpressionType.Call:
						{
							var mc = (MethodCallExpression)ex;

							if (IsConstant(mc.Method.DeclaringType) || mc.Method.DeclaringType == typeof(object))
								return pi;

							var attr = GetFunctionAttribute(mc.Method);

							if (attr != null && !attr.ServerSideOnly)
								return pi;

							break;
						}
				}

				canbe = false;
				pi.StopWalking = true;

				return pi;
			});

			return canbe;
		}

		#endregion

		#region CanBeCompiled

		bool CanBeCompiled(LambdaInfo lambda, ParseInfo expr, params QuerySource[] queries)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				if (canbe)
				{
					canbe = !IsServerSideOnly(lambda, pi, false, queries);

					if (canbe) switch (pi.NodeType)
					{
						case ExpressionType.Parameter:
							{
								var p = (ParameterExpression)pi.Expr;

								canbe = p == ParametersParam;
								break;
							}

						case ExpressionType.MemberAccess:
							{
								var ma   = (MemberExpression)pi.Expr;
								var attr = GetFunctionAttribute(ma.Member);

								canbe = attr == null || !attr.ServerSideOnly;
								break;
							}

						case ExpressionType.Call:
							{
								var mc   = (MethodCallExpression)pi.Expr;
								var attr = GetFunctionAttribute(mc.Method);

								canbe = attr == null || !attr.ServerSideOnly;
								break;
							}
					}
				}

				pi.StopWalking = !canbe;

				return pi;
			});

			return canbe;
		}

		#endregion

		#region CanBeTranslatedToSql

		bool CanBeTranslatedToSql(LambdaInfo lambda, ParseInfo expr, params QuerySource[] queries)
		{
			var canbe = true;

			expr.Walk(pi =>
			{
				if (canbe)
				{
					switch (pi.NodeType)
					{
						case ExpressionType.MemberAccess:
							{
								var ma = (MemberExpression)pi.Expr;
								var l  = ConvertMember(ma.Member);

								if (l != null)
								{
									canbe = CanBeTranslatedToSql(lambda, pi.Parent.Replace(UnwrapLambda(l.Body), null), queries);
									pi.StopWalking = true;
								}
								else
								{
									var attr = GetFunctionAttribute(ma.Member);

									if (attr == null && !IsNullableValueMember(ma.Member))
									{
										if (IsListCountMember(ma.Member))
										{
											var me = pi.ConvertTo<MemberExpression>();

											var src = GetSource(null, me.Create(ma.Expression, me.Property(Member.Expression)), queries);
											if (src == null)
												goto case ExpressionType.Parameter;
										}
										else
											goto case ExpressionType.Parameter;
									}
								}

								break;
							}

						case ExpressionType.Parameter:
							{
								var field = GetField(lambda, pi.Expr, queries);

								if (field == null)
									canbe = CanBeCompiled(lambda, pi, queries);

								pi.StopWalking = true;

								break;
							}

						case ExpressionType.Call:
							{
								var mc = pi.ConvertTo<MethodCallExpression>();
								var e  = pi.Expr as MethodCallExpression;

								if (e.Method.DeclaringType != typeof(Enumerable))
								{
									var cm = ConvertMethod(mc);

									if (cm != null)
									{
										canbe = CanBeTranslatedToSql(lambda, cm, queries);
										pi.StopWalking = true;
									}
									else
									{
										var attr = GetFunctionAttribute(e.Method);

										if (attr == null)
											canbe = CanBeCompiled(lambda, pi, queries);
									}
								}

								break;
							}
					}
				}

				if (!pi.StopWalking)
					pi.StopWalking = !canbe;

				return pi;
			});

			return canbe;
		}

		#endregion

		#region IsConstant

		public static bool IsConstant(Type type)
		{
			if (type.IsEnum)
				return true;

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Int16   :
				case TypeCode.Int32   :
				case TypeCode.Int64   :
				case TypeCode.UInt16  :
				case TypeCode.UInt32  :
				case TypeCode.UInt64  :
				case TypeCode.SByte   :
				case TypeCode.Byte    :
				case TypeCode.Decimal :
				case TypeCode.Double  :
				case TypeCode.Single  :
				case TypeCode.Boolean :
				case TypeCode.String  :
				case TypeCode.Char    : return true;
				default               : return false;
			}
		}

		#endregion

		#endregion

		#region Predicate Parser

		ISqlPredicate ParsePredicate(LambdaInfo lambda, ParseInfo parseInfo, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(parseInfo);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			try
			{
				switch (parseInfo.NodeType)
				{
					case ExpressionType.Equal :
					case ExpressionType.NotEqual :
					case ExpressionType.GreaterThan :
					case ExpressionType.GreaterThanOrEqual :
					case ExpressionType.LessThan :
					case ExpressionType.LessThanOrEqual :
						{
							var pi = parseInfo.ConvertTo<BinaryExpression>();
							var e  = parseInfo.Expr as BinaryExpression;
							var el = pi.Create(e.Left,  pi.Property(Binary.Left));
							var er = pi.Create(e.Right, pi.Property(Binary.Right));

							return ParseCompare(lambda, parseInfo.NodeType, el, er, queries);
						}

					case ExpressionType.Call:
						{
							var pi = parseInfo.Convert<MethodCallExpression>();
							var e  = pi.Expr as MethodCallExpression;

							ISqlPredicate predicate = null;

							if (e.Method.Name == "Equals" && e.Object != null && e.Arguments.Count == 1)
							{
								var el = pi.Create(e.Object,       pi.Property(MethodCall.Object));
								var er = pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0));

								return ParseCompare(lambda, ExpressionType.Equal, el, er, queries);
							}
							else if (e.Method.DeclaringType == typeof(string))
							{
								switch (e.Method.Name)
								{
									case "Contains"   : predicate = ParseLikePredicate(pi, "%", "%", queries); break;
									case "StartsWith" : predicate = ParseLikePredicate(pi, "",  "%", queries); break;
									case "EndsWith"   : predicate = ParseLikePredicate(pi, "%",  "", queries); break;
								}
							}
							else if (e.Method.DeclaringType == typeof(Enumerable))
							{
								switch (e.Method.Name)
								{
									case "Contains" : predicate = ParseInPredicate(pi, queries); break;
								}
							}
							else if (TypeHelper.IsSameOrParent(typeof(IList), e.Method.DeclaringType))
							{
								switch (e.Method.Name)
								{
									case "Contains" : predicate = ParseInPredicate(pi, queries); break;
								}
							}
							else if (e.Method == Functions.String.Like11) predicate = ParseLikePredicate(pi, queries);
							else if (e.Method == Functions.String.Like12) predicate = ParseLikePredicate(pi, queries);
							else if (e.Method == Functions.String.Like21) predicate = ParseLikePredicate(pi, queries);
							else if (e.Method == Functions.String.Like22) predicate = ParseLikePredicate(pi, queries);

							if (predicate != null)
								return Convert(predicate);

							break;
						}

					case ExpressionType.Conditional:
						return Convert(new SqlQuery.Predicate.ExprExpr(
							ParseExpression(lambda, parseInfo, queries),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(true)));

					case ExpressionType.MemberAccess:
						{
							var pi = parseInfo.Convert<MemberExpression>();
							var e  = pi.Expr as MemberExpression;

							if (e.Member.Name == "HasValue" && 
								e.Member.DeclaringType.IsGenericType && 
								e.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
							{
								var expr = ParseExpression(lambda, pi.Create(e.Expression, pi.Property(Member.Expression)), queries);
								return Convert(new SqlQuery.Predicate.IsNull(expr, true));
							}

							break;
						}

					case ExpressionType.TypeIs:
						{
							var pi = parseInfo.Convert<TypeBinaryExpression>();
							var e  = pi.Expr as TypeBinaryExpression;

							var table = GetSource(lambda, e.Expression, queries) as QuerySource.Table;

							if (table != null && table.InheritanceMapping.Count > 0)
								return MakeIsPredicate(table, e.TypeOperand);

							break;
						}
				}

				var ex = ParseExpression(lambda, parseInfo, queries);

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

		ISqlPredicate ParseCompare(LambdaInfo lambda, ExpressionType nodeType, ParseInfo left, ParseInfo right, QuerySource[] queries)
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

		ISqlPredicate ParseEnumConversion(ParseInfo left, SqlQuery.Predicate.Operator op, ParseInfo right, QuerySource[] queries)
		{
			ParseInfo<UnaryExpression> conv;
			ParseInfo                  value;

			if (left.NodeType == ExpressionType.Convert)
			{
				conv  = left.ConvertTo<UnaryExpression>();
				value = right;
			}
			else
			{
				conv  = right.ConvertTo<UnaryExpression>();
				value = left;
			}

			var operand = conv.Create(conv.Expr.Operand, conv.Property(Unary.Operand));
			var type    = operand.Expr.Type;

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
						value = value.ConvertTo<UnaryExpression>();
						value = value.Create(((UnaryExpression)value.Expr).Operand, value.Property(Unary.Operand));

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

		ISqlPredicate ParseObjectNullComparison(LambdaInfo lambda, ParseInfo left, ParseInfo right, QuerySource[] queries, bool isEqual)
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
			LambdaInfo leftLambda,  ParseInfo left,  QuerySource[] leftQueries,
			LambdaInfo rightLambda, ParseInfo right, QuerySource[] rightQueries)
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

			var isNull = right.Expr is ConstantExpression && ((ConstantExpression)right.Expr).Value == null;
			var cols   = sl.GetKeyFields(true);

			var condition = new SqlQuery.SearchCondition();
			var ta        = TypeAccessor.GetAccessor(right.Expr.Type != typeof(object) ? right.Expr.Type : left.Expr.Type);

			foreach (QueryField.Column col in cols)
			{
				var mi = ta[col.Field.Name].MemberInfo;

				QueryField rcol = null;

				if (sr != null)
				{
					rcol = GetField(rightLambda, Expression.MakeMemberAccess(right.Expr, mi), rightQueries);

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

					//rcol.Select(this);
				}

				var lcol = GetField(leftLambda, Expression.MakeMemberAccess(left.Expr, mi), leftQueries);

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

					//lcol.Select(this);
				}

				var rex =
					isNull ?
						new SqlValue(right.Expr.Type, null) :
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

		ISqlPredicate ParseNewObjectComparison(ExpressionType nodeType, ParseInfo left, ParseInfo right, params QuerySource[] queries)
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

			var newRight = right.Expr as NewExpression;
			var newExpr  = (NewExpression)left.Expr;

			if (newExpr.Members == null)
				return null;

			for (var i = 0; i < newExpr.Arguments.Count; i++)
			{
				var lex = ParseExpression(left.Create(newExpr.Arguments[i], left.Index(newExpr.Arguments, New.Arguments, i)), queries);
				var rex =
					right.NodeType == ExpressionType.New ?
						ParseExpression(right.Create(newRight.Arguments[i], right.Index(newRight.Arguments, New.Arguments, i)), queries) :
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

		ISqlExpression GetParameter(ParseInfo pi, MemberInfo member)
		{
			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var par    = ReplaceParameter(pi, _ => {});
			var expr   = Expression.MakeMemberAccess(par, member);
			var mapper = Expression.Lambda<Func<ExpressionInfo<T>,Expression,object[],object>>(
				Expression.Convert(expr, typeof(object)),
				new [] { _infoParam, _expressionParam, ParametersParam });

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

		static ParseInfo ConvertExpression(ParseInfo expr)
		{
			ParseInfo ret = null;

			expr.Walk(pi =>
			{
				if (ret == null) switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
					case ExpressionType.New:
						ret = pi;
						pi.StopWalking = true;
						break;
				}

				return pi;
			});

			if (ret == null)
				throw new NotImplementedException();

			return ret;
		}

		#endregion

		#region ParseInPredicate

		private ISqlPredicate ParseInPredicate(ParseInfo pi, params QuerySource[] queries)
		{
			var e = pi.Expr as MethodCallExpression;

			var argIndex = e.Object != null ? 0 : 1;

			var expr = ParseExpression(pi.Create(e.Arguments[argIndex], pi.Index(e.Arguments, MethodCall.Arguments, argIndex)), queries);
			var arr  = 
				e.Object != null ?
					pi.Create(e.Object,       pi.Property(MethodCall.Object)) :
					pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0));

			switch (arr.NodeType)
			{
				case ExpressionType.NewArrayInit:
					{
						var newArr = arr.ConvertTo<NewArrayExpression>();

						if (newArr.Expr.Expressions.Count == 0)
							return new SqlQuery.Predicate.Expr(new SqlValue(false));

						var exprs  = new ISqlExpression[newArr.Expr.Expressions.Count];

						for (var i = 0; i < newArr.Expr.Expressions.Count; i++)
						{
							var item = ParseExpression(
								newArr.Create(newArr.Expr.Expressions[i], newArr.Index(newArr.Expr.Expressions, NewArray.Expressions, i)),
								queries);

							exprs[i] = item;
						}

						return new SqlQuery.Predicate.InList(expr, false, exprs);
					}

				default:
					if (CanBeCompiled(null, arr, queries))
					{
						var p = BuildParameter(arr).SqlParameter;
						p.IsQueryParameter = false;
						return new SqlQuery.Predicate.InList(expr, false, p);
					}

					break;
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", pi.Expr);
		}

		#endregion

		#region LIKE predicate

		private ISqlPredicate ParseLikePredicate(ParseInfo pi, string start, string end, params QuerySource[] queries)
		{
			var e  = pi.Expr as MethodCallExpression;

			var o = ParseExpression(pi.Create(e.Object,       pi.Property(MethodCall.Object)),                 queries);
			var a = ParseExpression(pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0)), queries);

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

		private ISqlPredicate ParseLikePredicate(ParseInfo pi, params QuerySource[] queries)
		{
			var e  = pi.Expr as MethodCallExpression;
			var a1 = ParseExpression(pi.Create(e.Arguments[0], pi.Index(e.Arguments, MethodCall.Arguments, 0)), queries);
			var a2 = ParseExpression(pi.Create(e.Arguments[1], pi.Index(e.Arguments, MethodCall.Arguments, 1)), queries);

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ParseExpression(pi.Create(e.Arguments[2], pi.Index(e.Arguments, MethodCall.Arguments, 2)), queries);

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

		void ParseSearchCondition(ICollection<SqlQuery.Condition> conditions, LambdaInfo lambda, ParseInfo parseInfo, params QuerySource[] queries)
		{
			ParsingTracer.WriteLine(lambda);
			ParsingTracer.WriteLine(parseInfo);
			ParsingTracer.WriteLine(queries);
			ParsingTracer.IncIndentLevel();

			if (IsSubQuery(parseInfo, queries))
			{
				var cond = ParseConditionSubQuery(parseInfo, queries);

				if (cond != null)
				{
					conditions.Add(cond);
					ParsingTracer.DecIndentLevel();
					return;
				}
			}

			switch (parseInfo.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						ParseSearchCondition(conditions, lambda, pi.Create(e.Left,  pi.Property(Binary.Left)),  queries);
						ParseSearchCondition(conditions, lambda, pi.Create(e.Right, pi.Property(Binary.Right)), queries);

						break;
					}

				case ExpressionType.OrElse:
					{
						var pi = parseInfo.Convert<BinaryExpression>();
						var e  = parseInfo.Expr as BinaryExpression;

						var orCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(orCondition.Conditions, lambda, pi.Create(e.Left,  pi.Property(Binary.Left)),  queries);
						orCondition.Conditions[orCondition.Conditions.Count - 1].IsOr = true;
						ParseSearchCondition(orCondition.Conditions, lambda, pi.Create(e.Right, pi.Property(Binary.Right)), queries);

						conditions.Add(new SqlQuery.Condition(false, orCondition));

						break;
					}

				case ExpressionType.Not:
					{
						var pi = parseInfo.Convert<UnaryExpression>();
						var e  = parseInfo.Expr as UnaryExpression;

						var notCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(notCondition.Conditions, lambda, parseInfo.Create(e.Operand, pi.Property(Unary.Operand)), queries);

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
					var predicate = ParsePredicate(lambda, parseInfo, queries);
					conditions.Add(new SqlQuery.Condition(false, predicate));
					break;
			}

			ParsingTracer.DecIndentLevel();
		}

		#region ParsePredicateSubQuery

		SqlQuery.Condition ParseConditionSubQuery(ParseInfo expr, params QuerySource[] queries)
		{
			SqlQuery.Condition cond = null;

			if (expr.Expr.NodeType == ExpressionType.Call)
			{
				Func<SqlQuery.Condition> func = null;

				expr.ConvertTo<MethodCallExpression>().Match
				(
					pi => pi.IsQueryableMethod(pexpr =>
					{
						switch (pi.Expr.Method.Name)
						{
							case "Any" : func = () => ParseAnyCondition(SetType.Any,  pexpr, null, null); return true;
						}
						return false;
					}),
					pi => pi.IsQueryableMethod((pexpr,l) =>
					{
						switch (pi.Expr.Method.Name)
						{
							case "Any" : func = () => ParseAnyCondition(SetType.Any, pexpr, l, null); return true;
							case "All" : func = () => ParseAnyCondition(SetType.All, pexpr, l, null); return true;
						}
						return false;
					}),
					pi =>
					{
						ParseInfo s = null;
						return pi.IsQueryableMethod2("Contains", seq => s = seq, ex =>
						{
							func = () =>
							{
								var param  = Expression.Parameter(ex.Expr.Type, ex.NodeType == ExpressionType.Parameter ? ((ParameterExpression)ex).Name : "t");
								var lambda = new LambdaInfo(ParseInfo.CreateRoot(Expression.Equal(param, ex), null), ParseInfo.CreateRoot(param, null));
								return ParseAnyCondition(SetType.In, s, lambda, ex);
							};
							return true;
						});
					}
				);

				if (func != null)
				{
					var parentQueries = queries.Select(q => new ParentQuery { Parent = q, Parameter = q.Lambda.Parameters.FirstOrDefault()}).ToList();

					_parentQueries.InsertRange(0, parentQueries);

					cond = func();

					_parentQueries.RemoveRange(0, parentQueries.Count);
				}
			}

			return cond;
		}

		#endregion

		#endregion

		#region ParentQueries

		class ParentQuery
		{
			public QuerySource                    Parent;
			public ParseInfo<ParameterExpression> Parameter;
		}

		readonly List<ParentQuery> _parentQueries = new List<ParentQuery>();

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
				foreach (var query in _parentQueries)
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

			foreach (var query in _parentQueries)
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
						{
							var p = lambda.Parameters[i];
							if (p.Expr == expr)
								return queries[i];
						}

						foreach (var query in _parentQueries)
							if (query.Parameter == expr)
								return query.Parent;
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expr;

						if (IsListCountMember(ma.Member))
							return null;

						if (lambda != null && lambda.Parameters.Length > 0 && ma.Expression == lambda.Parameters[0].Expr)
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

			foreach (var query in _parentQueries)
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

		bool HasSource(QuerySource query, QuerySource source)
		{
			if (source == null)  return false;
			if (source == query) return true;

			foreach (var s in query.Sources)
				if (HasSource(s, source))
					return true;

			return false;
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

		ParseInfo ConvertNew(ParseInfo<NewExpression> pi)
		{
			var lambda = ConvertMember(pi.Expr.Constructor);

			if (lambda != null)
			{
				var ef    = UnwrapLambda(lambda.Body);
				var parms = new Dictionary<string,int>(lambda.Parameters.Count);
				var pn    = 0;

				foreach (var p in lambda.Parameters)
					parms.Add(p.Name, pn++);

				return pi.Parent.Replace(ef, null).Walk(wpi =>
				{
					if (wpi.NodeType == ExpressionType.Parameter)
					{
						var pe   = (ParameterExpression)wpi.Expr;
						var n    = parms[pe.Name];
						var expr = pi.Expr.Arguments[n];

						Func<Expression> fparam = () => pi.Index(pi.Expr.Arguments, New.Arguments, n);

						if (expr.NodeType == ExpressionType.MemberAccess)
							if (!_accessors.ContainsKey(expr))
								_accessors.Add(expr, fparam());

						return pi.Create(expr, null);
					}

					return wpi;
				});
			}

			return null;
		}

		ParseInfo ConvertMethod(ParseInfo<MethodCallExpression> pi)
		{
			var e = pi.Expr;
			var l = ConvertMember(e.Method);

			if (l == null)
				return null;

			var ef    = UnwrapLambda(l.Body);
			var parms = new Dictionary<string,int>(l.Parameters.Count);
			var pn    = e.Method.IsStatic ? 0 : -1;

			foreach (var p in l.Parameters)
				parms.Add(p.Name, pn++);

			var pie = pi.Parent.Replace(ef, null).Walk(wpi =>
			{
				if (wpi.NodeType == ExpressionType.Parameter)
				{
					Expression expr;

					var pe = (ParameterExpression)wpi.Expr;
					int n;

					if (parms.TryGetValue(pe.Name, out n))
					{
						Func<Expression> fparam;

						if (n < 0)
						{
							expr   = e.Object;
							fparam = () => pi.Property(MethodCall.Object);
						}
						else
						{
							expr   = e.Arguments[n];
							fparam = () => pi.Index(e.Arguments, MethodCall.Arguments, n);
						}

						if (expr.NodeType == ExpressionType.MemberAccess)
							if (!_accessors.ContainsKey(expr))
								_accessors.Add(expr, fparam());

						return pi.Create(expr, null);
					}
				}

				return wpi;
			});

			return pie;
		}

		#endregion
	}
}
