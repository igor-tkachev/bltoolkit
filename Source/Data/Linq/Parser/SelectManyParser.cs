using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class SelectManyParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return
				methodCall.IsQueryable("SelectMany") &&
				methodCall.Arguments.Count == 3      &&
				((LambdaExpression)methodCall.Arguments[1].Unwrap()).Parameters.Count == 1;
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence           = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);
			var collectionSelector = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var resultSelector     = (LambdaExpression)methodCall.Arguments[2].Unwrap();

			//if (collectionSelector.Parameters[0] == collectionSelector.Body.Unwrap())
			//{
			//	return resultSelector == null ? sequence : new SelectContext(resultSelector, sequence, sequence);
			//}

			var context    = new SelectManyContext(parent, collectionSelector, sequence);
			var expr       = collectionSelector.Body.Unwrap();
			var crossApply = null != expr.Find(e => e == collectionSelector.Parameters[0]);

			/*
			if (expr.NodeType == ExpressionType.Call)
			{
				var call = (MethodCallExpression)collectionSelector.Body;

				if (call.IsQueryable("DefaultIfEmpty"))
				{
					leftJoin = true;
					expr     = call.Arguments[0];
				}
			}
			*/

			var collectionSql = new SqlQuery();

			parser.SubQueryParsingCounter++;
			var collection = parser.ParseSequence(context, expr, collectionSql);
			parser.SubQueryParsingCounter--;

			var leftJoin = collection is DefaultIfEmptyParser.DefaultIfEmptyContext;
			var sql      = collection.SqlQuery;

			if (!leftJoin && crossApply)
			{
				if (sql.GroupBy.IsEmpty &&
					sql.Select.Columns.Count == 0 &&
					!sql.Select.HasModifier &&
					sql.Where.IsEmpty &&
					!sql.HasUnion &&
					sql.From.Tables.Count == 1)
				{
					crossApply = false;
				}
			}

			if (collectionSql != sql)
			{
				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);
				return new SelectContext(parent, resultSelector, sequence, context);
			}

			if (!leftJoin && !crossApply)
			{
				sequence.SqlQuery.From.Table(sql);

				//sql.ParentSql = sequence.SqlQuery;

				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, true);

				return new SelectContext(parent, resultSelector, sequence, context);
			}

			if (leftJoin && collectionSql != sql)
			{
				context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);
				return new SelectContext(parent, resultSelector, sequence, context);
			}

			//if (crossApply)
			{
				if (sql.GroupBy.IsEmpty &&
					sql.Select.Columns.Count == 0 &&
					!sql.Select.HasModifier &&
					//!sql.Where.IsEmpty &&
					!sql.HasUnion && sql.From.Tables.Count == 1)
				{
					var join = leftJoin ? SqlQuery.LeftJoin(sql) : SqlQuery.InnerJoin(sql);

					join.JoinedTable.Condition.Conditions.AddRange(sql.Where.SearchCondition.Conditions);

					sql.Where.SearchCondition.Conditions.Clear();

					if (collection is TableParser.TableContext)
					{
						var collectionParent = collection.Parent as TableParser.TableContext;

						// Association.
						//
						if (collectionParent != null)
						{
							var ts = (SqlQuery.TableSource)new QueryVisitor().Find(sequence.SqlQuery.From, e =>
							{
								if (e.ElementType == QueryElementType.TableSource)
								{
									var t = (SqlQuery.TableSource)e;
									return t.Source == collectionParent.SqlTable;
								}

								return false;
							});

							ts.Joins.Add(join.JoinedTable);
						}
						else
						{
							sequence.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);
						}
					}
					else
					{
						sequence.SqlQuery.From.Tables[0].Joins.Add(join.JoinedTable);
						//collection.SqlQuery = sequence.SqlQuery;
					}

					//sql.ParentSql = sequence.SqlQuery;

					context.Collection = new SubQueryContext(collection, sequence.SqlQuery, false);

					return new SelectContext(parent, resultSelector, sequence, context);
				}
			}

			throw new LinqException("Sequence '{0}' cannot be converted to SQL.", expr);
		}

		public class SelectManyContext : SelectContext
		{
			public SelectManyContext(IParseContext parent, LambdaExpression lambda, IParseContext sequence)
				: base(parent, lambda, sequence)
			{
			}

			private IParseContext _collection;
			public  IParseContext  Collection
			{
				get { return _collection; }
				set
				{
					_collection = value;
					_collection.Parent = this;
				}
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
					return Collection.BuildExpression(expression, level);

				var root = expression.GetRootObject();

				if (root == Lambda.Parameters[0])
					return base.BuildExpression(expression, level);

				return Collection.BuildExpression(expression, level);
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				if (Collection == null)
					base.BuildQuery(query, queryParameter);

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.ConvertToIndex(expression, level, flags);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.ConvertToIndex(expression, level, flags);
				}

				return base.ConvertToIndex(expression, level, flags);
			}

			/*
			public override int ConvertToParentIndex(int index, IParseContext context)
			{
				if (Collection == null)
					return base.ConvertToParentIndex(index, context);

				throw new NotImplementedException();
			}
			*/

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.ConvertToSql(expression, level, flags);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.ConvertToSql(expression, level, flags);
				}

				return base.ConvertToSql(expression, level, flags);
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.GetContext(expression, level, currentSql);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.GetContext(expression, level, currentSql);
				}

				return base.GetContext(expression, level, currentSql);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				if (Collection != null)
				{
					if (expression == null)
						return Collection.IsExpression(expression, level, requestFlag);

					var root = expression.GetRootObject();

					if (root != Lambda.Parameters[0])
						return Collection.IsExpression(expression, level, requestFlag);
				}

				return base.IsExpression(expression, level, requestFlag);
			}
		}
	}
}
