using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class GroupByParser : MethodCallParser
	{
		#region Parser Methods

		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			if (!methodCall.IsQueryable("GroupBy"))
				return false;

			var body = ((LambdaExpression)methodCall.Arguments[1].Unwrap()).Body.Unwrap();

			if (body.NodeType == ExpressionType	.MemberInit)
			{
				var mi = (MemberInitExpression)body;
				bool throwExpr;

				if (mi.NewExpression.Arguments.Count > 0 || mi.Bindings.Count == 0)
					throwExpr = true;
				else
					throwExpr = mi.Bindings.Any(b => b.BindingType != MemberBindingType.Assignment);

				if (throwExpr)
					throw new NotSupportedException(string.Format("Explicit construction of entity type '{0}' in group by is not allowed.", body.Type));
			}

			return (methodCall.Arguments[methodCall.Arguments.Count - 1].Unwrap().NodeType == ExpressionType.Lambda);
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence        = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
			var sequenceExpr    = methodCall.Arguments[0];
			var groupingType    = methodCall.Type.GetGenericArguments()[0];
			var keySelector     = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var elementSelector = (LambdaExpression)methodCall.Arguments[2].Unwrap();

			if (methodCall.Arguments[0].NodeType == ExpressionType.Call)
			{
				var call = (MethodCallExpression)methodCall.Arguments[0];

				if (call.Method.Name == "Select")
				{
					var type = ((LambdaExpression)call.Arguments[1].Unwrap()).Body.Type;

					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ExpressionParser.GroupSubQuery<,>))
					{
						sequence = new SubQueryContext(sequence);
					}
				}
			}

			var key      = new KeyContext(keySelector, sequence);
			var groupSql = parser.ParseExpressions(key, keySelector.Body.Unwrap(), ConvertFlags.Key);

#if DEBUG
			if (groupSql.Any(_ => !(_ is SqlField || _ is SqlQuery.Column)))
				throw new InvalidOperationException();
#endif

			sequence.SqlQuery.GroupBy.Items.Clear();

			foreach (var sql in groupSql)
				sequence.SqlQuery.GroupBy.Expr(sql);

			new QueryVisitor().Visit(sequence.SqlQuery.From, e =>
			{
				if (e.ElementType == QueryElementType.JoinedTable)
				{
					var jt = (SqlQuery.JoinedTable)e;
					if (jt.JoinType == SqlQuery.JoinType.Inner)
						jt.IsWeak = false;
				}
			});

			var element = new SelectContext (elementSelector, sequence);
			var groupBy = new GroupByContext(sequenceExpr, groupingType, sequence, key, element);

			return groupBy;
		}

		#endregion

		#region KeyContext

		class KeyContext : SelectContext
		{
			public KeyContext(LambdaExpression lambda, params IParseContext[] sequences)
				: base(lambda, sequences)
			{
			}
		}

		#endregion

		#region GroupByContext

		class GroupByContext : SequenceContextBase
		{
			public GroupByContext(Expression sequenceExpr, Type groupingType, IParseContext sequence, KeyContext key, SelectContext element)
				: base(sequence, null)
			{
				_sequenceExpr = sequenceExpr;
				_key          = key;
				_element      = element;
				_groupingType = groupingType;

				key.Parent = this;
			}

			readonly Expression    _sequenceExpr;
			readonly KeyContext    _key;
			readonly SelectContext _element;
			readonly Type          _groupingType;

			class Grouping<TKey,TElement> : IGrouping<TKey,TElement>
			{

				public Grouping(TKey key, QueryContext queryContext, Func<IDataContext,TKey,IQueryable<TElement>> itemReader)
				{
					Key = key;

					if (Common.Configuration.Linq.PreloadGroups)
					{
						_items = GetItems(queryContext, itemReader);
					}
					else
					{
						_queryContext = queryContext;
						_itemReader   = itemReader;
					}
				}

				private  IList<TElement>                              _items;
				readonly QueryContext                                 _queryContext;
				readonly Func<IDataContext,TKey,IQueryable<TElement>> _itemReader;

				public TKey Key { get; private set; }

				List<TElement> GetItems(QueryContext queryContext, Func<IDataContext,TKey,IQueryable<TElement>> elementReader)
				{
					var db = queryContext.GetDataContext();

					try
					{
						return elementReader(db.DataContextInfo.DataContext, Key).ToList();
					}
					finally
					{
						queryContext.ReleaseDataContext(db);
					}
				}

				public IEnumerator<TElement> GetEnumerator()
				{
					if (_items == null)
						_items = GetItems(_queryContext, _itemReader);

					return _items.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}
			}

			interface IGroupByHelper
			{
				Expression GetGrouping(GroupByContext context);
			}

			class GroupByHelper<TKey,TElement,TSource> : IGroupByHelper
			{
				public Expression GetGrouping(GroupByContext context)
				{
					var keyParam = Expression.Parameter(typeof(TKey), "key");

					var expr = Expression.Call(
						null,
// ReSharper disable AssignNullToNotNullAttribute
						ReflectionHelper.Expressor<object>.MethodExpressor(_ => Queryable.Where(null, (Expression<Func<TSource,bool>>)null)),
// ReSharper restore AssignNullToNotNullAttribute
						context._sequenceExpr,
						Expression.Lambda<Func<TSource,bool>>(
							Expression.Equal(context._key.Lambda.Body, keyParam),
							new[] { context._key.Lambda.Parameters[0] }));

					expr = Expression.Call(
						null,
// ReSharper disable AssignNullToNotNullAttribute
						ReflectionHelper.Expressor<object>.MethodExpressor(_ => Queryable.Select(null, (Expression<Func<TSource,TElement>>)null)),
// ReSharper restore AssignNullToNotNullAttribute
						expr,
						context._element.Lambda);

					var lambda = Expression.Lambda<Func<IDataContext,TKey,IQueryable<TElement>>>(
						Expression.Convert(expr, typeof(IQueryable<TElement>)),
						Expression.Parameter(typeof(IDataContext), "ctx"),
						keyParam);

					var itemReader = CompiledQuery.Compile(lambda);
					var keyExpr    = context._key.BuildExpression(null, 0);
					var keyReader  = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],TKey>>(
						keyExpr,
						new []
						{
							ExpressionParser.ContextParam,
							ExpressionParser.DataContextParam,
							ExpressionParser.DataReaderParam,
							ExpressionParser.ExpressionParam,
							ExpressionParser.ParametersParam,
						});

					return Expression.Call(
						null,
						ReflectionHelper.Expressor<object>.MethodExpressor(_ => GetGrouping(null, null, null, null, null, null, null)),
						new Expression[]
						{
							ExpressionParser.ContextParam,
							ExpressionParser.DataContextParam,
							ExpressionParser.DataReaderParam,
							ExpressionParser.ExpressionParam,
							ExpressionParser.ParametersParam,
							Expression.Constant(keyReader.Compile()),
							Expression.Constant(itemReader),
						});
				}

				static IGrouping<TKey,TElement> GetGrouping(
					QueryContext             context,
					IDataContext             dataContext,
					IDataReader              dataReader,
					Expression               expr,
					object[]                 ps,
					Func<QueryContext,IDataContext,IDataReader,Expression,object[],TKey> keyReader,
					Func<IDataContext,TKey,IQueryable<TElement>>                         itemReader)
				{
					var key = keyReader(context, dataContext, dataReader, expr, ps);
					return new Grouping<TKey,TElement>(key, context, itemReader);
				}
			}

			Expression BuildGrouping()
			{
				var gtype  = typeof(GroupByHelper<,,>).MakeGenericType(
					_key.Lambda.Body.Type,
					_element.Lambda.Body.Type,
					_key.Lambda.Parameters[0].Type);

				var helper = (IGroupByHelper)Activator.CreateInstance(gtype);

				return helper.GetGrouping(this);
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
					return BuildGrouping();

				if (level != 0)
				{
					var levelExpression = expression.GetLevelExpression(level);

					if (levelExpression.NodeType == ExpressionType.MemberAccess)
					{
						var ma = (MemberExpression)levelExpression;

						if (ma.Member.Name == "Key" && ma.Member.DeclaringType == _groupingType)
						{
							return levelExpression == expression ?
								_key.BuildExpression(null,       0) :
								_key.BuildExpression(expression, level + 1);
						}
					}
				}

				throw new NotImplementedException();
			}

			ISqlExpression ParseEnumerable(MethodCallExpression expr)
			{
				var args = new ISqlExpression[expr.Arguments.Count - 1];

				if (expr.Method.Name == "Count")
				{
					if (args.Length > 0)
					{
						var ctx = _element;
						var l   = (LambdaExpression)expr.Arguments[1].Unwrap();
						var cnt = Parser.ParseWhere(ctx, l, false);
						var sql = cnt.SqlQuery.Clone((_ => !(_ is SqlParameter)));

						sql.ParentSql = SqlQuery;
						sql.Select.Columns.Clear();

						if (ctx == cnt)
							ctx.SqlQuery.Where.SearchCondition.Conditions.RemoveAt(ctx.SqlQuery.Where.SearchCondition.Conditions.Count - 1);

						if (Parser.SqlProvider.IsSubQueryColumnSupported && Parser.SqlProvider.IsCountSubQuerySupported)
						{
							for (var i = 0; i < sql.GroupBy.Items.Count; i++)
							{
								var item1 = sql.GroupBy.Items[i];
								var item2 = SqlQuery.GroupBy.Items[i];
								var pr    = Parser.Convert(this, new SqlQuery.Predicate.ExprExpr(item1, SqlQuery.Predicate.Operator.Equal, item2));

								sql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, pr));
							}

							sql.GroupBy.Items.Clear();
							sql.Select.Expr(SqlFunction.CreateCount(expr.Type, sql));

							return sql;
						}

						var join = sql.WeakLeftJoin();

						SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);

						for (var i = 0; i < sql.GroupBy.Items.Count; i++)
						{
							var item1 = sql.GroupBy.Items[i];
							var item2 = SqlQuery.GroupBy.Items[i];
							var col   = sql.Select.Columns[sql.Select.Add(item1)];
							var pr    = Parser.Convert(this, new SqlQuery.Predicate.ExprExpr(col, SqlQuery.Predicate.Operator.Equal, item2));

							join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, pr));
						}

						return new SqlFunction(expr.Type, "Count", sql.Select.Columns[0]);
					}

					return SqlFunction.CreateCount(expr.Type, SqlQuery);
				}

				if (expr.Arguments.Count > 1)
				{
					for (var i = 1; i < expr.Arguments.Count; i++)
					{
						var ex = expr.Arguments[i].Unwrap();

						if (ex is LambdaExpression)
						{
							var l   = (LambdaExpression)ex;
							var ctx = new PathThroughContext(_element, l);

							args[i - 1] = Parser.ParseExpression(ctx, l.Body.Unwrap());
						}
						else
						{
							throw new NotImplementedException();
						}
					}
				}
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
									args = new[] { Parser.ParseExpression(this, l.Body) };
								}

								return false;
							}))
							{}
						}
					}
					else //if (query.ElementSource is QuerySource.Scalar)
					{
						args = _element.ConvertToSql(null, 0, ConvertFlags.Field);

						//var scalar = (QuerySource.Scalar)query.ElementSource;
						//args = new[] { scalar.GetExpressions(this)[0] };
					}
				}

				return new SqlFunction(expr.Type, expr.Method.Name, args);
			}

			PropertyInfo _keyProperty;

			public override ISqlExpression[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (level > 0)
				{
					switch (expression.NodeType)
					{
						case ExpressionType.Call         :
							{
								var e = (MethodCallExpression)expression;

								if (e.Method.DeclaringType == typeof(Enumerable))
								{
									return new[] { ParseEnumerable(e) };
								}

								break;
							}

						case ExpressionType.MemberAccess :
							{
								if (level != 0)
								{
									var levelExpression = expression.GetLevelExpression(level);

									if (levelExpression.NodeType == ExpressionType.MemberAccess)
									{
										var e = (MemberExpression)levelExpression;

										if (e.Member.Name == "Key")
										{
											if (_keyProperty == null)
												_keyProperty = _groupingType.GetProperty("Key");

											if (e.Member == _keyProperty)
											{
												if (levelExpression == expression)
													return _key.ConvertToSql(null, 0, flags);

												return _key.ConvertToSql(expression, level + 1, flags);
											}
										}
									}
								}

								break;
							}
					}
				}

				throw new NotImplementedException();
			}

			public override int[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				return false;
			}

			public override int ConvertToParentIndex(int index, IParseContext context)
			{
				var expr = SqlQuery.Select.Columns[index].Expression;

				if (!SqlQuery.GroupBy.Items.Exists(_ => _ == expr))
					SqlQuery.GroupBy.Items.Add(expr);

				return base.ConvertToParentIndex(index, context);
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
